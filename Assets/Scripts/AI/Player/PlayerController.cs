using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [SerializeField] CapsuleCollider _weaponCollider;
    [SerializeField] TrailRenderer _weaponTrail;
    [SerializeField] float _attackDelay;

    [Header("Sounds Name")]
    [SerializeField] AudioClip[] _walkSound;
    [SerializeField] AudioClip[] _attackSound;
    [SerializeField] AudioClip[] _hitSound;
    [SerializeField] AudioClip[] _dieSound;
    [SerializeField] AudioClip[] _skillSounds;

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

    private StateManager _stateManager;
    private Camera _camera;
    private Animator _animator;
    private Rigidbody _rigidBody;
    [HideInInspector] public NavMeshAgent _navMeshAgent;
    private SkinnedMeshRenderer _meshRenderer;
    private Color _onHitColor;

    private State _myState;

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
        //_navMeshAgent.updateRotation = false;
        _maxSpeed = _playerManager._playerSpeed;
        _onHitColor = _meshRenderer.material.color;

        PlayerKeySetting _keySet = PlayerKeySetting;
        _attack = _keySet._attack;
        _move = _keySet._move;
        _action = _keySet._action;
        _pause = _keySet._esc;
        _skill = _keySet._skill;
    }

    void Update()
    {
        _stateManager.SetState(_myState);
        if(_navMeshAgent.enabled)
            _animator.SetFloat("Speed", _navMeshAgent.velocity.magnitude / _maxSpeed);
        if (_stateManager.IsCanMove())
        {
            if (!_stateManager.IsAttacking())
            {
                if (Input.GetKey(_attack))
                {
                    StopAllCoroutines();
                    StartCoroutine(OnAttack(_attackDelay, "OnSwordAttack"));
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
                else if(Input.anyKeyDown)
                {
                    string keyName = Input.inputString.ToUpper();
                    KeyAction keyAction = PlayerKeySetting.GetKeyAction(keyName);
                    if(keyAction == KeyAction.SKILL)
                    {
                        if(_skillManager.IsReady(keyName))
                        {
                            LookMousePosition();
                            StartCoroutine(OnAttack(_skillManager.GetSkill(keyName)._skillDelay, _skillManager.GetSkill(keyName)._skillTrigger));
                            _skillManager.Use(keyName);
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

    public void ToTheStartPoint()
    {
        _myState = State.CANTMOVE;

        _navMeshAgent.enabled = false;
        transform.position = GameObject.FindGameObjectWithTag("Respawn").transform.position;
        _navMeshAgent.enabled = true;

        _myState = State.IDLE;
    }

    public void SetDestination(Transform tf)
    {
        _navMeshAgent.isStopped = false;
        _navMeshAgent.SetDestination(tf.position);
        var _moveDir = new Vector3(_navMeshAgent.steeringTarget.x, transform.position.y, _navMeshAgent.steeringTarget.z) - transform.position;
        if(_moveDir == Vector3.zero)
            _moveDir = new Vector3(tf.position.x, transform.position.y, tf.position.z) - transform.position;
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

    IEnumerator OnCasting(float attackDelay, string trigger)
    {
        _navMeshAgent.SetDestination(transform.position);
        _animator.SetBool(trigger, true);
        //_animator.GetCurrentAnimatorClipInfo(0).Length;   현재 애니메이션의 길이

        yield return new WaitForSeconds(attackDelay);

        _animator.SetBool(trigger, false);
        _myState = State.IDLE;
        SettingAttackInstance(0);
    }

    IEnumerator OnAttack(float attackDelay, string trigger)
    {
        _myState = State.ATTACK;
        _navMeshAgent.SetDestination(transform.position);
        LookMousePosition();
        _animator.SetTrigger(trigger);
        _animator.SetInteger("AttackIndex", Random.Range(0, 2));
        //_animator.GetCurrentAnimatorClipInfo(0).Length;   현재 애니메이션의 길이

        yield return new WaitForSecondsRealtime(attackDelay);

        _myState = State.IDLE;
        SettingAttackInstance(0);
    }

    private void WalkSoundPlay()
    {
        int num = Random.Range(0, _walkSound.Length);
        SoundManager.instance.SFXPlay(_walkSound[num]);
    }

    private void SwingSoundPlay()
    {
        int num = Random.Range(0, _attackSound.Length);
        SoundManager.instance.SFXPlay(_attackSound[num]);
    }

    private void SettingAttackInstance(int _value)
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
        SettingAttackInstance(0);
        _myState = State.IDLE;
        _navMeshAgent.isStopped = true;
        if (!isImmuned)
        {
            _animator.SetTrigger("OnHit");
        }
        if (percentageDamage)
            damage = (int)(_playerManager._playerHP * (float)damage / 100);
        UIController.instance.SetHPOrb(-damage);
    }

    public void SetMyState(State state) => _myState = state;

    public void RunTimeline()
    {
        Camera.main.TryGetComponent<CameraController>(out var cameraController);
        cameraController._isAble = false;
        _myState = State.CANTMOVE;
    }

    public void EndTimeline()
    {
        Camera.main.TryGetComponent<CameraController>(out var cameraController);
        cameraController._isAble = true;
        _myState = State.CANMOVE;
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
