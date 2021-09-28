using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    [SerializeField] float _moveSpeed = .25f;
    [SerializeField] float _wanderRadius = 8f;
    [SerializeField] AudioClip _trampleSound;

    NavMeshAgent agent;
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

        //When working on randomization, reference https://answers.unity.com/questions/475066/how-to-get-a-random-point-on-navmesh.html for help
        agent = GetComponent<NavMeshAgent>();
        /*
        agent.destination = goal.position;
        */

        resetTarget();
    }


    private void FixedUpdate()
    {
        MoveTank();
        TurnTank();

        float _distance = agent.remainingDistance;
        if(_distance != Mathf.Infinity && agent.pathStatus == NavMeshPathStatus.PathComplete && _distance <= 5.5)
        {
            //check if close enough to target
            resetTarget();
        }
        if(agent.pathStatus == NavMeshPathStatus.PathPartial || agent.pathStatus == NavMeshPathStatus.PathInvalid ||_distance == Mathf.Infinity || _distance <= 8)
        {
            //If target is too close or invalid position
            resetTarget();
            Debug.Log("Whoops");
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
        Vector3 _randomDirection = Random.insideUnitSphere * _wanderRadius;
        _randomDirection += agent.transform.position;
        NavMeshHit _hit;
        NavMesh.SamplePosition(_randomDirection, out _hit, _wanderRadius, 1);
        Vector3 _finalPosition = _hit.position;
        agent.destination = _finalPosition;
        Debug.Log(_finalPosition);
    }
}
