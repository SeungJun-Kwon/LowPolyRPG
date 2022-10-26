using UnityEngine;
using UnityEngine.UI;

public class Skill : ScriptableObject
{
    public string _skillName;
    public string _skillDesc;
    public string _skillTrigger;
    public float _skillRange = 1;
    public float _skillDuration = 1;
    public float _skillDelay = 1;
    public Sprite _skillIconImage;
}
