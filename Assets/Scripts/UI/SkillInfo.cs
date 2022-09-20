using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillInfo : MonoBehaviour
{
    [SerializeField] Text _skillName, _skillDescription;

    Skill _skill;

    public void GetSkill(Skill skill)
    {
        _skill = skill;
        _skillName.text = _skill.name;
        _skillDescription.text = _skill._skillDesc;
    }
}
