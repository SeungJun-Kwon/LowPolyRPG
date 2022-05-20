using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    [Header("Key Setting")]
    public KeyCode _inventoryOpen = KeyCode.I;
    public KeyCode _playerInfoOpen = KeyCode.P;

    [Header("HP, MP")]
    [SerializeField] private Image _hpOrb;
    [SerializeField] private Image _mpOrb;
    private int _playerHP, _playerMP;
    private int _currentPlayerHP, _currentPlayerMP;

    [Header("Skills")]
    [SerializeField] public Image[] _skillIcon = new Image[4];

    [Header("Inventory")]
    public GameObject _inventoryPanel;

    [Header("Player Info")]
    [SerializeField] GameObject _playerInfoPanel;

    private bool _isInventoryOpen = false;
    private bool _isPlayerInfoOpen = false;

    private void Awake()
    {
        #region Singleton
        if (instance == null) //instance�� null. ��, �ý��ۻ� �����ϰ� ���� ������
        {
            instance = this; //���ڽ��� instance�� �־��ݴϴ�.
            DontDestroyOnLoad(gameObject); //OnLoad(���� �ε� �Ǿ�����) �ڽ��� �ı����� �ʰ� ����
        }
        else
        {
            if (instance != this) //instance�� ���� �ƴ϶�� �̹� instance�� �ϳ� �����ϰ� �ִٴ� �ǹ�
                Destroy(this.gameObject); //�� �̻� �����ϸ� �ȵǴ� ��ü�̴� ��� AWake�� �ڽ��� ����
        }
        #endregion

        if(PlayerManager.instance == null)
        {
            PlayerManager.instance = FindObjectOfType<PlayerManager>();
        }

        _playerHP = PlayerManager.instance._playerHP;
        _playerMP = PlayerManager.instance._playerMP;
        _currentPlayerHP = _playerHP;
        _currentPlayerMP = _playerMP;
    }

    private void LateUpdate()
    {
        if(Input.GetKeyDown(_inventoryOpen))
        {
            TryGetComponent<SaveSystem>(out var _saveSystem);
            if (_isInventoryOpen)
            {
                _isInventoryOpen = false;
                _saveSystem.SaveInventory();
                _inventoryPanel.SetActive(false);
            }
            else
            {
                _isInventoryOpen = true;
                _saveSystem.LoadInventory();
                _inventoryPanel.SetActive(true);
            }
        }
        if(Input.GetKeyDown(_playerInfoOpen))
        {
            if (!_isPlayerInfoOpen)
            {
                _playerInfoPanel.SetActive(true);
                _isPlayerInfoOpen = true;
            }
            else
            {
                _playerInfoPanel.SetActive(false);
                _isPlayerInfoOpen = false;
            }
        }

        _hpOrb.fillAmount = Mathf.Lerp(_hpOrb.fillAmount, (float)_currentPlayerHP / (float)_playerHP, Time.deltaTime * 5f);
        _mpOrb.fillAmount = Mathf.Lerp(_mpOrb.fillAmount, (float)_currentPlayerMP / (float)_playerMP, Time.deltaTime * 5f);
    }

    public void SetHPOrb(int _value)
    {
        _currentPlayerHP += _value;
        if (_currentPlayerHP <= 0)
            _currentPlayerHP = 0;
        else if (_currentPlayerHP >= _playerHP)
            _currentPlayerHP = _playerHP;
    }

    public void SetMPOrb(int _value)
    {
        _currentPlayerMP += _value;
        if (_currentPlayerMP <= 0)
            _currentPlayerMP = 0;
        else if (_currentPlayerMP >= _playerMP)
            _currentPlayerMP = _playerMP;
    }
}
