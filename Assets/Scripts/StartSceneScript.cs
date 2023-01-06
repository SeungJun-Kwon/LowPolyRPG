using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartSceneScript : MonoBehaviour
{
    [SerializeField] SceneData _startScene;
    [SerializeField] SceneData _nextScene;
    [SerializeField] CustomButton _startButton, _manualButton, _exitButton;
    [SerializeField] Image _background;
    [SerializeField] Animator _animator;

    private void Start()
    {
        _startButton.interactable = true;
        _manualButton.interactable = true;
        _exitButton.interactable = true;
        SoundManager.instance.BGMPlay(_startScene._bgm);
    }

    public void StartGame()
    {
        StartCoroutine(StartGameCor());
    }

    public void Manual()
    {

    }

    public void Exit()
    {
        StartCoroutine(Fadeout());
    }

    IEnumerator StartGameCor()
    {
        _startButton.interactable = false;
        _manualButton.interactable = false;
        _exitButton.interactable = false;
        _animator.SetTrigger("Start");
        yield return new WaitForSeconds(1.5f);
        SceneMoveManager.instance.LoadScene(_nextScene);
    }

    IEnumerator Fadeout()
    {
        float count = 0f;

        while(count < 1f)
        {
            count += Time.deltaTime;
            yield return null;

            _background.color = new Color(_background.color.r, _background.color.g, _background.color.b, count);
        }

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
