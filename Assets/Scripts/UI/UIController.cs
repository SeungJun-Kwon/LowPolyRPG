using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;
using static DialogueQuest;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    [Header("HP, MP")]
    [SerializeField] private Image _hpOrb;
    [SerializeField] private Image _mpOrb;
    private int _playerHP, _playerMP;

    [Header("HP, MP Potion")]
    [SerializeField] Image _hpPotion;
    [SerializeField] Image _mpPotion;
    float _hpPotionCooldown, _mpPotionCooldown;

    [HideInInspector] public int _currentPlayerHP;
    [HideInInspector] public int _currentPlayerMP;

    SkillSlot _skillSlot;

    [HideInInspector] public GameObject PlayUI;

    [HideInInspector] public GameObject CinemachineUI;

    [Header("Player Info")]
    [SerializeField] GameObject _playerInfoPanel;
    PlayerInfo _playerInfo;
    [HideInInspector] public PlayerInfo PlayerInfo
    {
        get
        {
            if (!_playerInfo)
                _playerInfoPanel.TryGetComponent<PlayerInfo>(out _playerInfo);
            return _playerInfo;
        }
    }
    KeyCode _playerInfoOpen;

    [Header("Pause")]
    [SerializeField] GameObject _pausePanel;
    KeyCode _pauseOpen;

    [Header("Boss HP Bar")]
    [SerializeField] GameObject _bossHPBarPanel;
    BossHPBar _bossHPBar;
    [HideInInspector] public BossHPBar BossHPBar
    {
        get
        {
            if (!_bossHPBar)
                _bossHPBarPanel.TryGetComponent<BossHPBar>(out _bossHPBar);
            return _bossHPBar;
        }
    }

    NoticeArea _noticeArea;
    [HideInInspector] public NoticeArea NoticeArea
    {
        get
        {
            if (!_noticeArea)
            {
                GameObject noticeAreaPanel = PlayUI.transform.Find("MessageArea").gameObject;
                noticeAreaPanel.TryGetComponent<NoticeArea>(out _noticeArea);
            }
            return _noticeArea;
        }
    }

    [Header("Sounds")]
    [SerializeField] Slider _bgmSlider, _sfxSlider;

    [Header("BossEntry")]
    [SerializeField] GameObject _bossEntryPanel;
    BossEntry _bossEntry;
    [HideInInspector] public BossEntry BossEntry
    {
        get
        {
            if (!_bossEntry)
            {
                if (!_bossEntryPanel)
                    _bossEntryPanel = PlayUI.transform.Find("BossEntryUI").gameObject;
                _bossEntryPanel.TryGetComponent<BossEntry>(out _bossEntry);
            }
            return _bossEntry;
        }
    }

    [HideInInspector] GameObject _npcDialogue;

    [HideInInspector] public GameObject _playerDeathUI;
    [HideInInspector] public bool _isPlayerDeath = false;

    bool _isAnyUIOpen = false;
    bool _isPlayerInfoOpen = false;
    bool _isPauseOpen = false;

    PlayerManager _playerManager;

    private void Awake()
    {
        #region Singleton
        if (instance == null) //instance가 null. 즉, 시스템상에 존재하고 있지 않을때
        {
            instance = this; //내자신을 instance로 넣어줍니다.
            DontDestroyOnLoad(gameObject); //OnLoad(씬이 로드 되었을때) 자신을 파괴하지 않고 유지
        }
        else if (instance != this) { //instance가 내가 아니라면 이미 instance가 하나 존재하고 있다는 의미
                Destroy(this.gameObject); //둘 이상 존재하면 안되는 객체이니 방금 AWake된 자신을 삭제
        }
        #endregion

        PlayUI = transform.Find("PlayUI").gameObject;
        CinemachineUI = transform.Find("CinemachineUI").gameObject;

        _playerDeathUI = PlayUI.transform.Find("PlayerDeathUI").gameObject;
        _npcDialogue = PlayUI.transform.Find("NPCDialogue").gameObject;
        _playerInfoPanel.TryGetComponent(out _playerInfo);
        _bossHPBarPanel.TryGetComponent(out _bossHPBar);

        _skillSlot = GetComponentInChildren<SkillSlot>();
    }

    private void Start()
    {
        PlayerKeySetting playerKeySetting = PlayerController.instance.PlayerKeySetting;
        _playerInfoOpen = playerKeySetting._playerInfoOpen;
        _pauseOpen = playerKeySetting._esc;

        _playerManager = PlayerController.instance.PlayerManager;
        if (_playerManager == null)
        {
            _playerManager = FindObjectOfType<PlayerManager>();
        }
        _playerManager.OnValueChanged.AddListener(SetHpMpOrb);
        _playerHP = _playerManager.PlayerHP;
        _playerMP = _playerManager.PlayerMP;
        SetInit();

        _bgmSlider.onValueChanged.AddListener(SoundManager.instance.BGMVolume);
        _sfxSlider.onValueChanged.AddListener(SoundManager.instance.SFXVolume);
    }

    public void SetInit()
    {
        _playerInfoPanel.SetActive(false);
        _isPlayerInfoOpen = false;
        _pausePanel.SetActive(false);
        _isPauseOpen = false;
        _npcDialogue.SetActive(false);
        _currentPlayerHP = _playerHP;
        _currentPlayerMP = _playerMP;
        _hpPotionCooldown = 0;
        _mpPotionCooldown = 0;
        _isPlayerDeath = false;
        Time.timeScale = 1;
    }

    private void LateUpdate()
    {
        if (!_isPlayerDeath)
        {
            if (Input.GetKeyDown(_playerInfoOpen))
            {
                if (!_isPlayerInfoOpen)
                    _playerInfoPanel.SetActive(true);
                else
                    _playerInfoPanel.SetActive(false);

                _isPlayerInfoOpen = _playerInfo.gameObject.activeSelf;
            }

            if (Input.GetKeyDown(_pauseOpen))
            {
                _isAnyUIOpen = _isPlayerInfoOpen;
                if (!_isAnyUIOpen)
                {
                    _isPauseOpen = _pausePanel.gameObject.activeSelf;

                    if (!_isPauseOpen)
                    {
                        _pausePanel.SetActive(true);
                        PlayerController.instance.SetMyState(State.CANTMOVE);
                        Time.timeScale = 0;
                    }
                    else
                    {
                        _pausePanel.SetActive(false);
                        PlayerController.instance.SetMyState(State.CANMOVE);
                        Time.timeScale = 1;
                    }
                }
            }

            _hpOrb.fillAmount = Mathf.Lerp(_hpOrb.fillAmount, (float)_currentPlayerHP / (float)_playerHP, Time.deltaTime * 5f);
            _mpOrb.fillAmount = Mathf.Lerp(_mpOrb.fillAmount, (float)_currentPlayerMP / (float)_playerMP, Time.deltaTime * 5f);
        }
    }

    void SetHpMpOrb()
    {
        _playerHP = _playerManager.PlayerHP;
        _playerMP = _playerManager.PlayerMP;
        _currentPlayerHP = _playerManager.CurrentHP;
        _currentPlayerMP = _playerManager.CurrentMP;
    }

    public void UseSkill(int index) => _skillSlot.UseSkill(index);

    public void SetAsLastSibling(GameObject go) => go.transform.SetAsLastSibling();

    public void SetActiveBossHPBar(BossMonster monster, BossAI bossAI, bool trigger)
    {
        if(trigger)
            _bossHPBar.GetBossInformation(monster, bossAI);
        _bossHPBarPanel.SetActive(trigger);
    }

    public void UsePotion(KeyAction action)
    {
        if(action == KeyAction.HEALTHPOTION)
        {
            if(_hpPotionCooldown > 0)
            {
                NoticeArea.GetMessage("아직 HP 포션을 사용할 수 없습니다.");
                return;
            }
            else if(_currentPlayerHP >= _playerHP)
            {
                NoticeArea.GetMessage("HP가 가득 찼습니다.");
                return;
            }

            StartCoroutine(HPPotionCooldown());
        }
        else if(action == KeyAction.MANAPOTION)
        {
            if (_mpPotionCooldown > 0)
            {
                NoticeArea.GetMessage("아직 MP 포션을 사용할 수 없습니다.");
                return;
            }
            else if(_currentPlayerMP >= _playerMP)
            {
                NoticeArea.GetMessage("MP가 가득 찼습니다.");
                return;
            }

            StartCoroutine(MPPotionCooldown());
        }

        SetHpMpOrb();
    }

    IEnumerator HPPotionCooldown()
    {
        _hpPotionCooldown = _playerManager._hpPotionCooldown;
        _playerManager.CurrentHP += (int)((float)_playerManager.PlayerHP * 0.3f);

        while (_hpPotionCooldown > 0)
        {
            _hpPotionCooldown -= Time.deltaTime;

            _hpPotion.fillAmount = (_playerManager._hpPotionCooldown - _hpPotionCooldown) / _playerManager._hpPotionCooldown;

            yield return null;
        }
    }

    IEnumerator MPPotionCooldown()
    {
        _mpPotionCooldown = _playerManager._mpPotionCooldown;
        _playerManager.CurrentMP += (int)((float)_playerManager.PlayerMP * 0.3f);

        while(_mpPotionCooldown > 0)
        {
            _mpPotionCooldown -= Time.deltaTime;

            _mpPotion.fillAmount = (_playerManager._mpPotionCooldown - _mpPotionCooldown) / _playerManager._mpPotionCooldown;

            yield return null;
        }
    }
}
