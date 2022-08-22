using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{
    [SerializeField] ParticleSystem _hitEffect;
    [SerializeField] GameObject _damageText;

    [SerializeField] protected BossMonster _bossMonster;

    protected StateManager _stateManager;

    protected SkinnedMeshRenderer _meshRenderer;
    protected BoxCollider _boxCollider;
    protected Rigidbody _rigidBody;
    protected NavMeshAgent _navMesh;
    protected Animator _animator;

    protected Transform _target;
    protected Vector3 _distanceFromTarget;
    protected float _moveSpeed;

    protected string[] _skillName;
    protected Skill[] _bossSkill;
    protected GameObject[] _bossSkillPrefab;

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
        TryGetComponent<BoxCollider>(out _boxCollider);
        TryGetComponent<NavMeshAgent>(out _navMesh);
        TryGetComponent<Animator>(out _animator);

        _meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
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
        _bossSkill = _bossMonster._skill;
        _bossSkillPrefab = _bossMonster._skillPrefab;

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
        if (_state != State.DEAD)
        {
            if (_stateManager.IsCanMove())
            {
                _animator.SetFloat("Speed", _navMesh.velocity.magnitude / _bossMoveSpeed);
                if (_target)
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
    }

    protected IEnumerator PlayAttackAnim(string trigger)
    {
        _state = State.ATTACK;
        _navMesh.isStopped = true;
        transform.LookAt(_target);
        _animator.SetTrigger(trigger);
        _animator.SetFloat("Speed", 0);

        yield return new WaitForSeconds(_bossAttackDelay);

        _state = State.IDLE;
        _navMesh.isStopped = false;
    }

    public void Damaged(int minDamage, int maxDamage, int numberOfAttack)
    {
        for (int i = 0; i < numberOfAttack; i++)
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
        if (_bossHP <= 0)
        {
            StopAllCoroutines();
            StartCoroutine(Die());
            gameObject.layer = LayerMask.NameToLayer("DeathMonster");
            gameObject.tag = "DeathEnemy";
            return;
        }
        if (_stiffness <= 0)
        {
            StopAllCoroutines();
            _state = State.IDLE;
            _animator.SetTrigger("OnHit");
            _stiffness = _bossMonster._stiffness;
            _stiffnessCount--;
        }
        if (_stiffnessCount <= 0)
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

    IEnumerator Die()
    {
        _animator.SetTrigger("Dead");
        _state = State.DEAD;
        //_navMesh.enabled = false;

        PlayerManager playerManager = PlayerController.instance.PlayerManager;
        playerManager.GainExp(_bossMonster._monsterGiveExp);

        List<HuntingQuest> playerQuest = new List<HuntingQuest>();
        foreach (Quest quest in playerManager.GetCurrentQuests())
            if (quest._type == Quest.Type.HUNTING)
                playerQuest.Add((HuntingQuest)quest);

        foreach (HuntingQuest quest in playerQuest)
        {
            if (quest._targetMonster == _bossMonster)
                quest.HuntMonster();
        }

        yield return new WaitForSeconds(3f);

        gameObject.SetActive(false);
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
