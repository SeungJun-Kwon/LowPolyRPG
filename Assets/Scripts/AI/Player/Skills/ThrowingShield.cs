using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ThrowingShield : MonoBehaviour
{
    [SerializeField] private Skill _skill;

    private Transform _startPos = null;
    private Collider _nextTarget = null;
    private Transform _targetPos = null;
    private List<Collider> _targetArray;
    private float _skillDamageMultiplier;
    private float _skillRange;
    private int _skillNumberOfAttack;

    private void Awake()
    {
        _skillDamageMultiplier = _skill._skillDamageMultiplier;
        _skillRange = _skill._skillRange;
        _skillNumberOfAttack = _skill._skillNumberOfAttack;
    }

    private void OnEnable()
    {
        _startPos = transform;
        transform.rotation = Quaternion.Euler(-90, 0, 0);
        _targetArray = Physics.OverlapSphere(transform.position, _skillRange, LayerMask.GetMask("Boss", "Monster")).ToList();
        _nextTarget = _targetArray[0];
        _targetPos = _nextTarget.transform;
        _targetArray.RemoveAt(0);
    }

    private void FixedUpdate()
    {
        if(gameObject.activeSelf)
        {
            if(_targetPos)
                transform.position = Vector3.MoveTowards(transform.position, _targetPos.position, Time.fixedDeltaTime * 10f);
            transform.rotation = Quaternion.Euler(0, 0, Time.fixedDeltaTime * 5f);
            SetTarget();
        }
    }

    void SetTarget()
    {
        if((_targetPos.position - transform.position).magnitude <= 0.5f || !_targetPos || !_nextTarget || !_nextTarget.gameObject || !_nextTarget.gameObject.activeSelf)
        {
            if (_targetArray.Count > 0)
            {
                _nextTarget = _targetArray[0];
                _targetArray.RemoveAt(0);
                _targetPos = _nextTarget.transform;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.TryGetComponent<MonsterAI>(out var _monsterAI);
            int minDamage = (int)(PlayerManager.instance._playerMinPower * _skillDamageMultiplier);
            int maxDamage = (int)(PlayerManager.instance._playerMaxPower * _skillDamageMultiplier);
            _monsterAI.Damaged(minDamage, maxDamage, _skillNumberOfAttack, null);
        }
    }
}
