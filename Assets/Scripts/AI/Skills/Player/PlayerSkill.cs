using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSkill", menuName = "Skill/PlayerSkill")]
public class PlayerSkill : Skill
{
    [Header("Player Skill")]
    public int _skillNumberOfAttack = 1;
    public int _skillNumberOfEnemy = 1;
    public float _skillDamageMultiplier = 1;
    public float _skillCoolTime = 1;
    public bool _needEnemyInRange = false;
}
