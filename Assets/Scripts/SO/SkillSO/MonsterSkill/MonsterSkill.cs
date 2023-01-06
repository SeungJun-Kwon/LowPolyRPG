using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterSkill", menuName = "Skill/MonsterSkill")]
public class MonsterSkill : Skill
{
    public bool _isPercentageAttack = false;
    public int _skillDamage = 1;
}
