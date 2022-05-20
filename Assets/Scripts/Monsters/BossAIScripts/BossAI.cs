using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{
    SkinnedMeshRenderer _meshRenderer;
    MeshCollider _collider;
    NavMeshAgent _navMeshAgent;

    private void Awake()
    {
        _meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        _collider = GetComponent<MeshCollider>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        Mesh colliderMesh = new Mesh();
        _meshRenderer.BakeMesh(colliderMesh);
        _collider.sharedMesh = null;
        _collider.sharedMesh = colliderMesh;
        _collider.convex = true;
    }

    private void Update()
    {
        
    }

    
}
