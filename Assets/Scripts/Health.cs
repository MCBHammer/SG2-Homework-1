using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] int _health;

    [Header("Feedback")]
    [SerializeField] ParticleSystem _damageParticles;
    [SerializeField] AudioClip _damageSound;
    [SerializeField] ParticleSystem _deathParticles;
    [SerializeField] AudioClip _deathSound;
    float _waitTime = 1;

    //Cool Event Shit
    public event Action<int> TookDamage;

    public float HealthValue
    {
        get { return _health; }
        set {}
    }

    public void TakeDamage(int _damage)
    {
        _health -= _damage;
        TookDamage?.Invoke(_damage);
        
        if(_health <= 0)
        {
            Kill();
        } else {
            damageFeedback();
        }
    }

    public void Kill()
    {
        StartCoroutine("ObjectDestroy", _waitTime);
    }

    private IEnumerator ObjectDestroy(float WaitTime)
    {
        GameObject _visual = this.transform.Find("Art").gameObject;
        Collider _collider = this.gameObject.GetComponent<Collider>();
        _visual.SetActive(false);
        _collider.enabled = false;
        deathFeedback();
        yield return new WaitForSeconds(WaitTime);
        Destroy(this.gameObject);
    }

    private void damageFeedback()
    {
        //particles
        if (_damageParticles != null)
        {
            //_impactParticles = Instantiate(_impactParticles, transform.position, Quaternion.identity);
            ParticleHelper.particlePlay(_damageParticles);
        }
        //audio TODO - consider Object Pooling for performance
        if (_damageSound != null)
        {
            AudioHelper.PlayClip2D(_damageSound, .5f);
        }
    }

    private void deathFeedback()
    {
        //particles
        if (_deathParticles != null)
        {
            //_impactParticles = Instantiate(_impactParticles, transform.position, Quaternion.identity);
            ParticleHelper.particlePlay(_deathParticles);
        }
        //audio TODO - consider Object Pooling for performance
        if (_deathSound != null)
        {
            AudioHelper.PlayClip2D(_deathSound, .5f);
        }
    }
}
