using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 2;
    [SerializeField] float _turnSpeed = 5;
    [SerializeField] int _damage = 1;
    [SerializeField] AudioClip _impactSound;
    [SerializeField] ParticleSystem _impactParticles;
    public float _waitTime = 1;
    public float MoveSpeed
    {
        get { return _moveSpeed; }
        set { _moveSpeed = value; }
    }
    Rigidbody _rb;
    GameObject _target;
    float posY;

    private void OnEnable()
    {
        _rb = GetComponent<Rigidbody>();
        LockOn();
        posY = this.transform.position.y;
    }
    private void FixedUpdate()
    {
        Move();
        //Rotate();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Health _damageInterface = collision.gameObject.GetComponent<Health>();
        if(_damageInterface != null)
        {
            _damageInterface.TakeDamage(_damage);
        }
        StartCoroutine("ProjectileDestroy", _waitTime);
    }

    private IEnumerator ProjectileDestroy(float WaitTime)
    {
        GameObject _visual = this.transform.Find("Art").gameObject;
        Collider _collider = this.gameObject.GetComponent<Collider>();
        _visual.SetActive(false);
        _collider.enabled = false;
        ImpactFeedback();
        yield return new WaitForSeconds(WaitTime);
        Destroy(this.gameObject);
    }

    private void ImpactFeedback()
    {
        //particles
        if (_impactParticles != null)
        {
            //_impactParticles = Instantiate(_impactParticles, transform.position, Quaternion.identity);
            ParticleHelper.particlePlay(_impactParticles);
        }
        //audio TODO - consider Object Pooling for performance
        if (_impactSound != null)
        {
            AudioHelper.PlayClip2D(_impactSound, .5f);
        }
    }

    protected virtual void Move()
    {
        //transform.Translate(-_moveSpeed * Time.deltaTime, 0, 0);
        Vector3 moveOffset = transform.up * _moveSpeed /** Time.fixedDeltaTime*/;
        _rb.MovePosition(_rb.position + moveOffset);
    }

    //WIP, this is tough
    /*
    protected virtual void Rotate()
    {
        Vector3 _targetDirection = _target.transform.position - this.gameObject.transform.position;
        float _rotateStep = _turnSpeed * Time.deltaTime;
        Vector3 _newDirection = Vector3.RotateTowards(transform.right, _targetDirection, _rotateStep, 0f);
        Debug.DrawRay(transform.position, _newDirection, Color.red);
        this.transform.rotation = Quaternion.LookRotation(_newDirection);
    }
    */

    private void LockOn()
    {
        //found some neat code online, trying to rewrite it so I can better understand it. Thanks https://www.codegrepper.com/code-examples/csharp/unity+find+nearest+object
        //using boss instead of enemy because I already have an enemy script from homework 1 and I don't want to delete it
        float closestBossDistance = Mathf.Infinity;
        Boss closestBoss = null;
        Boss[] allBosses = GameObject.FindObjectsOfType<Boss>();

        foreach (Boss currentBoss in allBosses)
        {
            float distanceToBoss = (currentBoss.transform.position - this.transform.position).sqrMagnitude;
            if(distanceToBoss < closestBossDistance)
            {
                closestBossDistance = distanceToBoss;
                closestBoss = currentBoss;
            }
        }
        _target = closestBoss.gameObject;
        Debug.Log(_target.gameObject.name);
    }
}