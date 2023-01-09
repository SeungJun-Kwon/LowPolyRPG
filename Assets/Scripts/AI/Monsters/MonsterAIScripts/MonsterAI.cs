using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    [SerializeField] public NormalMonster _monster;
    [SerializeField] ParticleSystem _hitEffect;
    [SerializeField] GameObject _damageText;

    private GameObject _monsterHPGo;
    private MonsterHPBar _monsterHPBar;

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

        _attackPosition = transform.Find("Center");

        if(!_monsterHPGo)
        {
            _monsterHPGo = Instantiate(Resources.Load("Prefabs/UI/MonsterHPBar"), transform) as GameObject;
            _monsterHPGo.TryGetComponent(out _monsterHPBar);
        }

        GameObject objectNameGo = Instantiate(Resources.Load("Prefabs/UI/ObjectName"), transform) as GameObject;
        objectNameGo.TryGetComponent(out ObjectName objectName);
        objectName.SetText(_monster);
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
        _monsterHPBar.SetTransform(_boxColl.size);
    }

    private void OnEnable()
    {
        StatusInit();
    }

    protected virtual void StatusInit()
    {
        if (!_navMesh.enabled)
            _navMesh.enabled = true;
        _animator.ResetTrigger("Attack");
        _monsterHP = _monster._monsterHP;
        _monsterHPBar._fill.fillAmount = 1f;
        _originPos = transform.position;
        _target = null;
        gameObject.layer = LayerMask.NameToLayer("Monster");
        gameObject.tag = "Enemy";
        _isAttack = false;
        _isDead = false;
        _isChase = false;
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
                RaycastHit[] hits = Physics.SphereCastAll(transform.position, _monsterRange, transform.up, 0f, LayerMask.GetMask("Player"), QueryTriggerInteraction.Collide);
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

    public void Damaged(int minDamage, int maxDamage, int numberOfAttack, Transform target = null, AudioClip hitSound = null)
    {
        if (target == null)
            target = PlayerController.instance.transform;
        SetTarget(target);
        for (int i = 0; i < numberOfAttack; i++)
        {
            int damage = Random.Range(minDamage, maxDamage + 1);
            _monsterHP -= damage;
            _monsterHPBar._fill.fillAmount = (float)_monsterHP / (float)_monster._monsterHP;
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
        if (hitSound == null)
            hitSound = _monster._hitSound;
        SoundManager.instance.SFXPlay(hitSound);
        StartCoroutine(Damaged());
    }

    IEnumerator Damaged()
    {
        _navMesh.isStopped = true;

        yield return new WaitForSeconds(1f);

        _navMesh.isStopped = false;
    }

    IEnumerator Die()
    {
        _animator.SetTrigger("Dead");
        _isDead = true;
        _navMesh.enabled = false;
        SoundManager.instance.SFXPlay(_monster._deadSound);

        PlayerManager playerManager = PlayerController.instance.PlayerManager;
        playerManager.CurrentExp += _monster._monsterGiveExp;
        string noticeMessage = _monsterName + "을(를) 처치하여 " + _monster._monsterGiveExp + "의 경험치를 획득하였습니다.";
        UIController.instance.NoticeArea.GetMessage(noticeMessage);

        QuestManager questManager = PlayerController.instance.QuestManager;
        List<QuestData> questDataList = questManager.GetCurrentQuestData(Quest.Type.HUNTING);
        if(questDataList != null)
            foreach (HuntingQuestData quest in questDataList)
                quest.HuntMonster(_monster);

        yield return new WaitForSeconds(3f);

        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
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
