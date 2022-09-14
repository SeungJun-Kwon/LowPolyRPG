using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Skill", menuName = "Skill")]
public class Skill : ScriptableObject
{
    public string _skillName;
    public string _skillDesc;
    public string _skillTrigger;
    public int _skillNumberOfAttack = 1;
    public int _skillNumberOfEnemy = 1;
    public float _skillDamageMultiplier = 1;
    public float _skillRange = 1;
    public float _skillDuration = 1;
    public float _skillDelay = 1;
    public float _skillCoolTime = 1;
    public Sprite _skillIconImage;
}
