using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
    private float _targetAngleY;

    private bool _movementAllowed = true;

    private void Awake()
    {
        _camera = LevelManager.Instance.MainCamera;
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    private IEnumerator DisabledMovement()
    {
        yield return new WaitForSeconds(0.75f);
        _movementAllowed = true;
    }

    public void Spellcast(Transform targetTransform)
    {
        _animator.SetBool("running", false);
        _movementAllowed = false;
        _animator.SetTrigger("spellcast");
        _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, 0);
        _targetAngleY = Mathf.Rad2Deg * Mathf.Atan2(targetTransform.position.x - transform.position.x, targetTransform.position.z - transform.position.z);
        StartCoroutine(DisabledMovement());
    }

    private void FixedUpdate()
    {
        transform.eulerAngles = new Vector3(
            0.0f, Mathf.LerpAngle(transform.eulerAngles.y, _targetAngleY, _rotationSpeed), 0.0f);

        if (!_movementAllowed)
            return;

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
        if (horizontalInput == 0 && verticalInput > 0)
        {
            _targetAngleY = 0.0f;
        }
        else if (horizontalInput == 0 && verticalInput < 0)
        {
            _targetAngleY = 180.0f;
        }
        else if (horizontalInput > 0 && verticalInput == 0)
        {
            _targetAngleY = 90.0f;
        }
        else if (horizontalInput < 0 && verticalInput == 0)
        {
            _targetAngleY = -90.0f;
        }
        else if (horizontalInput < 0 && verticalInput == 0)
        {
            _targetAngleY = -90.0f;
        }
        else if (horizontalInput > 0 && verticalInput > 0)
        {
            _targetAngleY = 45.0f;
        }
        else if (horizontalInput < 0 && verticalInput > 0)
        {
            _targetAngleY = -45.0f;
        }
        else if (horizontalInput < 0 && verticalInput < 0)
        {
            _targetAngleY = -135.0f;
        }
        else if (horizontalInput > 0 && verticalInput < 0)
        {
            _targetAngleY = 135.0f;
        }

    }
}
