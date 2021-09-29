using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossStun : BossProjectile
{
    protected override void OnCollisionEnter(Collision collision)
    {
        TankController _tr = collision.gameObject.GetComponent<TankController>();
        FireController _fr = collision.gameObject.GetComponent<FireController>();
        if (_tr != null && _fr != null)
        {
            _tr.Stun();
            _fr.Stun();
        }
        StartCoroutine("ProjectileDestroy", _waitTime);
    }
}
