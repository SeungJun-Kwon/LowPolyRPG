using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "BossMonster", menuName = "Monster/BossMonster")]
public class BossMonster : Monster
{
    public int _stiffness;
    public int _stiffnessCount;
    public float _stiffnessDuration;
    public Sprite _bossImage;

    [Header("Attack & Skill")]
    public Skill[] _skill;
    public GameObject[] _skillPrefab;
}
