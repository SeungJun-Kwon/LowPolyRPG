using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public string _playerName = "Default";
    public float _playerSpeed = 5;
    public int _playerHP = 100;
    public int _playerMP = 50;
    public int _playerSTR = 5;
    public int _playerINT = 5;
    public int _playerDEX = 5;
    public int _playerLUK = 5;
    public int _playerStatPoint = 0;
    public int _playerPower = 0;
    public int _playerMinPower = 0, _playerMaxPower = 0;
    public int _playerLv = 1;
    public int _playerExp = 0, _totalExp = 50;

    private void Awake()
    {
        if (instance == null) //instance가 null. 즉, 시스템상에 존재하고 있지 않을때
        {
            instance = this; //내자신을 instance로 넣어줍니다.
            DontDestroyOnLoad(gameObject); //OnLoad(씬이 로드 되었을때) 자신을 파괴하지 않고 유지
        }
        else if (instance != this) //instance가 내가 아니라면 이미 instance가 하나 존재하고 있다는 의미
            Destroy(this.gameObject); //둘 이상 존재하면 안되는 객체이니 방금 AWake된 자신을 삭제
    }

    private void Start()
    {
        SetPower();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            GainExp(_totalExp);
    }

    public void SetPower()
    {
        _playerPower = (int)(_playerSTR * 2 + _playerDEX * 1 + _playerLUK * 0.5 + _playerINT * 0.1);
        _playerMinPower = (int)(_playerPower * 0.8);
        _playerMaxPower = (int)(_playerPower * 1.1);
    }

    public void GainExp(int _value)
    {
        _playerExp += _value;
        if (_playerExp >= _totalExp)
            LevelUp();
        if(PlayerInfo.instance && PlayerInfo.instance.gameObject.activeSelf)
            PlayerInfo.instance.UpdatePlayerStatus();
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
    }
}