using UnityEngine;

[CreateAssetMenu(fileName = "HuntingQuest", menuName = "Quest/HuntingQuest")]
public class HuntingQuest : Quest
{
    public Monster _targetMonster;
    public int _numberOfHunts;

    private void Awake()
    {
        this._type = Type.HUNTING;
    }
}
