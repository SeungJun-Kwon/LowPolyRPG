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

    private Camera _camera;
    private Animator _animator;
    private Rigidbody _rigidBody;
    private NavMeshAgent _navMeshAgent;
    private SkinnedMeshRenderer _meshRenderer;
    private Color _onHitColor;

    private float _maxSpeed;

    bool _isMove, _isAttacking, __isRigidImmuntity;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidBody = GetComponent<Rigidbody>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        transform.position = GameObject.FindGameObjectWithTag("Respawn").transform.position;
    }

    void Start()
    {
        _camera = Camera.main;
        _navMeshAgent.updateRotation = false;
        _maxSpeed = PlayerManager.instance._playerSpeed;
        _onHitColor = _meshRenderer.material.color;
    }

    void Update()
    {
        _animator.SetFloat("Speed", _navMeshAgent.velocity.magnitude / _maxSpeed);
        if (!_isAttacking && !__isRigidImmuntity)
        {
            if (Input.GetMouseButtonDown(0))
            {
                LookMousePosition();
                StopAllCoroutines();
                StartCoroutine(OnAttack(_attackDelay, "OnSwordAttack"));
            }
            else if (Input.GetMouseButton(1))
            {
                RaycastHit hit;
                if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    SetDestination(hit.point);
                }
            }

            if(Input.GetKeyDown(KeyCode.Q))
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

    private void FixedUpdate()
    {
        LookMoveDirection();
    }

    void SetDestination(Vector3 _dest)
    {
        _navMeshAgent.isStopped = false;
        _isMove = true;
        _navMeshAgent.SetDestination(_dest);
    }

    void LookMoveDirection()
    {
        if (_isMove)
        {
            if (_navMeshAgent.velocity.magnitude <= 0.1f)
            {
                _isMove = false;
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

        if (_isAttacking)
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
        _isMove = !_toggle;
        _navMeshAgent.isStopped = _toggle;
        _weaponCollider.enabled = _toggle;
        _isAttacking = _toggle;
        _weaponTrail.enabled = _toggle;
    }

    public void Damaged(int _damage)
    {
        _isAttacking = false;
        SettingAttackInstance(0);
        _navMeshAgent.isStopped = true;
        if (!__isRigidImmuntity)
        {
            _animator.SetTrigger("OnHit");
        }
        UIController.instance.SetHPOrb(-_damage);
    }
}
