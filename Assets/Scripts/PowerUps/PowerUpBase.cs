using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUpBase : MonoBehaviour
{
    protected abstract void PowerUp(Player player);
    protected abstract void PowerDown(Player player);
    [SerializeField] float _powerupDuration = 5f;

    [SerializeField] float _movementSpeed = 1;
    protected float MovementSpeed => _movementSpeed;

    [SerializeField] ParticleSystem _powerupParticles;
    [SerializeField] AudioClip _powerupSound;

    protected Rigidbody _rb;
    Player player;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Movement(_rb);
    }

    protected virtual void Movement(Rigidbody rb)
    {
        //calculate rotation
        Quaternion turnOffset = Quaternion.Euler(0, _movementSpeed, 0);
        rb.MoveRotation(_rb.rotation * turnOffset);
    }

    private void OnTriggerEnter(Collider other)
    {
        player = other.gameObject.GetComponent<Player>();
        if (player != null)
        {
            PowerUp(player);
            //spawn particles and SFX because we need to disable object
            //collider.enabled = false;
            GetComponent<Collider>().enabled = false;
            GetComponent<Renderer>().enabled = false;
            Feedback();
            StartCoroutine("Timer", _powerupDuration);           
        }
    }

    private IEnumerator Timer(int duration)
    {
        yield return new WaitForSeconds(duration);
        PowerDown(player);
        gameObject.SetActive(false);
    }

    private void Feedback()
    {
        //particles
        if (_powerupParticles != null)
        {
            _powerupParticles = Instantiate(_powerupParticles, transform.position, Quaternion.identity);
        }
        //audio TODO - consider Object Pooling for performance
        if (_powerupSound != null)
        {
            AudioHelper.PlayClip2D(_powerupSound, .5f);
        }
    }
}
