using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public string _playerName = "Default";
    public float _playerSpeed = 5;
    public int _playerHP = 100;
    public int _playerMP = 50;
    public int _playerSTR = 5;
    public int _playerDEX = 5;
    public int _playerINT = 5;
    public int _playerLUK = 5;
    public int _playerStatPoint = 0;
    public int _playerPower = 0;
    public int _playerMinPower = 0, _playerMaxPower = 0;
    public int _playerLv = 1;
    public int _playerExp = 0, _totalExp = 50;

    List<Quest> _currentQuest = new List<Quest>();
    List<Quest> _completedQuest = new List<Quest>();

    int _addedSTR = 0;
    int _addedDEX = 0;
    int _addedINT = 0;
    int _addedLUK = 0;
    int _addedPower = 0;

    private void Start()
    {
        SetPower();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            LevelUp();
        }
    }

    public void SetPower()
    {
        _playerPower = (int)((_playerSTR + _addedSTR) * 2 + (_playerDEX + _addedDEX) * 1 + (_playerLUK + _addedLUK) * 0.5 + (_playerINT + _addedINT) * 0.1);
        _playerMinPower = (int)((_playerPower + _addedPower) * 0.8);
        _playerMaxPower = (int)((_playerPower + _addedPower) * 1.1);
    }

    public void BuffPower(float value)
    {
        _addedSTR = (int)(_playerSTR * value);
        _addedDEX = (int)(_playerDEX * value);
        _addedLUK = (int)(_playerLUK * value);
        _addedINT = (int)(_playerINT * value);
    }

    public void GainExp(int _value)
    {
        _playerExp += _value;
        PlayerInfo playerInfo = UIController.instance.PlayerInfo;
        if (playerInfo.gameObject.activeSelf)
            playerInfo.UpdatePlayerStatus();
        if (_playerExp >= _totalExp)
            LevelUp();
    }

    public void LevelUp()
    {
        _playerLv++;
        _playerStatPoint += 2;
        switch (_playerLv)
        {
            case < 50:
                _totalExp = (int)(_totalExp * 1.1);
                break;
            case < 100:
                _totalExp = (int)(_totalExp * 1.5);
                break;
            case < 200:
                _totalExp = (int)(_totalExp * 2);
                break;
            default:
                _totalExp = (int)(_totalExp * 3);
                break;
        }
        _playerExp = 0;
        PlayerInfo playerInfo = UIController.instance.PlayerInfo;
        if (playerInfo.gameObject.activeSelf)
            playerInfo.UpdatePlayerStatus();
    }

    public List<Quest> GetCurrentQuests()
    {
        return _currentQuest;
    }

    public List<Quest> GetCompletedQuests()
    {
        return _completedQuest;
    }

    public void AddQuest(Quest quest)
    {
        _currentQuest.Add(quest);
    }

    public void RemoveQuest(Quest quest)
    {
        _currentQuest.Remove(quest);
    }

    public void CompleteQuest(Quest quest)
    {
        _currentQuest.Remove(quest);
        _completedQuest.Add(quest);
    }
}