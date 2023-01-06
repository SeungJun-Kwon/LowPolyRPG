using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneMoveManager : MonoBehaviour
{
    public static SceneMoveManager instance;

    [SerializeField] CanvasGroup _canvasGroup;
    [SerializeField] Image _progressBar;
    [SerializeField] GameObject _background;

    SceneData _currentScene;
    AudioClip _portalSound;

    private void Awake()
    {
        #region Singtone
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(this);
            return;
        }
        else if (instance != this)
            Destroy(this.gameObject);
        #endregion
    }

    private void Start()
    {
        _portalSound = Resources.Load("Sounds/SFX/SFX_Portal") as AudioClip;
    }

    public void LoadScene(SceneData sceneData)
    {
        _background.SetActive(true);
        SceneManager.sceneLoaded += OnSceneLoaded;
        _currentScene = sceneData;
        StartCoroutine(LoadSceneProcess());
    }

    private IEnumerator LoadSceneProcess()
    {
        if(PlayerController.instance != null)
            PlayerController.instance.SetMyState(State.CANTMOVE);
        _progressBar.fillAmount = 0f;
        SoundManager.instance.SFXPlay(_portalSound);

        // 코루틴 내에서 다른 코루틴을 yield return 해주면 해당 코루틴이 끝날 때까지 기다린다
        yield return StartCoroutine(Fade(true));

        AsyncOperation op = SceneManager.LoadSceneAsync(_currentScene._name);
        op.allowSceneActivation = false; // 씬 로딩이 끝나도 자동으로 전환하지 않는다.

        float timer = 0f;
        while(!op.isDone)
        {
            yield return null;
            if(op.progress < 0.9f)
                _progressBar.fillAmount = op.progress;
            else
            {
                timer += Time.deltaTime;
                _progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer);
                if(_progressBar.fillAmount >= 1f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }

    IEnumerator Fade(bool isFadeIn)
    {
        float timer = 0f;
        while(timer <= 1f)
        {
            yield return null;
            timer += Time.deltaTime;
            _canvasGroup.alpha = isFadeIn ? Mathf.Lerp(0f, 1f, timer) : Mathf.Lerp(1f, 0f, timer);
        }

        if (!isFadeIn)
            _background.SetActive(false);
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if(arg0.name == _currentScene._name)
        {
            StartCoroutine(Fade(false));
            PlayerController.instance.ToTheStartPoint();
            SoundManager.instance.BGMPlay(_currentScene._bgm);
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
