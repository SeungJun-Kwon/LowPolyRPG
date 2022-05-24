using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MonsterAI : MonoBehaviour
{
    [SerializeField] NormalMonster _monster;
    [SerializeField] ParticleSystem _hitEffect;
    [SerializeField] GameObject _damageText;

    [Header("Hp Bar")]
    public GameObject _hpBarPrefab;
    public Vector3 _hpBarOffset = new Vector3(0, 2.2f, 0);

    private Canvas _uiCanvas;
    private Image _hpBarImage;
    private GameObject _hpBar;

    protected Transform _target;
    protected Animator _animator;
    protected SkinnedMeshRenderer _meshRenderer;
    protected NavMeshAgent _navMesh;
    protected Rigidbody _rigidBody;
    protected BoxCollider _boxColl;
    protected MonsterSpawner _spawner;
    protected Vector3 _distanceFromTarget;
    protected Vector3 _originPos;
    protected Color _originColor;
    protected float _moveSpeed;
    protected bool _isDead, _isAttack;

    protected string _monsterName;
    protected int _monsterLevel;
    protected int _monsterDamage;
    protected int _monsterHP;
    protected float _monsterMoveSpeed;
    protected float _monsterAttackDelay;
    protected float _monsterRange;
    protected float _cognizance;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        _navMesh = GetComponent<NavMeshAgent>();
        _rigidBody = GetComponent<Rigidbody>();
        _boxColl = GetComponent<BoxCollider>();
        _spawner = GetComponentInParent<MonsterSpawner>();
        _uiCanvas = GameObject.Find("UI").GetComponent<Canvas>();
    }

    private void Start()
    {
        _originColor = _meshRenderer.material.color;
        _originPos = transform.position;
        _monsterName = _monster._monsterName;
        _monsterLevel = _monster._monsterLevel;
        _monsterMoveSpeed = _monster._monsterMoveSpeed;
        _monsterAttackDelay = _monster._monsterAttackDelay;
        _monsterDamage = _monster._monsterDamage;
        _monsterRange = _monster._monsterRange;
        _cognizance = _monster._cognizance;
        if (!_navMesh.enabled)
            _navMesh.enabled = true;
        _navMesh.speed = _monsterMoveSpeed;
        _navMesh.acceleration = _monsterMoveSpeed;
        _navMesh.stoppingDistance = _monsterRange;
        _navMesh.angularSpeed = 1000f;
    }

    private void OnEnable()
    {
        StatusInit();
    }

    private void StatusInit()
    {
        if (!_navMesh.enabled)
            _navMesh.enabled = true;
        SetHpBar();
        _monsterHP = _monster._monsterHP;
        _originPos = transform.position;
        SetTarget(null);
        gameObject.layer = LayerMask.NameToLayer("Monster");
        gameObject.tag = "Enemy";
        _isDead = false;
        _isAttack = false;
    }

    void SetHpBar()
    {
        _hpBar = Instantiate<GameObject>(_hpBarPrefab, _uiCanvas.transform);
        _hpBarImage = _hpBar.GetComponentsInChildren<Image>()[1];

        var _monsterHpBar = _hpBar.GetComponent<MonsterHpBar>();
        _monsterHpBar._targetTransform = gameObject.transform;
        _monsterHpBar._offset = _hpBarOffset;
    }

    protected virtual void Update()
    {
        if (!_isDead)
        {
            _animator.SetFloat("Speed", _navMesh.velocity.magnitude / _monsterMoveSpeed);
            if (_target == null)
            {
                _navMesh.SetDestination(_originPos);
            }
            else
            {
                _distanceFromTarget = transform.position - _target.position;
                if (_distanceFromTarget.magnitude > _monsterRange)
                {
                    _navMesh.SetDestination(_target.position);
                    _rigidBody.velocity = Vector3.zero;
                    _rigidBody.angularVelocity = Vector3.zero;
                }
                else if (_distanceFromTarget.magnitude <= _monsterRange && !_isAttack)
                {
                    StartCoroutine(Attack());
                }
            }
        }
    }

    public void SetTarget(Transform _targetTf)
    {
        _target = _targetTf;
    }

    IEnumerator Attack()
    {
        _isAttack = true;
        _navMesh.isStopped = true;
        transform.LookAt(_target);
        _rigidBody.velocity = Vector3.zero;
        _rigidBody.angularVelocity = Vector3.zero;
        _animator.SetTrigger("Attack");

        yield return new WaitForSeconds(_monsterAttackDelay);

        _isAttack = false;
        _navMesh.isStopped = false;
    }

    public void Damaged(int minDamage, int maxDamage, int numberOfAttack, Transform target)
    {
        if (target == null)
            target = GameObject.FindGameObjectWithTag("Player").transform;
        SetTarget(target);
        for (int i = 0; i < numberOfAttack; i++)
        {
            int damage = Random.Range(minDamage, maxDamage + 1);
            _monsterHP -= damage;
            _hpBarImage.fillAmount = (float)_monsterHP / (float)_monster._monsterHP;
            GameObject damageObject = Instantiate(_damageText, _uiCanvas.transform);
            var damageText = damageObject.GetComponent<DamageText>();
            damageText._damage = damage;
            damageText._targetTransform = transform;
            Object effect = Instantiate(_hitEffect, transform.position, Quaternion.identity);
        }
        _animator.StopPlayback();
        if (_monsterHP <= 0)
        {
            StopAllCoroutines();
            StartCoroutine(Die());
            gameObject.layer = LayerMask.NameToLayer("DeathMonster");
            gameObject.tag = "DeathEnemy";
            return;
        }
        _animator.SetTrigger("OnHit");
        StartCoroutine(OnHitColor());
    }

    IEnumerator OnHitColor() {
        _meshRenderer.material.color = Color.white;

        yield return new WaitForSeconds(0.1f);

        _meshRenderer.material.color = _originColor;
    }

    IEnumerator Die()
    {
        _animator.SetTrigger("Dead");
        _isDead = true;
        _navMesh.enabled = false;
        PlayerManager.instance.GainExp(_monster._monsterGiveExp);
        yield return new WaitForSeconds(3f);

        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        Destroy(_hpBar);
        if (!_spawner)
            _spawner = GetComponentInParent<MonsterSpawner>();
        _spawner.RemoveMonster(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Melee" && !_isDead)
        {
            PlayerController _playerController = other.GetComponentInParent<PlayerController>();
            _playerController.TryGetComponent<PlayerManager>(out PlayerManager _playerManager);
            Damaged(_playerManager._playerMinPower, _playerManager._playerMaxPower, 1, other.transform);
        }
    }
}
