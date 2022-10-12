using UnityEngine;

[CreateAssetMenu(fileName = "HuntingQuest", menuName = "Quest/HuntingQuest")]
public class HuntingQuest : Quest
{
    public Monster _targetMonster;
    public int _numberOfHunts;
    [HideInInspector]
    public int _currentNumberOfHunts = 0;

    private void Awake()
    {
        this._type = Type.HUNTING;
    }

    public void HuntMonster()
    {
        if(_currentNumberOfHunts < _numberOfHunts)
        {
            _currentNumberOfHunts++;
            UIController.instance.NoticeArea.GetMessage(_targetMonster.name + " " + _currentNumberOfHunts + "/" + _numberOfHunts);
            if (_currentNumberOfHunts == _numberOfHunts)
                _canComplete = true;
        }
    }
}
