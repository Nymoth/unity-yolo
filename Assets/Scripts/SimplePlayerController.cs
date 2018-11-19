using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlayerController : MonoBehaviour {

    public float baseSpeed = 5f;
    public float jumpSpeed = 5f;
    public float turnSpeed = 2f;
    public float backwardMultiplier = .6f;

    private Vector3 _moveDirection = Vector3.zero;

    private Animator _animator;
    private CharacterController _controller;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        bool leftMouse = Input.GetMouseButton(0);
        bool rightMouse = Input.GetMouseButton(1);
        bool middleMouse = Input.GetMouseButton(2);

        Cursor.visible = !(leftMouse || rightMouse || middleMouse);


        float turn = 0f;
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = 0f;

        if ((leftMouse && rightMouse) || middleMouse)
        {
            vertical = 1;
        }
        else
        {
            vertical = Input.GetAxisRaw("Vertical");
        }

        if (vertical < 0)
        {
            vertical *= backwardMultiplier;
        }

        if (rightMouse || middleMouse)
        {
            turn = Input.GetAxis("Mouse X");
        }
        else
        {
            turn = Input.GetAxisRaw("TurnHorizontal");
        }

        transform.Rotate(Vector3.up * turn * turnSpeed * 100 * Time.deltaTime);
        _animator.SetFloat("turn", turn);

        Vector3 localMov = Vector3.zero;
        if (_controller.isGrounded)
        {
            _moveDirection = new Vector3(horizontal, 0f, vertical);
            localMov = _moveDirection;
            _moveDirection = transform.TransformDirection(_moveDirection);
            _moveDirection = _moveDirection * baseSpeed;

            if (Input.GetButtonDown("Jump"))
            {
                _moveDirection.y = jumpSpeed;
            }
        }

        _moveDirection.y = _moveDirection.y + (Physics.gravity.y * Time.deltaTime);

        _controller.Move(_moveDirection * Time.deltaTime);
        _animator.SetFloat("movX", localMov.x);
        _animator.SetFloat("movY", _moveDirection.y);
        _animator.SetFloat("movZ", localMov.z);
    }


}
