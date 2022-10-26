using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ThrowingShield : MonoBehaviour
{
    [SerializeField] private PlayerSkill _skill;

    Transform _nextTarget = null;
    BoxCollider _boxCollider;
    Vector3 _moveDir;

    float _skillDamageMultiplier;
    float _skillRange;
    float _skillDuration;
    int _skillNumberOfAttack;
    int _skillNumberOfEnemy;
    bool _activeSelf;

    List<Transform> _targetArray;

    private void Awake()
    {
        TryGetComponent<BoxCollider>(out _boxCollider);
        if (!_boxCollider)
            _boxCollider = GetComponent<BoxCollider>();
    }

    private void OnEnable()
    {
        _moveDir = Vector3.zero;
        _skillDamageMultiplier = _skill._skillDamageMultiplier;
        _skillRange = _skill._skillRange;
        _skillDuration = _skill._skillDuration;
        _skillNumberOfAttack = _skill._skillNumberOfAttack;
        _skillNumberOfEnemy = 0;
        _targetArray = new List<Transform>();

        SetTarget();
        if (_nextTarget == null)
        {
            Destroy(gameObject);
            return;
        }

        _activeSelf = true;
    }

    private void FixedUpdate()
    {
        if(_activeSelf)
        {
            if (_nextTarget == null || _nextTarget.gameObject == null || !_nextTarget.gameObject.activeSelf)
                Destroy(gameObject);
            else if ((_nextTarget || _nextTarget.gameObject.activeSelf) && _boxCollider.enabled)
            {
                _moveDir = _nextTarget.position - transform.position;
                _moveDir.y = 0;
            }
            transform.Translate(_moveDir.normalized * Time.fixedDeltaTime * 10f);

            transform.rotation = Quaternion.Euler(0, 0, Time.fixedDeltaTime * 5f);
        }
    }

    public void SetTarget()
    {
        _skillNumberOfEnemy++;
        if (_skillNumberOfEnemy > _skill._skillNumberOfEnemy)
        {
            Destroy(gameObject);
            return;
        }

        if(_targetArray.Count > 0)
        {
            _nextTarget = _targetArray[0];
            _targetArray.RemoveAt(0);
            return;
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, _skillRange, LayerMask.GetMask("Boss", "Monster"));
        if(colliders.Length == 0)
        {
            _nextTarget = null;
            return;
        }
        else
        {
            if(colliders[0].gameObject.layer == LayerMask.NameToLayer("Boss"))
            {
                _nextTarget = colliders[0].transform;
                return;
            }
            foreach (Collider collider in colliders)
                _targetArray.Add(collider.transform);
            _nextTarget = _targetArray[0];
            _targetArray.RemoveAt(0);
            return;
        }
    }

    IEnumerator Delay()
    {
        _boxCollider.enabled = false;

        yield return new WaitForSeconds(_skillDuration);

        _boxCollider.enabled = true;
        SetTarget();
    }

    private void OnDestroy()
    {
        _activeSelf = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.transform == _nextTarget)
        {
            PlayerManager _playerManager = PlayerController.instance.PlayerManager;
            int minDamage = (int)(_playerManager._playerMinPower * _skillDamageMultiplier);
            int maxDamage = (int)(_playerManager._playerMaxPower * _skillDamageMultiplier);
            if(other.gameObject.layer == LayerMask.NameToLayer("Boss"))
            {
                other.transform.root.TryGetComponent<BossAI>(out var bossAI);
                bossAI.Damaged(minDamage, maxDamage, _skillNumberOfAttack);
            }
            else
            {
                other.TryGetComponent<MonsterAI>(out var _monsterAI);
                _monsterAI.Damaged(minDamage, maxDamage, _skillNumberOfAttack, PlayerController.instance.transform);
            }
            StartCoroutine(Delay());
        }
    }
}
