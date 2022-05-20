using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{
    [SerializeField] BossMonster _bossMonster;
    [SerializeField] Skill[] _bossSkill;
    [SerializeField] GameObject[] _bossSkillPrefab;

    protected SkinnedMeshRenderer _meshRenderer;
    protected MeshCollider _meshCollider;
    protected Rigidbody _rigidBody;
    protected NavMeshAgent _navMesh;
    protected Animator _animator;
    protected Vector3 _distanceFromTarget;
    protected float _moveSpeed;
    protected bool _isDead, _isAttack;

    protected string _bossName;
    protected int _bossLevel;
    protected int _bossDamage;
    protected int _bossHP;
    protected float _bossMoveSpeed;
    protected float _bossAttackDelay;
    protected float _bossRange;

    private void Awake()
    {
        _meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        _meshCollider = GetComponent<MeshCollider>();
        _rigidBody = GetComponent<Rigidbody>();
        _navMesh = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        Mesh colliderMesh = new Mesh();
        _meshRenderer.BakeMesh(colliderMesh);
        _meshCollider.sharedMesh = null;
        _meshCollider.sharedMesh = colliderMesh;
        _meshCollider.convex = true;
        _bossName = _bossMonster._monsterName;
        _bossLevel = _bossMonster._monsterLevel;
        _bossMoveSpeed = _bossMonster._monsterMoveSpeed;
        _bossAttackDelay = _bossMonster._monsterAttackDelay;
        _bossDamage = _bossMonster._monsterDamage;
        _bossRange = _bossMonster._monsterRange;
        if (!_navMesh.enabled)
            _navMesh.enabled = true;
        _navMesh.speed = _bossMoveSpeed;
        _navMesh.acceleration = _bossMoveSpeed;
        _navMesh.stoppingDistance = _bossRange;
        _navMesh.angularSpeed = 1000f;
    }

    private void Update()
    {
        
    }

    
}
