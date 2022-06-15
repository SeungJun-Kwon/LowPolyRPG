using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    Camera _camera;

    Rigidbody _rigidbody;

    Vector3 _destination;

    bool _isMove;

    private void Awake()
    {
        _camera = Camera.main;
        TryGetComponent<Rigidbody>(out _rigidbody);
    }

    private void Update()
    {
        if(Input.GetMouseButton(1))
        {
            RaycastHit hit;
            if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if ((_rigidbody.position - hit.point).magnitude > 1f)
                {
                    _destination = hit.point;
                    _isMove = true;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if(_isMove)
        {
            Vector3 moveDir = _destination - transform.position;
            moveDir = moveDir.normalized * 5f;
            _rigidbody.MovePosition(_rigidbody.position + moveDir * Time.fixedDeltaTime);
            if ((_destination - _rigidbody.position).magnitude <= 0.1f)
            {
                _isMove = false;
            }
        }
    }
}
