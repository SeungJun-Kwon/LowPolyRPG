using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : BaseUI
{
    [Header("UI Text")]
    [SerializeField] Text _playerName;
    [SerializeField] Text _playerLv;
    [SerializeField] Text _playerStr;
    [SerializeField] Text _playerDex;
    [SerializeField] Text _playerInt;
    [SerializeField] Text _playerLuk;
    [SerializeField] Text _playerStatPoint;
    [SerializeField] Button[] _playerStatPointButton;
    [SerializeField] Text[] _playerPower;
    [SerializeField] Text _expPercent;
    [SerializeField] Image _expBar;

    PlayerManager _playerManager;

    protected override void OnEnable()
    {
        base.OnEnable();
        _playerManager = PlayerController.instance.PlayerManager;
        UpdatePlayerStatus();
        _playerManager.OnValueChanged.AddListener(UpdatePlayerStatus);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _playerManager.OnValueChanged.RemoveListener(UpdatePlayerStatus);
    }

    public void UpdatePlayerStatus()
    {
        if (_playerManager == null)
            _playerManager = PlayerController.instance.PlayerManager;

        _playerName.text = _playerManager._playerName;
        _playerLv.text = _playerManager._playerLv.ToString();
        _playerStr.text = _playerManager._playerSTR.ToString();
        _playerDex.text = _playerManager._playerDEX.ToString();
        _playerInt.text = _playerManager._playerINT.ToString();
        _playerLuk.text = _playerManager._playerLUK.ToString();
        _playerStatPoint.text = _playerManager._playerStatPoint.ToString();
        _playerPower[0].text = _playerManager._playerMinPower.ToString();
        _playerPower[1].text = _playerManager._playerMaxPower.ToString();
        _expPercent.text = string.Format("{0:F2}%", ((float)_playerManager.CurrentExp * 100 / (float)_playerManager._totalExp).ToString());
        _expBar.fillAmount = (float)_playerManager.CurrentExp / (float)_playerManager._totalExp;

        if(_playerManager._playerStatPoint <= 0)
        {
            for (int i = 0; i < _playerStatPointButton.Length; i++)
            {
                _playerStatPointButton[i].interactable = false;
                _playerStatPointButton[i].gameObject.SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < _playerStatPointButton.Length; i++)
            {
                _playerStatPointButton[i].interactable = true;
                _playerStatPointButton[i].gameObject.SetActive(true);
            }
        }
    }

    public void StatUp(int _value)
    {
        if (_playerManager._playerStatPoint > 0)
        {
            _playerManager._playerStatPoint--;
            switch (_value)
            {
                case 0:
                    _playerManager._playerSTR++;
                    _playerManager.PlayerHP += 10;
                    _playerManager.CurrentHP += 10;
                    break;
                case 1:
                    _playerManager._playerDEX++;
                    _playerManager._attackSpeed *= 0.95f;
                    break;
                case 2:
                    _playerManager._playerINT++;
                    _playerManager.PlayerMP += 10;
                    _playerManager.CurrentMP += 10;
                    break;
                case 3:
                    _playerManager._playerLUK++;
                    break;
                default:
                    break;
            }
            _playerManager.SetPower();
        }
    }
}
