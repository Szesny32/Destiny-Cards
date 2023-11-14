using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagePlayerController : MonoBehaviour
{
    [SerializeField]
    private Vector3 _cameraOffset;

    [SerializeField]
    private float _movementSpeed;

    [SerializeField]
    private float _rotationSpeed;

    private Camera _camera;
    private Rigidbody _rigidbody;
    private Animator _animator;


    private void Awake()
    {
        _camera = LevelManager.Instance.MainCamera;
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        SetRotationBasedOnInput(horizontalInput, verticalInput);

        var movement = new Vector3(horizontalInput, 0.0f, verticalInput).normalized;
        _rigidbody.velocity = new Vector3(
            movement.x * _movementSpeed,
            _rigidbody.velocity.y,
            movement.z * _movementSpeed);

        _camera.transform.position = transform.position + _cameraOffset;

        _animator.SetBool("running", horizontalInput != 0 || verticalInput != 0);
    }

    private void SetRotationBasedOnInput(float horizontalInput, float verticalInput)
    {
        float angleY = transform.eulerAngles.y;
        if (horizontalInput == 0 && verticalInput > 0)
        {
            angleY = 0.0f;
        }
        else if (horizontalInput == 0 && verticalInput < 0)
        {
            angleY = 180.0f;
        }
        else if (horizontalInput > 0 && verticalInput == 0)
        {
            angleY = 90.0f;
        }
        else if (horizontalInput < 0 && verticalInput == 0)
        {
            angleY = -90.0f;
        }
        else if (horizontalInput < 0 && verticalInput == 0)
        {
            angleY = -90.0f;
        }
        else if (horizontalInput > 0 && verticalInput > 0)
        {
            angleY = 45.0f;
        }
        else if (horizontalInput < 0 && verticalInput > 0)
        {
            angleY = -45.0f;
        }
        else if (horizontalInput < 0 && verticalInput < 0)
        {
            angleY = -135.0f;
        }
        else if (horizontalInput > 0 && verticalInput < 0)
        {
            angleY = 135.0f;
        }

        transform.eulerAngles =  new Vector3(0.0f, Mathf.LerpAngle(transform.eulerAngles.y, angleY, _rotationSpeed), 0.0f);
    }
}
