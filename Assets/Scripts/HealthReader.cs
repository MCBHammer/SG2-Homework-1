using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthReader : MonoBehaviour
{
    [SerializeField] GameObject _trackedObject;

    float _healthValue;
    Health _healthDetected;
    Slider _healthBar;

    private void Awake()
    {
        _healthBar = GetComponent<Slider>();
        _healthDetected = _trackedObject.GetComponent<Health>();
    }

    private void OnEnable()
    {
        _healthDetected.TookDamage += HealthUpdate;
    }

    private void OnDisable()
    {
        _healthDetected.TookDamage -= HealthUpdate;
    }

    private void Start()
    {
         _healthValue = _healthDetected.HealthValue;
        _healthBar.maxValue = _healthValue;
        _healthBar.value = _healthValue;
    }
    // Update is called once per frame
    private void HealthUpdate(int _damage)
    {
        _healthBar.value -= _damage;
    }
}
