using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;
    public PlayerSkill[] _skill;
    public GameObject[] _skillPrefab;
    public Transform[] _skillTransform;
    
    GameObject[] _skills = new GameObject[4];
    SkillScript[] _skillScripts = new SkillScript[4];

    enum SkillKey { Q = 0, W, E, R };
    float[] _currentCoolTime = new float[4];

    PlayerManager _playerManager;

    private void Awake()
    {
        if(!instance)
        {
            instance = this;
        }
    }

    private void Start()
    {
        for (int i = 0; i < _skill.Length; i++)
        {
            _currentCoolTime[i] = _skill[i]._skillCoolTime;
            _skillPrefab[i].TryGetComponent(out _skillScripts[i]);
            if (_skillScripts[i] != null && _skill[i]._skillActivition == SkillActivition.PASSIVE)
            {
                PlayerController.instance.gameObject.AddComponent(_skillScripts[i].GetType());
                SkillScript skillScript = PlayerController.instance.GetComponent(_skillScripts[i].GetType()) as SkillScript;
                skillScript._skill = _skill[i];
            }
        }

        _playerManager = PlayerController.instance.PlayerManager;
    }

    private void Update()
    {
        for (int i = 0; i < _currentCoolTime.Length; i++)
        {
            if (_currentCoolTime[i] < _skill[i]._skillCoolTime)
                _currentCoolTime[i] += Time.deltaTime;
        }
    }

    public Skill GetSkill(string _key)
    {
        int skillKey = (int)Enum.Parse<SkillKey>(_key);
        return _skill[skillKey];
    }

    public SkillActivition GetSkillActivition(string value)
    {
        int skillKey = (int)Enum.Parse<SkillKey>(value);
        return _skill[skillKey]._skillActivition;
    }

    public SkillType GetSkillType(string value)
    {
        int skillKey = (int)Enum.Parse<SkillKey>(value);
        return _skill[skillKey]._skillType;
    }

    public KeyCode GetSkillKey(Skill skill)
    {
        for(int i = 0; i < _skill.Length; i++)
        {
            if (_skill[i] == skill)
            {
                return PlayerController.instance.PlayerKeySetting._skill[i];
            }
        }
        return KeyCode.None;
    }

    public bool IsReady(string _key)
    {
        int skillKey = (int)Enum.Parse<SkillKey>(_key);
        return IsReady(_skill[skillKey]);
    }

    public bool IsReady(Skill skill)
    {
        for(int skillKey = 0; skillKey < _skill.Length; skillKey++)
        {
            if (_skill[skillKey].name == skill.name)
            {
                if (_playerManager.CurrentMP < _skill[skillKey]._requireMP)
                {
                    if (_skill[skillKey]._skillActivition != SkillActivition.PASSIVE)
                        UIController.instance.NoticeArea.GetMessage("MP가 부족합니다.");
                    return false;
                }
                else if (_currentCoolTime[skillKey] < _skill[skillKey]._skillCoolTime)
                {
                    if (_skill[skillKey]._skillActivition != SkillActivition.PASSIVE)
                        UIController.instance.NoticeArea.GetMessage(String.Format("아직 스킬을 사용할 수 없습니다.(남은 시간 {0}초)"
                            , (int)(_skill[skillKey]._skillCoolTime - _currentCoolTime[skillKey])));
                    return false;
                }
                else if (_skill[(int)skillKey]._needEnemyInRange)
                {
                    if (IsEnemyInRange((int)skillKey))
                        return true;
                    else
                        return false;
                }
                return true;
            }
        }

        return false;
    }

    public void Use(string _key)
    {
        int skillKey = (int)Enum.Parse<SkillKey>(_key);
        Use(_skill[skillKey]);
    }

    public void Use(Skill skill)
    {
        for(int i = 0; i < _skill.Length; i++)
        {
            if (_skill[i].name == skill.name)
            {
                _currentCoolTime[i] = 0;
                _playerManager.CurrentMP -= _skill[i]._requireMP;
                UIController.instance.UseSkill(i);
                SoundManager.instance.SFXPlay(_skill[i]._skillUseSound);
                return;
            }
        }
    }

    public void Activation(string _key)
    {
        int skillKey = (int)Enum.Parse<SkillKey>(_key);
        _skillPrefab[skillKey].SetActive(true);
    }

    public void Instantiate(string _key)
    {
        int skillKey = (int)Enum.Parse<SkillKey>(_key);
        _skills[skillKey] = Instantiate(_skillPrefab[skillKey], _skillTransform[skillKey].position, Quaternion.identity);
        SoundManager.instance.SFXPlay(_skill[skillKey]._skillUseSound);
    }

    public void Destroy(Skill skill)
    {
        int index;
        for(index = 0; index < _skill.Length; index++)
        {
            if (skill == _skill[index])
                break;
        }
        if (index >= _skill.Length)
            return;

        if (_skills[index] != null)
        {
            _skills[index].TryGetComponent(out SkillScript skillScript);
            skillScript.OnEnd();
            _skills[index] = null;
        }
    }

    public void Destroy(string _key)
    {
        int skillKey = (int)Enum.Parse<SkillKey>(_key);
        Destroy(_skill[skillKey]);
    }

    public bool IsEnemyInRange(int skillKey)
    {
        float _range = _skill[skillKey]._skillRange;
        if (Physics.OverlapSphere(transform.position, _range, LayerMask.GetMask("Boss", "Monster"), QueryTriggerInteraction.Collide).Length > 0)
        {
            return true;
        }

        return false;
    }
}
