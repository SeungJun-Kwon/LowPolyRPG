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
    [SerializeField] Sound[] _bgmSounds;
    [SerializeField] AudioSource _bgmPlayer;

    [Header("SFX")]
    [SerializeField] Sound[] _sfxSounds;
    [SerializeField] List<AudioSource> _sfxPlayer = new List<AudioSource>();

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
        _bgmPlayer.clip = _bgmSounds[0]._audioClip;
        BGMPlay();
    }

    public void BGMPlay()
    {
        _bgmPlayer.Play();
    }

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
                audioSource.clip = _sfxSounds[i]._audioClip;
                _sfxPlayer.Add(audioSource);
                _sfxPlayer[_sfxPlayer.Count - 1].Play();
                return;
            }
        }
        Debug.Log("NOT FOUND " + sfxName);
    }

    public void SFXPlay(AudioClip audioClip)
    {
        for (int i = 0; i < _sfxPlayer.Count; i++)
        {
            if (!_sfxPlayer[i].isPlaying)
            {
                _sfxPlayer[i].clip = audioClip;
                _sfxPlayer[i].Play();
                return;
            }
        }
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        _sfxPlayer.Add(audioSource);
        _sfxPlayer[_sfxPlayer.Count - 1].Play();
    }
}
