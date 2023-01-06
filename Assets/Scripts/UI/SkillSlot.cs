using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : BaseUI
{
    [SerializeField] SkillIcon[] _skillIcon;

    public SkillInfo _skillInfo;

    List<PlayerSkill> _skill = new List<PlayerSkill>();

    private void Start()
    {
        PlayerSkill[] skills = SkillManager.instance._skill;
        for (int i = 0; i < skills.Length; i++)
        {
            _skill.Add(skills[i]);
            _skillIcon[i].SetSkill(_skill[i]);
        }
    }

    public void UseSkill(int index) => _skillIcon[index].UseSkill();
}
