using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    [SerializeField] SkillIcon[] _skillIcon;

    public SkillInfo _skillInfo;

    List<Skill> _skill = new List<Skill>();

    private void Start()
    {
        Skill[] skills = SkillManager.instance._skill;
        for (int i = 0; i < skills.Length; i++)
        {
            _skill.Add(skills[i]);
            _skillIcon[i].SetSkill(_skill[i]);
        }
    }

    public void UseSkill(int index) => _skillIcon[index].UseSkill();
}
