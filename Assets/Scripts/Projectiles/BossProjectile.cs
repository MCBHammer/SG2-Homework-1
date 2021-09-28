using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : Projectile
{
    protected override void LockOn()
    {
        Player player = FindObjectOfType<Player>();
        if(player != null)
            _target = player.gameObject;
        Debug.Log(_target.gameObject.name);
    }
}
