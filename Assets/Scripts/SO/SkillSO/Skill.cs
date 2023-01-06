using UnityEngine;
using UnityEngine.UI;

public enum SkillActivition { ACTIVE = 0, PASSIVE, }
public enum SkillType { INSTANT = 0, CASTING, KEYDOWN, BUFF, }

public class Skill : ScriptableObject
{
    public string _skillName;
    public SkillActivition _skillActivition;
    public SkillType _skillType;
    public string _skillDesc;
    public string _skillTrigger;
    public float _skillRange = 1;
    public float _skillDuration = 1;
    public float _skillDelay = 1;
    public Sprite _skillIconImage;
    public AudioClip _skillUseSound;
    public AudioClip _skillHitSound;
}
