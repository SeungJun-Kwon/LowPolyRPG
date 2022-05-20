using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform _targetTransform;
    [SerializeField] float _cameraSpeed;
    [SerializeField] Vector3 _offset;

    private void Awake()
    {
        if (!_targetTransform)
            _targetTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, _targetTransform.position + _offset, Time.deltaTime * _cameraSpeed);
    }
}
