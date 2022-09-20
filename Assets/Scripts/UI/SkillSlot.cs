using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    [SerializeField] SkillIcon[] _skillIcon;

    [SerializeField] List<Skill> _skill;

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
