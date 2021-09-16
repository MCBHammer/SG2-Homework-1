using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour, IDamageable
{
    [SerializeField] ParticleSystem _damageParticles;
    float _waitTime = 1;
    public void TakeDamage(int _damage)
    {
         Kill();
    }

    public void Kill()
    {
        StartCoroutine("DestructibleDestroy", _waitTime);
    }

    private IEnumerator DestructibleDestroy(float WaitTime)
    {
        GameObject _visual = this.transform.Find("Art").gameObject;
        Collider _collider = this.gameObject.GetComponent<Collider>();
        _visual.SetActive(false);
        _collider.enabled = false;
        damageFeedback();
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
    }
}
