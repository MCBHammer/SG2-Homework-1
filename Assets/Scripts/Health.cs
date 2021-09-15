using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] int _health;
    public void TakeDamage(int _damage)
    {
        _health -= _damage;
        if(_health <= 0)
        {
            Kill();
        }
    }

    public void Kill()
    {
        Destroy(this.gameObject);
    }
}
