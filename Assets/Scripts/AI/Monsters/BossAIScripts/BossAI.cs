using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{
    [SerializeField] ParticleSystem _hitEffect;
    [SerializeField] GameObject _damageText;

    [SerializeField] BossMonster _bossMonster;
    [SerializeField] Skill[] _bossSkill;
    [SerializeField] GameObject[] _bossSkillPrefab;

    protected StateManager _stateManager;

    protected SkinnedMeshRenderer _meshRenderer;
    protected MeshCollider _meshCollider;
    protected Rigidbody _rigidBody;
    protected NavMeshAgent _navMesh;
    protected Animator _animator;

    protected Transform _target;
    protected Vector3 _distanceFromTarget;
    protected float _moveSpeed;

    protected string _bossName;
    protected int _bossLevel;
    protected int _bossDamage;
    protected int _bossHP;
    protected int _stiffness;
    protected int _stiffnessCount;
    protected float _bossMoveSpeed;
    protected float _bossAttackDelay;
    protected float _bossRange;
    protected float _stiffnessDuration;
    protected bool _isPlayerInRange;

    protected State _state;

    protected virtual void Awake()
    {
        TryGetComponent<StateManager>(out _stateManager);
        TryGetComponent<Rigidbody>(out _rigidBody);
        TryGetComponent<NavMeshAgent>(out _navMesh);
        TryGetComponent<Animator>(out _animator);

        _meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        _meshCollider = GetComponentInChildren<MeshCollider>();
    }

    private void Start()
    {
        _bossName = _bossMonster._monsterName;
        _bossLevel = _bossMonster._monsterLevel;
        _bossDamage = _bossMonster._monsterDamage;
        _bossHP = _bossMonster._monsterHP;
        _stiffness = _bossMonster._stiffness;
        _stiffnessCount = _bossMonster._stiffnessCount;
        _stiffnessDuration = _bossMonster._stiffnessDuration;
        _bossMoveSpeed = _bossMonster._monsterMoveSpeed;
        _bossAttackDelay = _bossMonster._monsterAttackDelay;
        _bossRange = _bossMonster._monsterRange;
        _state = State.IDLE;
        _stateManager.SetInitState();
        _target = PlayerController.instance.transform;
        if (!_navMesh.enabled)
            _navMesh.enabled = true;
        _navMesh.speed = _bossMoveSpeed;
        _navMesh.acceleration = _bossMoveSpeed;
        _navMesh.stoppingDistance = _bossRange;
        _navMesh.angularSpeed = 1000f;
    }

    protected virtual void Update()
    {
        _stateManager.SetState(_state);
        if(_stateManager.IsCanMove())
        {
            _animator.SetFloat("Speed", _navMesh.velocity.magnitude / _bossMoveSpeed);
            if(_target)
            {
                _navMesh.SetDestination(_target.position);
                _rigidBody.velocity = Vector3.zero;
                _rigidBody.angularVelocity = Vector3.zero;
                RaycastHit[] hits = Physics.SphereCastAll(transform.position, _bossRange, transform.up, 0f, LayerMask.GetMask("Player"));
                if (hits.Length > 0)
                    _isPlayerInRange = true;
                else
                    _isPlayerInRange = false;
            }
        }
        else
        {
            _navMesh.isStopped = true;
        }
    }

    public void Damaged(int minDamage, int maxDamage, int numberOfAttack)
    {
        for(int i = 0; i < numberOfAttack; i++)
        {
            int damage = Random.Range(minDamage, maxDamage + 1);
            _bossHP -= damage;
            GameObject damageObject = Instantiate(_damageText);
            var damageText = damageObject.GetComponent<DamageText>();
            damageText._damage = damage;
            damageText._targetTransform = transform;
            Object effect = Instantiate(_hitEffect, transform.position, Quaternion.identity);
            if (_state != State.STUNNED)
                _stiffness -= damage;
        }
        if(_stiffness <= 0)
        {
            StopAllCoroutines();
            _state = State.IDLE;
            _animator.SetTrigger("OnHit");
            _stiffness = _bossMonster._stiffness;
            _stiffnessCount--;
        }
        if(_stiffnessCount <= 0)
        {
            StartCoroutine(Stun());
            _stiffnessCount = _bossMonster._stiffnessCount;
        }
    }

    protected IEnumerator Stun()
    {
        _state = State.STUNNED;
        _animator.SetBool("IsStun", true);

        yield return new WaitForSeconds(_stiffnessDuration);

        _state = State.IDLE;
        _animator.SetBool("IsStun", false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Melee" && _state != State.DEAD)
        {
            PlayerManager _playerManager = PlayerController.instance.PlayerManager;
            Damaged(_playerManager._playerMinPower, _playerManager._playerMaxPower, 1);
        }
    }
}
