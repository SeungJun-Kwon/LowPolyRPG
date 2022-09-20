using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    [Header("HP, MP")]
    [SerializeField] private Image _hpOrb;
    [SerializeField] private Image _mpOrb;
    private int _playerHP, _playerMP;
    private int _currentPlayerHP, _currentPlayerMP;

    SkillSlot _skillSlot;

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

    [Header("Quest Info")]
    [SerializeField] GameObject _questInfoPanel;
    QuestInfo _questInfo;
    [HideInInspector] public QuestInfo QuestInfo
    {
        get
        {
            if (!_questInfo)
                _questInfoPanel.TryGetComponent<QuestInfo>(out _questInfo);
            return _questInfo;
        }
    }
    KeyCode _questInfoOpen;

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
                GameObject noticeAreaPanel = transform.Find("MessageArea").gameObject;
                noticeAreaPanel.TryGetComponent<NoticeArea>(out _noticeArea);
            }
            return _noticeArea;
        }
    }

    [Header("Sounds")]
    [SerializeField] Slider _bgmSlider, _sfxSlider;

    bool _isPlayerInfoOpen = false;
    bool _isQuestInfoOpen = false;
    bool _isPauseOpen = false;

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

        _playerInfoPanel.TryGetComponent<PlayerInfo>(out _playerInfo);
        _questInfoPanel.TryGetComponent<QuestInfo>(out _questInfo);
        _bossHPBarPanel.TryGetComponent<BossHPBar>(out _bossHPBar);

        _skillSlot = GetComponentInChildren<SkillSlot>();
    }

    private void Start()
    {
        PlayerKeySetting playerKeySetting = PlayerController.instance.PlayerKeySetting;
        _playerInfoOpen = playerKeySetting._playerInfoOpen;
        _questInfoOpen = playerKeySetting._questInfoOpen;
        _pauseOpen = playerKeySetting._esc;

        PlayerManager playerManager = PlayerController.instance.PlayerManager;
        if (playerManager == null)
        {
            playerManager = FindObjectOfType<PlayerManager>();
        }
        _playerHP = playerManager._playerHP;
        _playerMP = playerManager._playerMP;
        _currentPlayerHP = _playerHP;
        _currentPlayerMP = _playerMP;

        _bgmSlider.onValueChanged.AddListener(SoundManager.instance.BGMVolume);
        _sfxSlider.onValueChanged.AddListener(SoundManager.instance.SFXVolume);
    }

    private void LateUpdate()
    {
        if(Input.GetKeyDown(_playerInfoOpen))
        {
            _isPlayerInfoOpen = _playerInfo.gameObject.activeSelf;

            if (!_isPlayerInfoOpen)
                _playerInfoPanel.SetActive(true);
            else
                _playerInfoPanel.SetActive(false);
        }

        if(Input.GetKeyDown(_questInfoOpen))
        {
            _isQuestInfoOpen = _questInfo.gameObject.activeSelf;
            if (!_isQuestInfoOpen)
            {
                List<Quest> currentQuest = PlayerController.instance.PlayerManager.GetCurrentQuests();
                _questInfo.SetQuest(currentQuest);
                _questInfoPanel.SetActive(true);
            }
            else
                _questInfoPanel.SetActive(false);
        }

        if(Input.GetKeyDown(_pauseOpen))
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

    public void UseSkill(int index) => _skillSlot.UseSkill(index);

    public void SetActiveBossHPBar(BossMonster monster, bool trigger)
    {
        if(trigger)
            _bossHPBar.GetBossInformation(monster);
        _bossHPBarPanel.SetActive(trigger);
    }
}
