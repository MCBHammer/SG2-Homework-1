using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossStun : BossProjectile
{
    protected override void OnCollisionEnter(Collision collision)
    {
        TankController _tr = collision.gameObject.GetComponent<TankController>();
        if(_tr != null)
        {
            _tr.Stun();
        }
        StartCoroutine("ProjectileDestroy", _waitTime);
    }
}
