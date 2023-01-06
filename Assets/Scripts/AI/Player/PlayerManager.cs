using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
    [HideInInspector] public UnityEvent OnValueChanged;

    public string _playerName = "Default";
    public float _playerSpeed = 3.5f;
    public float _playerMaxSpeed = 5;
    public float _attackSpeed = 1;
    [SerializeField] int _playerHP = 100;
    public int PlayerHP
    {
        get => _playerHP;
        set
        {
            _playerHP = value;
            OnValueChanged.Invoke();
        }
    }
    [SerializeField] int _playerMP = 100;
    public int PlayerMP
    {
        get => _playerMP;
        set
        {
            _playerMP = value;
            OnValueChanged.Invoke();
        }
    }
    public float _hpPotionCooldown = 10f;
    public float _mpPotionCooldown = 10f;
    public int _playerSTR = 5;
    public int _playerDEX = 5;
    public int _playerINT = 5;
    public int _playerLUK = 5;
    public int _playerStatPoint = 0;
    int _playerPower = 0;
    public int PlayerPower
    {
        get => _playerPower;
        set
        {
            _playerPower = value;
            OnValueChanged.Invoke();
        }
    }
    public int _playerMinPower = 0, _playerMaxPower = 0;
    public int _playerLv = 1;
    public int _totalExp = 50;
    int _playerExp = 0;
    public int CurrentExp
    {
        get => _playerExp;
        set
        {
            _playerExp = value;
            if (CurrentExp >= _totalExp)
                LevelUp();
            OnValueChanged.Invoke();
        }
    }

    [Header("Sounds Name")]
    public AudioClip[] _walkSound;
    public AudioClip[] _attackSound;
    public AudioClip _hitSound;
    public AudioClip _dieSound;
    public AudioClip _levelUpSound;

    int _currentHP, _currentMP;
    public int CurrentHP
    {
        get => _currentHP;
        set
        {
            _currentHP = value;
            if (_currentHP > _playerHP)
                _currentHP = _playerHP;
            else if (_currentHP < 0)
                _currentHP = 0;
            OnValueChanged.Invoke();
        }
    }
    public int CurrentMP
    {
        get => _currentMP;
        set
        {
            _currentMP = value;
            if (_currentMP > _playerMP)
                _currentMP = _playerMP;
            else if (_currentMP < 0)
                _currentMP = 0;
            OnValueChanged.Invoke();
        }
    }

    ParticleSystem _levelUpParticle;

    int _addedSTR = 0;
    int _addedDEX = 0;
    int _addedINT = 0;
    int _addedLUK = 0;
    int _addedPower = 0;

    private void Awake()
    {
        gameObject.transform.Find("Player_LevelUp").gameObject.TryGetComponent<ParticleSystem>(out _levelUpParticle);
    }

    private void Start()
    {
        _currentHP = _playerHP;
        _currentMP = _playerMP;
        SetPower();

        if(_levelUpSound == null)
            _levelUpSound = Resources.Load("Sounds/SFX/Player/SFX_Player_LevelUp") as AudioClip;
        if (_hitSound == null)
            _hitSound = Resources.Load("Sounds/SFX/Player/SFX_Player_Hit") as AudioClip;
        if (_dieSound == null)
            _dieSound = Resources.Load("Sounds/SFX/Player/SFX_Player_Die") as AudioClip;
    }

    public void SetInit()
    {
        CurrentHP = PlayerHP;
        CurrentMP = PlayerMP;
        SetPower();
    }

    public void SetPower()
    {
        PlayerPower = (int)((_playerSTR + _addedSTR) * 2 + (_playerDEX + _addedDEX) * 1 + (_playerLUK + _addedLUK) * 0.5 + (_playerINT + _addedINT) * 0.1);
        _playerMinPower = (int)((_playerPower + _addedPower) * 0.8);
        _playerMaxPower = (int)((_playerPower + _addedPower) * 1.1);
    }

    public void BuffPower(float value)
    {
        _addedSTR = (int)(_playerSTR * value);
        _addedDEX = (int)(_playerDEX * value);
        _addedLUK = (int)(_playerLUK * value);
        _addedINT = (int)(_playerINT * value);
        SetPower();
    }

    public void LevelUp()
    {
        _playerLv++;
        _playerStatPoint += 2;
        PlayerHP += 10;
        PlayerMP += 5;
        CurrentHP = PlayerHP;
        CurrentMP = PlayerMP;
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
        SoundManager.instance.SFXPlay(_levelUpSound);
        if (!_levelUpParticle.gameObject.activeSelf)
            _levelUpParticle.gameObject.SetActive(true);
        _levelUpParticle.Play();
        UIController.instance.NoticeArea.GetMessage("·¹º§ ¾÷!");
    }
}