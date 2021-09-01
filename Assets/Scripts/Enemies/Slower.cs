using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slower : Enemy
{
    [SerializeField] float _slowAmount = 0.125f;
    protected override void PlayerImpact(Player player)
    {
        TankController controller = player.gameObject.GetComponent<TankController>();
        if (controller != null)
        {
            controller.MoveSpeed -= _slowAmount;
        }
    }
}
