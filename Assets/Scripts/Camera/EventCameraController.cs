using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCameraController : MonoBehaviour
{
    [SerializeField] Vector3 _pos;
    [SerializeField] Quaternion _rot;
    [SerializeField] float _moveSpeed = 1f, _rotationSpeed = 1f;

    private void OnEnable()
    {
        transform.position = Camera.main.transform.position;
        transform.rotation = Camera.main.transform.rotation;

        CameraController.instance._isAble = false;
        CameraController.instance._isEvent = true;
    }

    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, _pos, Time.deltaTime * _moveSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, _rot, Time.deltaTime * _moveSpeed);
    }

    public void SetMovePosition(Transform tf) => _pos = tf.position;

    public void SetMoveSpeed(float speed) => _moveSpeed = speed;

    public void SetRotate(Transform tf) => _rot = Quaternion.LookRotation((tf.position - _pos).normalized).normalized;

    public void SetRotateSpeed(float speed) => _rotationSpeed = speed;

    private void OnDisable()
    {
        CameraController.instance._isAble = true;
        CameraController.instance._isEvent = false;
    }
}
