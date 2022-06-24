using UnityEngine;

[CreateAssetMenu(fileName = "HuntingQuest", menuName = "Quest/HuntingQuest")]
public class HuntingQuest : Quest
{
    public Monster _targetMonster;
    public int _numberOfHunts;
    int _currentNumberOfHunts = 0;

    private void Awake()
    {
        this._type = Type.HUNTING;
    }

    public void HuntMonster()
    {
        if(_currentNumberOfHunts < _numberOfHunts)
        {
            _currentNumberOfHunts++;
            if (_currentNumberOfHunts == _numberOfHunts)
                _canComplete = true;
        }
    }
}
