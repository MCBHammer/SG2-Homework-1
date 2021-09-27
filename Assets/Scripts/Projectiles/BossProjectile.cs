﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : Projectile
{
    protected override void LockOn()
    {
        Player player = FindObjectOfType<Player>();
        _target = player.gameObject;
        Debug.Log(_target.gameObject.name);
    }
}
