using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillIcon : MonoBehaviour
{
    [SerializeField] Image _skillImage, _skillFill;

    Skill _skill;

    float _currentCoolTime;

    public void SetSkill(Skill skill) => _skill = skill;

    public void UseSkill()
    {
        _currentCoolTime = 0f;
        StartCoroutine(StartCoolDown());
    }

    IEnumerator StartCoolDown()
    {
        if (_currentCoolTime < _skill._skillCoolTime)
        {
            _skillFill.fillAmount = (_skill._skillCoolTime - _currentCoolTime) / _skill._skillCoolTime;
            _currentCoolTime += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        else
            yield break;
    }
}
