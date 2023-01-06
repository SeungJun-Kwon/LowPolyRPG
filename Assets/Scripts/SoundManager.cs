using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string _name;
    public AudioClip _audioClip;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;

    [Header("BGM")]
    [SerializeField] AudioClip _bgmClip;
    [SerializeField] AudioSource _bgmPlayer;

    [Header("SFX")]
    [SerializeField] Sound[] _sfxSounds;
    List<AudioSource> _sfxPlayer = new List<AudioSource>();

    float _curBGMVolume = 0.5f, _curSFXVolume = 0.5f;

    private void Awake()
    {
        if (!instance)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        BGMPlay(_bgmClip);
    }

    public void BGMPlay(AudioClip bgm)
    {
        _bgmClip = bgm;
        _bgmPlayer.clip = _bgmClip;
        _bgmPlayer.loop = true;
        _bgmPlayer.Play();
    }

    public void BGMStop() => _bgmPlayer.Stop();

    public void SFXPlay(string sfxName)
    {
        for(int i = 0; i < _sfxSounds.Length; i++)
        {
            if(_sfxSounds[i]._name == sfxName)
            {
                for(int j = 0; j < _sfxPlayer.Count; j++)
                {
                    if(!_sfxPlayer[j].isPlaying)
                    {
                        _sfxPlayer[j].clip = _sfxSounds[i]._audioClip;
                        _sfxPlayer[j].Play();
                        return;
                    }
                }
                AudioSource audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.volume = _curSFXVolume;
                audioSource.clip = _sfxSounds[i]._audioClip;
                _sfxPlayer.Add(audioSource);
                _sfxPlayer[_sfxPlayer.Count - 1].Play();
                return;
            }
        }
    }

    public void SFXPlay(AudioClip audioClip, bool loop = false)
    {
        if (audioClip == null)
            return;

        for (int i = 0; i < _sfxPlayer.Count; i++)
        {
            if (!_sfxPlayer[i].isPlaying)
            {
                _sfxPlayer[i].clip = audioClip;
                _sfxPlayer[i].Play();
                _sfxPlayer[i].loop = loop;
                return;
            }
        }
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = _curSFXVolume;
        audioSource.clip = audioClip;
        audioSource.loop = loop;
        _sfxPlayer.Add(audioSource);
        _sfxPlayer[_sfxPlayer.Count - 1].Play();
    }

    public void SFXStop()
    {
        foreach (var sfxPlayer in _sfxPlayer)
            sfxPlayer.Stop();
    }

    public void BGMVolume(float value)
    {
        _curBGMVolume = value;
        _bgmPlayer.volume = value;
    }

    public void SFXVolume(float value)
    {
        _curSFXVolume = value;
        foreach (AudioSource audioSource in _sfxPlayer)
        {
            audioSource.volume = value;
        }
    }
}
