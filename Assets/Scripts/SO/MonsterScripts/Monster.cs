using UnityEngine;

public class Monster : ScriptableObject
{
    public enum Type { Normal, Boss, }
    public Type _monsterType;
    public string _monsterName;
    public int _monsterLevel;
    public int _monsterHP;
    public int _monsterDamage;
    public int _monsterGiveExp;
    public float _monsterMoveSpeed;
    public float _monsterAttackDelay;
    public float _monsterRange;
    public AudioClip _attackSound;
    public AudioClip _hitSound;
    public AudioClip _deadSound;

    private void OnEnable()
    {
        string soundFile = "Sounds/SFX/Monsters/";
        string monsterName = _monsterName.Replace(" ", "");
        if (_monsterType == Type.Normal)
            soundFile += "Normal/" + monsterName + "/SFX_NormalMonster_";
        else if (_monsterType == Type.Boss)
            soundFile += "Boss/" + monsterName + "/SFX_BossMonster_";
        soundFile += monsterName;
        _attackSound = Resources.Load(soundFile + "_Attack") as AudioClip;
        _hitSound = Resources.Load(soundFile + "_Hit") as AudioClip;
        _deadSound = Resources.Load(soundFile + "_Die") as AudioClip;
    }
}
