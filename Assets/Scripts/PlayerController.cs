using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float baseSpeed = 1000f;
    public float turningSpeed = 100f;
    public float jumpForce = 3000f;
    public float backwardMultiplier = .8f;
    public float walkingMultiplier = .5f;

    private bool _flag_Walking = false;

    private bool _forward = false;
    private bool _backward = false;
    private bool _sideLeft = false;
    private bool _sideRight = false;
    private bool _turnLeft = false;
    private bool _turnRight = false;
    private bool _jumping = false;

    private bool _moving = false;
    private bool _turning = false;

    private Animator _animator;
    private CharacterController _characterController;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();

        _animator.SetBool("_flag_Walking", _flag_Walking);
    }

	void Update () {
        HandleLocomotion();
        HandleFlags();
    }

    private void HandleLocomotion()
    {
        _forward = false;
        _backward = false;
        _sideLeft = false;
        _sideRight = false;
        _turnLeft = false;
        _turnRight = false;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            _forward = true;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            _backward = true;
        }
        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.LeftArrow))
        {
            _sideLeft = true;
        }
        if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.RightArrow))
        {
            _sideRight = true;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            _turnLeft = true;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            _turnRight = true;
        }
        if (Input.GetKeyDown(KeyCode.Space) && _characterController.isGrounded)
        {
            Jump();
        }
        

        if (_forward && !_backward)
        {
            MoveForward();
        }
        else if (!_forward && _backward)
        {
            MoveBackward();
        }

        if (_sideLeft && !_sideRight)
        {
            MoveLeftSide();
        }
        else if (!_sideLeft && _sideRight)
        {
            MoveRightSide();
        }

        if (_turnLeft && !_turnRight)
        {
            TurnLeft();
        }
        else if (!_turnLeft && _turnRight)
        {
            TurnRight();
        }

        if (!_moving)
        {
            _animator.SetBool("movingForward", false);
            _animator.SetBool("movingBackward", false);
            _animator.SetBool("movingLeftSide", false);
            _animator.SetBool("movingRightSide", false);
        } else {
            if (!_forward)
            {
                _animator.SetBool("movingForward", false);
            }
            if (!_backward)
            {
                _animator.SetBool("movingBackward", false);
            }
            if (!_sideLeft)
            {
                _animator.SetBool("movingLeftSide", false);
            }
            if (!_sideRight)
            {
                _animator.SetBool("movingRightSide", false);
            }
        }

        if (_moving || !_turning)
        {
            _animator.SetBool("turningLeft", false);
            _animator.SetBool("turningRight", false);
        }
        if (_jumping)
        {
            if (_characterController.isGrounded)
            {
                _jumping = false;
            }
            _animator.SetBool("jumping", !_characterController.isGrounded);
        }
        else if (!_characterController.isGrounded)
        {
            _animator.SetBool("falling", true);
        }
        else
        {
            _animator.SetBool("falling", false);
        }

        _characterController.Move(Vector3.up * Physics.gravity.y * Time.deltaTime);
    }
    

    private void MoveForward()
    {
        _characterController.Move(transform.TransformDirection(Vector3.forward) * GetMovingSpeed() * Time.deltaTime);
        _animator.SetBool("movingForward", true);
        _animator.SetBool("movingBackward", false);
        _moving = true;
    }

    private void MoveBackward()
    {
        _characterController.Move(transform.TransformDirection(Vector3.back) * GetMovingSpeed(true) * Time.deltaTime);
        _animator.SetBool("movingBackward", true);
        _animator.SetBool("movingForward", false);
        _moving = true;
    }

    private void MoveLeftSide()
    {
        _characterController.Move(transform.TransformDirection(Vector3.left) * GetMovingSpeed() * Time.deltaTime);
        _animator.SetBool("movingLeftSide", true);
        _animator.SetBool("movingRightSide", false);
        _moving = true;
    }

    private void MoveRightSide()
    {
        _characterController.Move(transform.TransformDirection(Vector3.right) * GetMovingSpeed() * Time.deltaTime);
        _animator.SetBool("movingRightSide", true);
        _animator.SetBool("movingLeftSide", false);
        _moving = true;
    }

    private void TurnLeft()
    {
        transform.Rotate(Vector3.down * turningSpeed * Time.deltaTime);
        _animator.SetBool("turningLeft", true);
        _animator.SetBool("turningRight", false);
        _turning = true;
    }

    private void TurnRight()
    {
        transform.Rotate(Vector3.up * turningSpeed * Time.deltaTime);
        _animator.SetBool("turningRight", true);
        _animator.SetBool("turningLeft", false);
        _turning = true;
    }

    private void Jump()
    {
        _characterController.Move(Vector3.up * jumpForce * Time.deltaTime);
        _jumping = true;
    }


    private void HandleFlags()
    {
        if (Input.GetKeyDown(KeyCode.KeypadDivide))
        {
            _flag_Walking = !_flag_Walking;
            _animator.SetBool("_flag_Walking", _flag_Walking);
        }
    }

    private float GetMovingSpeed(bool backward = false)
    {
        float speed = baseSpeed;
        if (_flag_Walking)
        {
            speed *= walkingMultiplier;
        } else if (backward)
        {
            speed *= backwardMultiplier;
        }
        return speed;
    }
}
