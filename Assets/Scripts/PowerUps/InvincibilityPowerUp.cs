using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibilityPowerUp : PowerUpBase
{
    //[SerializeField] Renderer TankBody;
    //[SerializeField] Renderer TankTurret;

    [SerializeField] Material Invincible;
    [SerializeField] Material Vulnerable;

    Renderer TankBody;
    Renderer TankTurret;

    private void Start()
    {
        TankBody = GameObject.Find("Tank/Art/Body").GetComponent<Renderer>();
        TankTurret = GameObject.Find("Tank/Art/Turret").GetComponent<Renderer>();
    }
    protected override void PowerUp(Player player)
    {
        
        TankBody.material = Invincible;
        TankTurret.material = Invincible;
        player.isInvincible = true;
    }

    protected override void PowerDown(Player player)
    {
        TankBody.material = Vulnerable;
        TankTurret.material = Vulnerable;
        player.isInvincible = false;
    }
}
