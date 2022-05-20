using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
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

    public static PlayerInfo instance;

    private void Awake()
    {
        if(!instance)
        {
            instance = this;
        }
    }

    private void OnEnable()
    {
        UpdatePlayerStatus();
    }

    public void UpdatePlayerStatus()
    {
        PlayerManager playerManager = PlayerManager.instance;
        _playerName.text = playerManager._playerName;
        _playerLv.text = playerManager._playerLv.ToString();
        _playerStr.text = playerManager._playerSTR.ToString();
        _playerDex.text = playerManager._playerDEX.ToString();
        _playerInt.text = playerManager._playerINT.ToString();
        _playerLuk.text = playerManager._playerLUK.ToString();
        _playerStatPoint.text = playerManager._playerStatPoint.ToString();
        _playerPower[0].text = playerManager._playerMinPower.ToString();
        _playerPower[1].text = playerManager._playerMaxPower.ToString();
        _expPercent.text = string.Format("{0:F2}%", ((float)playerManager._playerExp * 100 / (float)playerManager._totalExp).ToString());
        _expBar.fillAmount = (float)playerManager._playerExp / (float)playerManager._totalExp;
        playerManager.SetPower();

        if(playerManager._playerStatPoint <= 0)
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
        PlayerManager playerManager = PlayerManager.instance;
        if (playerManager._playerStatPoint > 0)
        {
            playerManager._playerStatPoint--;
            switch (_value)
            {
                case 0:
                    playerManager._playerSTR++;
                    break;
                case 1:
                    playerManager._playerDEX++;
                    break;
                case 2:
                    playerManager._playerINT++;
                    break;
                case 3:
                    playerManager._playerLUK++;
                    break;
                default:
                    break;
            }
            UpdatePlayerStatus();
        }
    }
}
