using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    [SerializeField] float _moveSpeed = .25f;
    [SerializeField] float _stunLength = 2;
    [SerializeField] Material _stunMaterial;
    GameObject _tankBody;
    GameObject _tankTurret;
    Material _tankMaterial;
    bool _isStunned = false;
    public float MoveSpeed
    {
        get => _moveSpeed;
        set
        {
            value = Mathf.Clamp(value, 0.1f, 3);
            _moveSpeed = value;
        }
    }

    [SerializeField] float _turnSpeed = 2f;

    Rigidbody _rb = null;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _tankBody = this.transform.Find("Art/Body").gameObject;
        _tankTurret = this.transform.Find("Art/Turret").gameObject;
        _tankMaterial = _tankBody.GetComponent<Renderer>().material;
    }

    private void FixedUpdate()
    {
        if(_isStunned == false)
        {
            MoveTank();
            TurnTank();
        }
    }

    public void MoveTank()
    {
        // calculate the move amount
        float moveAmountThisFrame = Input.GetAxis("Vertical") * _moveSpeed;
        // create a vector from amount and direction
        Vector3 moveOffset = transform.forward * moveAmountThisFrame;
        // apply vector to the rigidbody
        _rb.MovePosition(_rb.position + moveOffset);
        // technically adjusting vector is more accurate! (but more complex)
    }

    public void TurnTank()
    {
        // calculate the turn amount
        float turnAmountThisFrame = Input.GetAxis("Horizontal") * _turnSpeed;
        // create a Quaternion from amount and direction (x,y,z)
        Quaternion turnOffset = Quaternion.Euler(0, turnAmountThisFrame, 0);
        // apply quaternion to the rigidbody
        _rb.MoveRotation(_rb.rotation * turnOffset);
    }

    public void Stun()
    {
        StartCoroutine(StunTimer(_stunLength));
    }

    private IEnumerator StunTimer(float _stunTime)
    {
        _isStunned = true;
        if(_stunMaterial != null)
        {
            _tankBody.GetComponent<Renderer>().material = _stunMaterial;
            _tankTurret.GetComponent<Renderer>().material = _stunMaterial;
        }
        yield return new WaitForSeconds(_stunTime);
        _isStunned = false;
        _tankBody.GetComponent<Renderer>().material = _tankMaterial;
        _tankTurret.GetComponent<Renderer>().material = _tankMaterial;
    }
}
