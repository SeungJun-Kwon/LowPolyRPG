using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;
    public Skill[] _skill;
    public GameObject[] _skillPrefab;
    public Transform[] _skillTransform;

    private enum SkillKey { Q = 0, W, E, R };
    private float[] _currentCoolTime = new float[4];

    private void Awake()
    {
        if(!instance)
        {
            instance = this;
        }
    }

    private void Start()
    {
        for(int i = 0; i < _currentCoolTime.Length; i++)
        {
            _currentCoolTime[i] = _skill[i]._skillCoolTime;
        }
    }

    private void Update()
    {
        for (int i = 0; i < _currentCoolTime.Length; i++)
        {
            if (_currentCoolTime[i] < _skill[i]._skillCoolTime)
            {
                _currentCoolTime[i] += Time.deltaTime;
                UIController.instance._skillIcon[i].fillAmount = (_skill[i]._skillCoolTime - _currentCoolTime[i]) / _skill[i]._skillCoolTime;
            }

        }
    }

    public Skill GetSkill(string _key)
    {
        int _skillKey = (int)Enum.Parse<SkillKey>(_key);
        return _skill[_skillKey];
    }

    public bool IsReady(string _key)
    {
        int _skillKey = (int)Enum.Parse<SkillKey>(_key);
        if (_currentCoolTime[_skillKey] >= _skill[(int)_skillKey]._skillCoolTime)
            return true;

        return false;
    }

    public void Use(string _key)
    {
        int _skillKey = (int)Enum.Parse<SkillKey>(_key);
        _currentCoolTime[_skillKey] = 0;
    }

    public void Instantiate(string _key)
    {
        int _skillKey = (int)Enum.Parse<SkillKey>(_key);
        Instantiate(_skillPrefab[_skillKey], _skillTransform[_skillKey].position, Quaternion.identity);
    }

    public bool IsEnemyInRange(string _key)
    {
        int _skillKey = (int)Enum.Parse<SkillKey>(_key);
        float _range = _skill[_skillKey]._skillRange;
        if (Physics.OverlapSphere(transform.position, _range, LayerMask.GetMask("Boss", "Monster")).Length > 0)
        {
            return true;
        }

        return false;
    }
}
