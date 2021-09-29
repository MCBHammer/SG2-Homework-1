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
    [SerializeField] float _chargeTime = 0.5f;

    float _prob;
    Boss _boss;

    // Start is called before the first frame update
    private void Awake()
    {
        _tr = this.gameObject.transform;
        _boss = FindObjectOfType<Boss>();
        float _waitTime = Random.Range(_lowCooldown, _highCooldown);
        StartCoroutine("BFire", _waitTime);
        //Debug.Log(_tr);
    }

    private void OnEnable()
    {
        _boss.Attack2Start += LockFire;
        _boss.Attack2Stop += UnlockFire;
    }

    private void OnDisable()
    {
        _boss.Attack2Start -= LockFire;
        _boss.Attack2Stop -= UnlockFire;
    }

    private void Update()
    {
        
    }

    public override void Fire()
    {
        FireFeedback();
        //Debug.Log(_prob);
        if (_prob >= _specialProjChance)
        {
            Instantiate(_projectile, _tr.position + _tr.forward * 5, Quaternion.Euler(0, _tr.rotation.eulerAngles.y, 0));
            Projectile _projSet = _projectile.GetComponent<BossProjectile>();
            //Debug.Log(_projSet.MoveSpeed);
        } else
        {
            Instantiate(_specialProj, _tr.position + _tr.forward * 5, Quaternion.Euler(0, _tr.rotation.eulerAngles.y, 0));
            Projectile _projSet = _specialProj.GetComponent<BossProjectile>();
            //Debug.Log(_projSet.MoveSpeed);
        }
        
        /*
        float _speedSet = this.gameObject.GetComponent<Rigidbody>().velocity.magnitude;
        _projSet.MoveSpeed = _speedSet * _projSpeed;
        Debug.Log(_speedSet);
        */
    }

    private IEnumerator BFire(float WaitTime)
    {
        _prob = Random.value;
        yield return new WaitForSeconds(WaitTime - _chargeTime);

        GameObject _BossTurret = this.transform.Find("Art/Turret").gameObject;
        Color _BossMaterial = _BossTurret.GetComponent<Renderer>().material.color;
        Color _projMaterial = _projectile.transform.Find("Art/Sphere").gameObject.GetComponent<Renderer>().sharedMaterial.color;
        Color _specialProjMaterial = _specialProj.transform.Find("Art/Sphere").gameObject.GetComponent<Renderer>().sharedMaterial.color;
        Color _currentColor;

        float currentTime = 0;
        while (currentTime < _chargeTime)
        {
            currentTime += Time.deltaTime;
            float t = currentTime / _chargeTime;
            if(_prob >= _specialProjChance)
            {
                _currentColor = Color.Lerp(_BossMaterial, _projMaterial, t);
            } else
            {
                _currentColor = Color.Lerp(_BossMaterial, _specialProjMaterial, t);
            }
            _BossTurret.GetComponent<Renderer>().material.color = _currentColor;
            yield return null;
        }

        yield return new WaitForSeconds(_chargeTime);
        Fire();
        _BossTurret.GetComponent<Renderer>().material.color = _BossMaterial;
        float _waitTime = Random.Range(_lowCooldown, _highCooldown);
        StartCoroutine("BFire", _waitTime);
    }

    private void LockFire()
    {
        StopCoroutine("BFire");
    }

    private void UnlockFire()
    {
        float _waitTime = Random.Range(_lowCooldown, _highCooldown);
        StartCoroutine("BFire", _waitTime);
    }
}
