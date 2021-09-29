using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class Boss : MonoBehaviour
{
    [SerializeField] float _moveSpeed = .25f;
    [SerializeField] float _wanderRadius = 8f;
    [SerializeField] float _detectDistance = 5;
    [SerializeField] AudioClip _trampleSound;
    [SerializeField] AudioClip _attack2Sound;

    [SerializeField] float _attack2Time = 2;
    [SerializeField] float _runAwayTime = 4;
    [SerializeField] float _attackRadius = 8f;
    bool _isAttack2 = false;

    public event Action Attack2Start;
    public event Action Attack2Stop;

    public bool Attack2Bool
    {
        get => _isAttack2;
        set {; }
    }

    NavMeshAgent agent;

    Player _player;
    public float MoveSpeed
    {
        get => _moveSpeed;
        set
        {
            value = Mathf.Clamp(value, 0.1f, 3);
            _moveSpeed = value;
        }
    }

    [SerializeField] float _turnSpeed = 2f;

    Rigidbody _rb = null;

    //Testing NavMesh
    //public Transform goal;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _player = FindObjectOfType<Player>();

        //When working on randomization, reference https://answers.unity.com/questions/475066/how-to-get-a-random-point-on-navmesh.html for help
        agent = GetComponent<NavMeshAgent>();
        /*
        agent.destination = goal.position;
        */

        resetTarget();
    }

    private void OnEnable()
    {
        Attack2Start += StopMoving;
        Attack2Stop += StartMoving;
    }

    private void OnDisable()
    {
        Attack2Start -= StopMoving;
        Attack2Stop -= StartMoving;
    }


    private void FixedUpdate()
    {
        MoveTank();
        TurnTank();

        float _distance = agent.remainingDistance;
        if(_distance != Mathf.Infinity && agent.pathStatus == NavMeshPathStatus.PathComplete && _distance <= 5.5)
        {
            if(_isAttack2 == false)
            {
                //check if close enough to target
                resetTarget();
                //Debug.Log("It was me!");
            } 
        }
        if(agent.pathStatus == NavMeshPathStatus.PathPartial || agent.pathStatus == NavMeshPathStatus.PathInvalid ||_distance == Mathf.Infinity || _distance <= 8)
        {
            if(_isAttack2 == false)
            {
                //If target is too close or invalid position
                resetTarget();
                //Debug.Log("Whoops");
            }
        }

        //Keeping the if statement compact
        if(_player != null)
        {
            float _playerDistance = (_player.transform.position - this.transform.position).sqrMagnitude;
            if (_playerDistance < _detectDistance * _detectDistance && _isAttack2 == false)
            {
                StartCoroutine(Attack2(_attack2Time, _runAwayTime));
            }
        }
        //Debug.Log(agent.destination);
        //Debug.Log(agent.remainingDistance);
    }

    public void MoveTank()
    {
        //copied from TankController
        /*
        // calculate the move amount
        float moveAmountThisFrame = Input.GetAxis("Vertical") * _moveSpeed;
        // create a vector from amount and direction
        Vector3 moveOffset = transform.forward * moveAmountThisFrame;
        // apply vector to the rigidbody
        _rb.MovePosition(_rb.position + moveOffset);
        // technically adjusting vector is more accurate! (but more complex)
        */
    }

    public void TurnTank()
    {
        //copied from TankController
        /*
        // calculate the turn amount
        float turnAmountThisFrame = Input.GetAxis("Horizontal") * _turnSpeed;
        // create a Quaternion from amount and direction (x,y,z)
        Quaternion turnOffset = Quaternion.Euler(0, turnAmountThisFrame, 0);
        // apply quaternion to the rigidbody
        _rb.MoveRotation(_rb.rotation * turnOffset);
        */
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destructible _destructible = collision.gameObject.GetComponent<Destructible>();
        if(_destructible != null)
        {
            _destructible.TakeDamage(1);
            if (_trampleSound != null)
            {
                AudioHelper.PlayClip2D(_trampleSound, .5f);
            }
        }
    }

    private void resetTarget()
    {
        Vector3 _randomDirection = UnityEngine.Random.insideUnitSphere * _wanderRadius;
        _randomDirection += agent.transform.position;
        NavMeshHit _hit;
        NavMesh.SamplePosition(_randomDirection, out _hit, _wanderRadius, 1);
        Vector3 _finalPosition = _hit.position;
        agent.destination = _finalPosition;
        //Debug.Log(_finalPosition);
    }

    private IEnumerator Attack2(float _attackTime, float _runTime)
    {
        _isAttack2 = true;
        Attack2Start?.Invoke();

        //Texture Change
        float currentTime = 0;
        GameObject _BossBody = this.transform.Find("Art/Body").gameObject;
        GameObject _BossTurret = this.transform.Find("Art/Turret").gameObject;
        Color _BossMaterial = _BossBody.GetComponent<Renderer>().material.color;
        while(currentTime < _attackTime)
        {
            currentTime += Time.deltaTime;
            float t = currentTime / _attackTime;
            Color _currentColor = Color.Lerp(_BossMaterial, Color.white, t);
            _BossBody.GetComponent<Renderer>().material.color = _currentColor;
            _BossTurret.GetComponent<Renderer>().material.color = _currentColor;
            yield return null;
        }

        yield return new WaitForSeconds(_attackTime);

        AttackNearby();
        _BossBody.GetComponent<Renderer>().material.color = _BossMaterial;
        _BossTurret.GetComponent<Renderer>().material.color = _BossMaterial;
        agent.isStopped = false;
        RunAway();

        yield return new WaitForSeconds(_runTime);
        _isAttack2 = false;
        Attack2Stop?.Invoke();
    }

    private void AttackNearby()
    {
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, _attackRadius);
        for(int i  = 0; i < hitColliders.Length; i++)
        {
            IDamageable _hit = hitColliders[i].GetComponent<IDamageable>();
            Boss _boss = hitColliders[i].GetComponent<Boss>();
            if(_hit != null && _boss == null)
            {
                _hit.TakeDamage(2);
            }
        }
        if (_attack2Sound != null)
        {
            AudioHelper.PlayClip2D(_attack2Sound, .75f);
        }
    }

    private void RunAway()
    {
        Vector3 _towardsPlayer = _player.transform.position - this.transform.position;
        NavMeshHit _hit;
        NavMesh.SamplePosition(-_towardsPlayer, out _hit, 8, 1);
        Vector3 _finalPosition = _hit.position;
        agent.destination = _finalPosition;
    }

    private void StopMoving()
    {
        agent.isStopped = true;
        //Debug.Log("This is working");
    }

    private void StartMoving()
    {
        resetTarget();
    }
}
