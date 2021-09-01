using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureIncrease : CollectibleBase
{
    [SerializeField] int _treasureValue = 1;

    protected override void Collect(Player player)
    {
        Player Player = player.GetComponent<Player>();
        Player.LootGet(_treasureValue);
    }

    protected override void Movement(Rigidbody rb)
    {
        //calculate rotation
        Quaternion turnOffset = Quaternion.Euler(MovementSpeed, 0, MovementSpeed);
        rb.MoveRotation(_rb.rotation * turnOffset);
    }

}
