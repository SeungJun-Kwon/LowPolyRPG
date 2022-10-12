using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineController : MonoBehaviour
{
    public static TimelineController instance;

    public PlayableDirector _playableDirector;
    public TimelineAsset _timeline;

    private void Awake()
    {
        #region Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
            Destroy(this.gameObject);
        #endregion
    }

    public void Play(PlayableDirector playableDirector)
    {
        _playableDirector = playableDirector;
        _playableDirector.Play();
    }

    public void PlayFromTimeline(PlayableDirector playableDirector, TimelineAsset timeline)
    {
        _playableDirector = playableDirector;
        _timeline = timeline;
        _playableDirector.Play(_timeline);
    }
}
