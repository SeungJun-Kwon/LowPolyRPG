using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rush : MonoBehaviour
{
    [SerializeField] Skill _skill;

    PlayerController _playerController;
    NavMeshAgent _playerNavMesh;

    private void Awake()
    {
        _playerController = PlayerController.instance;
    }

    private void OnEnable()
    {
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        Vector3 rushDir = _playerController.transform.forward;
        Vector3 destination = _playerController.transform.position + rushDir * 5;
        transform.forward = -rushDir;
        transform.position += rushDir * 5;
        _playerController._navMeshAgent.speed = 1000;
        _playerController._navMeshAgent.acceleration = 1000;
        _playerController.SetDestination(destination);

        yield return new WaitForSeconds(_skill._skillDelay);

        _playerController._navMeshAgent.speed = 3.5f;
        _playerController._navMeshAgent.acceleration = 8;
    }
}
