using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] CapsuleCollider _weaponCollider;
    [SerializeField] TrailRenderer _weaponTrail;
    [SerializeField] float _attackDelay;

    [Header("Sounds Name")]
    [SerializeField] AudioClip[] _walkSound;
    [SerializeField] AudioClip[] _attackSound;
    [SerializeField] AudioClip[] _hitSound;
    [SerializeField] AudioClip[] _dieSound;
    [SerializeField] AudioClip[] _skillSounds;

    private StateManager _stateManager;
    private Camera _camera;
    private Animator _animator;
    private Rigidbody _rigidBody;
    private NavMeshAgent _navMeshAgent;
    private SkinnedMeshRenderer _meshRenderer;
    private Color _onHitColor;

    private State _myState;

    KeyCode _attack, _move, _action, _pause;
    KeyCode[] _skill;

    float _maxSpeed;
    bool _isRigidImmuntity;

    private void Awake()
    {
        _stateManager = GetComponent<StateManager>();
        _animator = GetComponent<Animator>();
        _rigidBody = GetComponent<Rigidbody>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        transform.position = GameObject.FindGameObjectWithTag("Respawn").transform.position;
    }

    void Start()
    {
        _myState = State.IDLE;
        _stateManager.SetState(_myState);

        _camera = Camera.main;
        _navMeshAgent.updateRotation = false;
        _maxSpeed = PlayerManager.instance._playerSpeed;
        _onHitColor = _meshRenderer.material.color;

        PlayerKeySetting _keySet = PlayerKeySetting.instance;
        _attack = _keySet._attack;
        _move = _keySet._move;
        _action = _keySet._action;
        _pause = _keySet._esc;
        _skill = _keySet._skill;
    }

    void Update()
    {
        _stateManager.SetState(_myState);
        if (_stateManager.IsCanMove())
        {
            _animator.SetFloat("Speed", _navMeshAgent.velocity.magnitude / _maxSpeed);
            if (!_stateManager.IsAttacking())
            {
                if (Input.GetKeyDown(_attack))
                {
                    LookMousePosition();
                    StopAllCoroutines();
                    _myState = State.ATTACK;
                    StartCoroutine(OnAttack(_attackDelay, "OnSwordAttack"));
                }
                else if (Input.GetKey(_move))
                {
                    RaycastHit hit;
                    if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out hit))
                    {
                        _myState = State.MOVE;
                        SetDestination(hit.point);
                    }
                }

                if (Input.GetKeyDown(_skill[0]))
                {
                    if (SkillManager.instance.IsReady("Q"))
                    {
                        if (SkillManager.instance.IsEnemyInRange("Q"))
                        {
                            LookMousePosition();
                            StartCoroutine(OnAttack(SkillManager.instance.GetSkill("Q")._skillDelay, SkillManager.instance.GetSkill("Q")._skillTrigger));
                            SkillManager.instance.Use("Q");
                        }
                    }
                }
            }
        }
    }

    private void FixedUpdate()
    {
        LookMoveDirection();
    }

    void SetDestination(Vector3 _dest)
    {
        _navMeshAgent.isStopped = false;
        _navMeshAgent.SetDestination(_dest);
    }

    void LookMoveDirection()
    {
        if (_stateManager.IsMoving())
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
    }

    private void LookMousePosition()
    {
        RaycastHit hit;
        Ray _inputRay = _camera.ScreenPointToRay(Input.mousePosition);
        float _rayLength = 100f;
        Quaternion _attackRotation;
        if (Physics.Raycast(_inputRay, out hit, _rayLength))
        {
            Vector3 _playerToMouse = hit.point - transform.position;
            _playerToMouse.y = 0f;
            _attackRotation = Quaternion.LookRotation(_playerToMouse);
            transform.rotation = _attackRotation;
        }
    }

    IEnumerator OnAttack(float startDelay, float endDelay, string trigger)
    {
        SettingAttackInstance(1);
        _navMeshAgent.SetDestination(transform.position);
        _animator.SetTrigger(trigger);
        //endDelay = _animator.GetCurrentAnimatorClipInfo(0).Length;   // 현재 애니메이션의 길이
        endDelay = _animator.GetCurrentAnimatorStateInfo(0).length / 2;
        Debug.Log(_animator.GetCurrentAnimatorStateInfo(0).length.ToString());
        yield return new WaitForSeconds(endDelay);
        SettingAttackInstance(0);
    }

    IEnumerator OnAttack(float attackDelay, string trigger)
    {
        _navMeshAgent.SetDestination(transform.position);
        _animator.SetTrigger(trigger);
        //_animator.GetCurrentAnimatorClipInfo(0).Length;   현재 애니메이션의 길이

        yield return new WaitForSeconds(attackDelay);

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

    public void Damaged(int _damage)
    {
        SettingAttackInstance(0);
        _myState = State.IDLE;
        _navMeshAgent.isStopped = true;
        if (!_isRigidImmuntity)
        {
            _animator.SetTrigger("OnHit");
        }
        UIController.instance.SetHPOrb(-_damage);
    }
}
