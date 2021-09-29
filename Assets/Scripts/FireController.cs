using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour
{
    [SerializeField] public GameObject _projectile;
    [SerializeField] ParticleSystem _fireParticles;
    [SerializeField] AudioClip _fireSound;
    [SerializeField] float _fireCooldown = 0.5f;
    [SerializeField] float _stunLength = 2;
    bool _isStunned = false;
    bool coolDownBool = false;
    protected Transform _tr;

    private void Awake()
    {
        _tr = this.gameObject.transform;
    }

    private void Update()
    {
        //in Update instead of FixedUpdate so input works properly every time
        if (Input.GetKeyDown(KeyCode.Space) && coolDownBool == false && _isStunned == false)
        {
            Fire();
            StartCoroutine("FireCooldown", _fireCooldown);
        }
    }

    public virtual void Fire()
    {
        //This is convoluted because I want to reuse for the boss
        FireFeedback();
        Instantiate(_projectile, _tr.position + _tr.forward * 2, Quaternion.Euler(0, _tr.rotation.eulerAngles.y, 0));
        Projectile _projSet = _projectile.GetComponent<Projectile>();
        TankController _tankSet = this.gameObject.GetComponent<TankController>();
        _projSet.MoveSpeed = _tankSet.MoveSpeed * 2;
    }

    private IEnumerator FireCooldown(float WaitTime)
    {
        coolDownBool = true;
        yield return new WaitForSeconds(WaitTime);
        coolDownBool = false;
    }

    protected virtual void FireFeedback()
    {
        //particles
        if (_fireParticles != null)
        {
            //_impactParticles = Instantiate(_impactParticles, transform.position, Quaternion.identity);
            ParticleHelper.particlePlay(_fireParticles);
        }
        //audio TODO - consider Object Pooling for performance
        if (_fireSound != null)
        {
            AudioHelper.PlayClip2D(_fireSound, .15f);
        }
    }

    public void Stun()
    {
        StartCoroutine(StunTimer(_stunLength));
    }

    private IEnumerator StunTimer(float _stunTime)
    {
        _isStunned = true;
        yield return new WaitForSeconds(_stunTime);
        _isStunned = false;
    }
}
