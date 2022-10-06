using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform _targetTransform;
    [SerializeField] float _cameraSpeed;
    [SerializeField] Vector3 _offset;
    [SerializeField] Vector3 _minOffset, _maxOffset;
    [SerializeField] Vector3 _rotation;
    [SerializeField] float _scrollSpeed = 5f;

    [HideInInspector] public bool _isAble = true;

    private void Awake()
    {
        if (!_targetTransform)
            _targetTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (_isAble)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");

            if (scroll > 0 && _offset.magnitude > _minOffset.magnitude)
                _offset -= new Vector3(0, 0.2f, -0.1f) * _scrollSpeed;
            else if (scroll < 0 && _offset.magnitude < _maxOffset.magnitude)
                _offset += new Vector3(0, 0.2f, -0.1f) * _scrollSpeed;
        }
    }

    private void LateUpdate()
    {
        if (_isAble)
        {
            transform.position = Vector3.Lerp(transform.position, _targetTransform.position + _offset, Time.deltaTime * _cameraSpeed);
            transform.rotation = Quaternion.Euler(_rotation);
        }
    }
}
