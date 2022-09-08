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
    protected Transform _attackPosition;
    protected Animator _animator;
    protected SkinnedMeshRenderer _meshRenderer;
    protected NavMeshAgent _navMesh;
    protected Rigidbody _rigidBody;
    protected BoxCollider _boxColl;
    protected MonsterSpawner _spawner;
    protected Vector3 _originPos;
    protected Color _originColor;
    protected float _moveSpeed;
    protected bool _isDead, _isChase, _isAttack;

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

        _attackPosition = transform.Find("Center");
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
        _target = null;
        gameObject.layer = LayerMask.NameToLayer("Monster");
        gameObject.tag = "Enemy";
        _isDead = false;
        _isChase = false;
    }

    void SetHpBar()
    {
        _hpBar = Instantiate<GameObject>(_hpBarPrefab, _uiCanvas.transform);
        _hpBar.SetActive(true);
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
                return;
            }

            if (_isChase)
            {
                _navMesh.SetDestination(_target.position);
                _rigidBody.velocity = Vector3.zero;
                _rigidBody.angularVelocity = Vector3.zero;
                RaycastHit[] hits = Physics.SphereCastAll(transform.position, _monsterRange, transform.up, 0f, LayerMask.GetMask("Player"));
                if (hits.Length > 0)
                    _isChase = false;
            }
            else
            {
                _navMesh.isStopped = true;
                _rigidBody.velocity = Vector3.zero;
                _rigidBody.angularVelocity = Vector3.zero;
            }
        }
    }

    public void SetTarget(Transform _targetTf)
    {
        _target = _targetTf;
        _isChase = true;
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
            GameObject damageObject = Instantiate(_damageText);
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
    }

    IEnumerator Die()
    {
        _animator.SetTrigger("Dead");
        _isDead = true;
        _navMesh.enabled = false;

        PlayerManager playerManager = PlayerController.instance.PlayerManager;
        playerManager.GainExp(_monster._monsterGiveExp);
        string noticeMessage = _monsterName + "을(를) 처치하여 " + _monster._monsterGiveExp + "의 경험치를 획득하였습니다.";
        UIController.instance.NoticeArea.GetMessage(noticeMessage);

        List<HuntingQuest> playerQuest = new List<HuntingQuest>();
        foreach(Quest quest in playerManager.GetCurrentQuests())
            if(quest._type == Quest.Type.HUNTING)
                playerQuest.Add((HuntingQuest)quest);

        foreach(HuntingQuest quest in playerQuest)
        {
            if (quest._targetMonster == _monster)
                quest.HuntMonster();
        }

        yield return new WaitForSeconds(3f);

        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        Destroy(_hpBar);
        if (!_spawner)
            _spawner = GetComponentInParent<MonsterSpawner>();
        if(_spawner)
            _spawner.RemoveMonster(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Melee" && !_isDead)
        {
            PlayerManager _playerManager = PlayerController.instance.PlayerManager;
            Damaged(_playerManager._playerMinPower, _playerManager._playerMaxPower, 1, other.transform);
        }
    }
}
