using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDeathUI : MonoBehaviour
{
    Image _death;
    Button _restartButton;
    Button _endButton;
    RectTransform _rectTransform;
    CanvasGroup _canvasGroup;

    private void Awake()
    {
        transform.Find("DeathImage").gameObject.TryGetComponent<Image>(out _death);
        transform.Find("RestartButton").gameObject.TryGetComponent<Button>(out _restartButton);
        transform.Find("EndButton").gameObject.TryGetComponent<Button>(out _endButton);
        TryGetComponent<RectTransform>(out _rectTransform);
        TryGetComponent<CanvasGroup>(out _canvasGroup);
    }

    private void Start()
    {
        _restartButton.onClick.AddListener(OnClickRestartButton);
        _endButton.onClick.AddListener(OnClickEndButton);
    }

    private void OnEnable()
    {
        _rectTransform.SetAsLastSibling();
        _canvasGroup.alpha = 0;
        _restartButton.enabled = false;
        _endButton.enabled = false;
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float fadeCount = 0;
        while (fadeCount < 1f)
        {
            fadeCount += Time.deltaTime;
            yield return null;
            _canvasGroup.alpha = fadeCount;
        }

        fadeCount = 0;
        Color color;
        color = _restartButton.image.color;
        _restartButton.image.color = new Color(color.r, color.g, color.b, fadeCount);
        _endButton.image.color = new Color(color.r, color.g, color.b, fadeCount);
        _restartButton.gameObject.SetActive(true);
        _endButton.gameObject.SetActive(true);

        while (fadeCount < 1f)
        {
            fadeCount += Time.deltaTime;
            yield return null;
            color = _restartButton.image.color;
            _restartButton.image.color = new Color(color.r, color.g, color.b, fadeCount);
            _endButton.image.color = new Color(color.r, color.g, color.b, fadeCount);
        }

        _restartButton.enabled = true;
        _endButton.enabled = true;
    }

    IEnumerator FadeOut()
    {
        _restartButton.enabled = false;
        _endButton.enabled = false;

        float fadeCount = 1f;
        Color color;
        while (fadeCount > 0)
        {
            fadeCount -= Time.deltaTime;
            yield return null;
            color = _restartButton.image.color;
            _restartButton.image.color = new Color(color.r, color.g, color.b, fadeCount);
            _endButton.image.color = new Color(color.r, color.g, color.b, fadeCount);
        }
        _restartButton.gameObject.SetActive(false);
        _endButton.gameObject.SetActive(false);

        fadeCount = 1f;
        while (fadeCount > 0)
        {
            fadeCount -= Time.deltaTime;
            yield return null;
            _canvasGroup.alpha = fadeCount;
        }

        PlayerController.instance.Resurrection();
        gameObject.SetActive(false);
    }

    public void OnClickRestartButton()
    {
        StartCoroutine(FadeOut());
    }

    public void OnClickEndButton()
    {
        Application.Quit();
    }
}
