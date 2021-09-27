using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthReader : MonoBehaviour
{
    float _healthValue;
    Health _healthDetected;
    Slider _healthBar;

    private void Awake()
    {
        _healthBar = GetComponent<Slider>();
    }
    private void Start()
    {
        //_healthDetected.Health
        _healthBar.maxValue = _healthValue;
        _healthBar.value = _healthValue;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        _healthBar.value = _healthValue;
    }
}
