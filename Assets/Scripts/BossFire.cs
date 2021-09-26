using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFire : FireController
{

    [SerializeField] float _lowCooldown = 4f;
    [SerializeField] float _highCooldown = 7f;
    // Start is called before the first frame update
    private void Awake()
    {
        float _waitTime = Random.Range(_lowCooldown, _highCooldown);
        StartCoroutine("BFire", _waitTime);
    }

    public override void Fire()
    {
        FireFeedback();
        Instantiate(_projectile, _tr.position + _tr.forward * 2, Quaternion.Euler(0, _tr.rotation.eulerAngles.y, 0));
        Projectile _projSet = _projectile.GetComponent<Projectile>();
        float _speedSet = this.gameObject.GetComponent<Rigidbody>().velocity.magnitude;
        _projSet.MoveSpeed = _speedSet * 2;
    }

    private IEnumerator BFire(float WaitTime)
    {
        yield return new WaitForSeconds(WaitTime);
        Fire();
        float _waitTime = Random.Range(_lowCooldown, _highCooldown);
        StartCoroutine("BFire", _waitTime);
    }
}
