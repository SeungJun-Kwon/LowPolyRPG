using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Skill")]
public class Skill : ScriptableObject
{
    public string _skillName;
    public string _skillDesc;
    public int _skillDamageMultiplier;
    public float _skillRange;
    public float _skillCoolTime;
}
