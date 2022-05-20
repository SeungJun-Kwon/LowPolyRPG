using UnityEngine;

public class Monster : ScriptableObject
{
    public enum Type { Normal, Boss, }

    public string _monsterName;
    public int _monsterLevel;
    public int _monsterHP;
    public int _monsterDamage;
    public int _monsterGiveExp;
    public float _monsterMoveSpeed;
    public float _monsterAttackDelay;
}
