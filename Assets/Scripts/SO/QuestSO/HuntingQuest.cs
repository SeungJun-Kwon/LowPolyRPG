using UnityEngine;

[CreateAssetMenu(fileName = "HuntingQuest", menuName = "Quest/HuntingQuest")]
public class HuntingQuest : Quest
{
    public Monster _targetMonster;
    public int _numberOfHunts;

    protected override void Awake()
    {
        base.Awake();
        this._type = Type.HUNTING;
    }
}
