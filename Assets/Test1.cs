using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Test1 : MonoBehaviour
{
    public float _range;

    Camera _camera;

    Rigidbody _rigidbody;
    NavMeshAgent _navMeshAgent;

    Transform _target;
    Vector3 _destination;

    private void Awake()
    {
        _camera = Camera.main;
        TryGetComponent<Rigidbody>(out _rigidbody);
        TryGetComponent<NavMeshAgent>(out _navMeshAgent);

        _target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
    {
        _navMeshAgent.stoppingDistance = _range;
    }

    private void Update()
    {
        if(_target)
        {
            _navMeshAgent.SetDestination(_target.position);
        }
    }

    private void FixedUpdate()
    {
        if(_destination != Vector3.zero)
        {
            //Vector3 moveDir = _destination - transform.position;
            //moveDir = moveDir.normalized;
            //transform.Translate(moveDir * Time.fixedDeltaTime * 3f);
            if ((_destination - transform.position).magnitude <= _range)
            {
                _navMeshAgent.isStopped = true;
                //_destination = Vector3.zero;
                //_rigidbody.velocity = Vector3.zero;
            }
            else
                _navMeshAgent.isStopped = false;
        }
    }
}
