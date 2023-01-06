using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

[RequireComponent(typeof(PlayerManager))]
[RequireComponent(typeof(QuestManager))]
[RequireComponent(typeof(PlayerKeySetting))]
[RequireComponent(typeof(SkillManager))]
[RequireComponent(typeof(StateManager))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;    

    [SerializeField] CapsuleCollider _weaponCollider;
    [SerializeField] TrailRenderer _weaponTrail;

    PlayerManager _playerManager;
    public PlayerManager PlayerManager
    {
        get
        {
            if (_playerManager == null)
                TryGetComponent<PlayerManager>(out _playerManager);
            return _playerManager;
        }
    }

    QuestManager _questManager;
    public QuestManager QuestManager
    {
        get
        {
            if (_questManager == null)
                TryGetComponent<QuestManager>(out _questManager);
            return _questManager;
        }
    }

    SkillManager _skillManager;
    public SkillManager SkillManager
    {
        get
        {
            if (_skillManager == null)
                TryGetComponent<SkillManager>(out _skillManager);
            return _skillManager;
        }
    }

    PlayerKeySetting _playerKeySetting;
    public PlayerKeySetting PlayerKeySetting
    {
        get
        {
            if (_playerKeySetting == null)
                TryGetComponent<PlayerKeySetting>(out _playerKeySetting);
            return _playerKeySetting;
        }
    }

    Animator _animator;
    public Animator Animator
    {
        get
        {
            if (_animator == null)
                TryGetComponent(out _animator);
            return _animator;
        }
    }

    [HideInInspector] public NavMeshAgent _navMeshAgent;
    [HideInInspector] public UnityEvent _attackEvent;

    StateManager _stateManager;
    Camera _camera;
    Rigidbody _rigidBody;
    SkinnedMeshRenderer _meshRenderer;
    AudioClip _dieSound;
    Color _originColor, _onHitColor;

    State _myState;

    KeyCode _attack, _move, _action, _pause;
    KeyCode[] _skill;

    float _maxSpeed;
    bool _isRigidImmuntity;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (instance != this)
            Destroy(this.gameObject);

        TryGetComponent<StateManager>(out _stateManager);
        TryGetComponent<Animator>(out _animator);
        TryGetComponent<Rigidbody>(out _rigidBody);
        TryGetComponent<NavMeshAgent>(out _navMeshAgent);
        TryGetComponent<PlayerManager>(out _playerManager);
        TryGetComponent<SkillManager>(out _skillManager);
        TryGetComponent<PlayerKeySetting>(out _playerKeySetting);

        _meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

        _camera = Camera.main;
    }

    private void OnEnable()
    {
        ToTheStartPoint();
    }

    void Start()
    {
        if (_attackEvent == null)
            _attackEvent = new UnityEvent();

        _maxSpeed = _playerManager._playerMaxSpeed;
        _originColor = _meshRenderer.material.color;
        _onHitColor = Color.red;
        _animator.SetFloat("AttackSpeed", _playerManager._attackSpeed);
        _dieSound = Resources.Load("Sounds/SFX/Player/SFX_Player_Die") as AudioClip;

        PlayerKeySetting _keySet = PlayerKeySetting;
        _attack = _keySet._attack;
        _move = _keySet._move;
        _action = _keySet._action;
        _pause = _keySet._esc;
        _skill = _keySet._skill;

        ToTheStartPoint();
    }

    void Update()
    {
        _stateManager.SetState(_myState);
        if (_myState != State.DEAD)
        {
            if (_navMeshAgent.enabled)
                _animator.SetFloat("Speed", _navMeshAgent.velocity.magnitude / _maxSpeed);
            if (_stateManager.IsCanMove())
            {
                if (!_stateManager.IsAttacking())
                {
                    if (EventSystem.current.currentSelectedGameObject == null)
                    {
                        if (Input.GetKey(_attack))
                        {
                            StopAllCoroutines();
                            LookMousePosition();
                            StartCoroutine(OnAttack(_playerManager._attackSpeed, "OnSwordAttack"));
                            _attackEvent.Invoke();
                        }
                        else if (Input.GetKey(_move))
                        {
                            int layer = 1 << LayerMask.NameToLayer("Ground");
                            RaycastHit hit;
                            if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out hit, 100f, layer))
                            {
                                if ((hit.point - transform.position).magnitude > 0.5f)
                                {
                                    _myState = State.MOVE;
                                    SetDestination(hit.point);
                                }
                            }
                        }
                        else if (Input.anyKeyDown)
                        {
                            string keyName = Input.inputString;
                            if (keyName.Length < 1)
                                return;
                            if (keyName[0] >= '0' && keyName[0] <= '9')
                                keyName = "Alpha" + keyName;
                            else
                                keyName = keyName.ToUpper();
                            KeyAction keyAction = PlayerKeySetting.GetKeyAction(keyName);

                            if (keyAction == KeyAction.SKILL)
                            {
                                if (_skillManager.GetSkillActivition(keyName) == SkillActivition.PASSIVE)
                                    return;

                                if (_skillManager.IsReady(keyName))
                                {
                                    LookMousePosition();
                                    Skill skill = _skillManager.GetSkill(keyName);
                                    SkillType type = _skillManager.GetSkillType(keyName);
                                    switch (type)
                                    {
                                        case SkillType.INSTANT:
                                            StartCoroutine(OnAttack(skill._skillDelay, skill._skillTrigger));
                                            break;
                                        case SkillType.CASTING:
                                            OnCasting(skill._skillTrigger);
                                            break;
                                        case SkillType.KEYDOWN:
                                            StartCoroutine(OnKeyDown(keyName, skill._skillDuration, skill._skillTrigger));
                                            break;
                                        case SkillType.BUFF:
                                            StartCoroutine(OnAction(skill._skillDelay, skill._skillTrigger));
                                            break;
                                    }
                                    _skillManager.Use(keyName);
                                }
                            }
                            else if(keyAction == KeyAction.HEALTHPOTION || keyAction == KeyAction.MANAPOTION)
                                UIController.instance.UsePotion(keyAction);
                        }
                    }
                }
            }
        }
    }

    private void FixedUpdate()
    {
        //if (_stateManager.IsMoving())
        //    LookMoveDirection();
    }

    public void Resurrection(bool anim = true, float time = 2f)
    {
        if(anim)
            _animator.SetTrigger("Resurrection");
        Invoke("ToTheStartPoint", time);
        PlayerManager.CurrentHP = PlayerManager.PlayerHP;
        PlayerManager.CurrentMP = PlayerManager.PlayerMP;
        UIController.instance.SetInit();
    }

    public void ToTheStartPoint()
    {
        _navMeshAgent.enabled = false;
        transform.position = GameObject.FindGameObjectWithTag("Respawn").transform.position;

        _navMeshAgent.enabled = true;
        _myState = State.IDLE;
        SetDestination(transform.position);
        gameObject.layer = LayerMask.NameToLayer("Player");

        BossAI boss = GameObject.FindObjectOfType<BossAI>();
        if (boss != null && boss.gameObject.activeSelf)
            boss.SetInit();
    }

    public void SetDestination(Transform tf)
    {
        _navMeshAgent.isStopped = false;
        _navMeshAgent.SetDestination(tf.position);
        var _moveDir = new Vector3(_navMeshAgent.steeringTarget.x, transform.position.y, _navMeshAgent.steeringTarget.z) - transform.position;
        if(_moveDir == Vector3.zero)
            _moveDir = new Vector3(tf.position.x, transform.position.y, tf.position.z) - transform.position;
        if(_moveDir != Vector3.zero)
            transform.forward = _moveDir;
    }

    public void SetDestination(Vector3 _dest)
    {
        _navMeshAgent.isStopped = false;
        _navMeshAgent.SetDestination(_dest);
    }

    void LookMoveDirection()
    {
        if (_navMeshAgent.velocity.magnitude <= 0.1f)
        {
            _myState = State.IDLE;
            _navMeshAgent.isStopped = true;
            return;
        }

        var _moveDir = new Vector3(_navMeshAgent.steeringTarget.x, transform.position.y, _navMeshAgent.steeringTarget.z) - transform.position;
        if (_moveDir != Vector3.zero)
            transform.forward = _moveDir;
    }

    private void LookMousePosition()
    {
        RaycastHit hit;
        Ray _inputRay = _camera.ScreenPointToRay(Input.mousePosition);
        float _rayLength = 100f;
        int layer = 1 << LayerMask.NameToLayer("Ground");
        Quaternion _attackRotation;
        if (Physics.Raycast(_inputRay, out hit, _rayLength, layer))
        {
            Vector3 _playerToMouse = hit.point - transform.position;
            _playerToMouse.y = 0f;
            _attackRotation = Quaternion.LookRotation(_playerToMouse);
            transform.rotation = _attackRotation;
        }
    }

    IEnumerator OnKeyDown(string key, float delay, string trigger)
    {
        _myState = State.ATTACK;
        _navMeshAgent.SetDestination(transform.position);
        _animator.SetTrigger(trigger);

        float count = 0f;
        while(count < delay)
        {
            count += Time.deltaTime;

            if (Input.GetKeyUp(key.ToLower()))
                break;

            yield return null;
        }

        _skillManager.Destroy(key);
        _animator.SetTrigger(trigger);
        _myState = State.IDLE;
        SettingAttackInstance(0);
    }

    IEnumerator OnAttack(float attackSpeed, string trigger)
    {
        _myState = State.ATTACK;
        _navMeshAgent.SetDestination(transform.position);
        _animator.SetTrigger(trigger);
        _animator.SetInteger("AttackIndex", Random.Range(0, 2));

        yield return new WaitForSeconds(1/attackSpeed);

        _myState = State.IDLE;
        SettingAttackInstance(0);
    }

    void OnCasting(string trigger)
    {
        _myState = State.ATTACK;
        _navMeshAgent.SetDestination(transform.position);
        _animator.SetTrigger(trigger);
    }

    IEnumerator OnAction(float delay, string trigger)
    {
        _myState = State.CANTMOVE;
        _navMeshAgent.SetDestination(transform.position);
        _animator.SetTrigger(trigger);

        yield return new WaitForSeconds(delay);

        _myState = State.IDLE;
    }

    private void WalkSoundPlay()
    {
        int num = Random.Range(0, _playerManager._walkSound.Length);
        SoundManager.instance.SFXPlay(_playerManager._walkSound[num]);
    }

    private void SwingSoundPlay()
    {
        int num = Random.Range(0, _playerManager._attackSound.Length);
        SoundManager.instance.SFXPlay(_playerManager._attackSound[num]);
    }

    public void SettingAttackInstance(int _value)
    {
        bool _toggle;
        if (_value > 0)
            _toggle = true;
        else
            _toggle = false;
        _navMeshAgent.isStopped = _toggle;
        _weaponCollider.enabled = _toggle;
        _weaponTrail.enabled = _toggle;
    }

    public void Damaged(int damage, bool isImmuned = true, bool percentageDamage = false)
    {
        if (gameObject.layer != LayerMask.NameToLayer("DeathPlayer"))
        {
            SettingAttackInstance(0);
            _myState = State.IDLE;
            _navMeshAgent.isStopped = true;
            if (percentageDamage)
                damage = (int)(_playerManager.PlayerHP * (float)damage / 100);
            _playerManager.CurrentHP -= damage;
            if (_playerManager.CurrentHP <= 0)
            {
                Die();
                return;
            }
            _meshRenderer.material.color = _onHitColor;
            Invoke("HitColor", 0.5f);
            SoundManager.instance.SFXPlay(PlayerManager._hitSound);

            if (!isImmuned)
            {
                _animator.SetTrigger("OnHit");
            }
        }
    }

    void HitColor() => _meshRenderer.material.color = _originColor;

    void Die()
    {
        StopAllCoroutines();
        _myState = State.DEAD;
        _animator.SetTrigger("Dead");
        _navMeshAgent.enabled = true;
        SoundManager.instance.SFXPlay(_dieSound);
        SettingAttackInstance(0);
        _navMeshAgent.transform.position = transform.position;
        _navMeshAgent.isStopped = true;
        gameObject.layer = LayerMask.NameToLayer("DeathPlayer");
        UIController.instance._playerDeathUI.SetActive(true);
        UIController.instance._isPlayerDeath = true;
    }

    public void SetMyState(State state) => _myState = state;

    public void RunTimeline(float speed)
    {
        Camera.main.TryGetComponent<CameraController>(out var cameraController);
        cameraController._isAble = false;
        _navMeshAgent.speed = speed;
        UIController.instance.PlayUI.SetActive(false);
        UIController.instance.CinemachineUI.SetActive(true);
    }

    public void EndTimeline()
    {
        Camera.main.TryGetComponent<CameraController>(out var cameraController);
        cameraController._isAble = true;
        _navMeshAgent.speed = PlayerManager._playerSpeed;
        UIController.instance.PlayUI.SetActive(true);
        UIController.instance.CinemachineUI.SetActive(false);
    }
}
