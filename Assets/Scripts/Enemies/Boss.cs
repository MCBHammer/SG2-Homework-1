using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    [SerializeField] float _moveSpeed = .25f;
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

    //Testing NavMesh
    public Transform goal;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = goal.position;
    }


    private void FixedUpdate()
    {
        MoveTank();
        TurnTank();
    }

    public void MoveTank()
    {
        //copied from TankController
        /*
        // calculate the move amount
        float moveAmountThisFrame = Input.GetAxis("Vertical") * _moveSpeed;
        // create a vector from amount and direction
        Vector3 moveOffset = transform.forward * moveAmountThisFrame;
        // apply vector to the rigidbody
        _rb.MovePosition(_rb.position + moveOffset);
        // technically adjusting vector is more accurate! (but more complex)
        */
    }

    public void TurnTank()
    {
        //copied from TankController
        /*
        // calculate the turn amount
        float turnAmountThisFrame = Input.GetAxis("Horizontal") * _turnSpeed;
        // create a Quaternion from amount and direction (x,y,z)
        Quaternion turnOffset = Quaternion.Euler(0, turnAmountThisFrame, 0);
        // apply quaternion to the rigidbody
        _rb.MoveRotation(_rb.rotation * turnOffset);
        */
    }
}
