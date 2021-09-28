using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenEffects : MonoBehaviour
{
    [SerializeField] GameObject _player;
    Health _playerHealth;

    private void Awake()
    {
        _playerHealth = _player.GetComponent<Health>();
    }

    private void OnEnable()
    {
        _playerHealth.TookDamage += CameraEffects;
    }

    private void OnDisable()
    {
        _playerHealth.TookDamage -= CameraEffects;
    }

    void CameraEffects(int _damage)
    {
        StartCoroutine(CameraShake(0.75f, 0.6f, _damage));
        Debug.Log("It's Working!");
    }

    //A lot of camera shake examples are done in Update, and this was the best coroutine one I could find. Works for me! https://www.gamedeveloper.com/business/different-ways-of-shaking-camera-in-unity
    private IEnumerator CameraShake(float duration, float magnitude, float damageScale)
    {
        Vector3 orignalPosition = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude * damageScale;
            float y = Random.Range(-1f, 1f) * magnitude * damageScale;

            transform.position = new Vector3(orignalPosition.x + x, orignalPosition.y + y, orignalPosition.z);
            elapsed += Time.deltaTime;
            yield return 0;
            //Debug.Log(elapsed);
        }
        transform.position = orignalPosition;
    }
}
