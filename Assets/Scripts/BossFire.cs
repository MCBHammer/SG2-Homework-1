using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFire : FireController
{

    [SerializeField] float _lowCooldown = 4f;
    [SerializeField] float _highCooldown = 7f;
    [SerializeField] float _projSpeed = 1.5f;

    [SerializeField] GameObject _specialProj;
    [SerializeField] float _specialProjChance = 0.3f;

    // Start is called before the first frame update
    private void Awake()
    {
        _tr = this.gameObject.transform;
        float _waitTime = Random.Range(_lowCooldown, _highCooldown);
        StartCoroutine("BFire", _waitTime);
        //Debug.Log(_tr);
    }

    private void Update()
    {
        
    }

    public override void Fire()
    {
        FireFeedback();
        float _prob = Random.value;
        Debug.Log(_prob);
        if (_prob >= _specialProjChance)
        {
            Instantiate(_projectile, _tr.position + _tr.forward * 5, Quaternion.Euler(0, _tr.rotation.eulerAngles.y, 0));
            Projectile _projSet = _projectile.GetComponent<BossProjectile>();
            Debug.Log(_projSet.MoveSpeed);
        } else
        {
            Instantiate(_specialProj, _tr.position + _tr.forward * 5, Quaternion.Euler(0, _tr.rotation.eulerAngles.y, 0));
            Projectile _projSet = _specialProj.GetComponent<BossProjectile>();
            Debug.Log(_projSet.MoveSpeed);
        }
        
        /*
        float _speedSet = this.gameObject.GetComponent<Rigidbody>().velocity.magnitude;
        _projSet.MoveSpeed = _speedSet * _projSpeed;
        Debug.Log(_speedSet);
        */
    }

    private IEnumerator BFire(float WaitTime)
    {
        yield return new WaitForSeconds(WaitTime);
        Fire();
        float _waitTime = Random.Range(_lowCooldown, _highCooldown);
        StartCoroutine("BFire", _waitTime);
    }
}
