using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachinUI : BaseUI
{
    [SerializeField] float _fadeInSpeed = 1f;

    GameObject _upperBar, _lowerBar;
    CanvasGroup _canvasGroup;

    protected override void Awake()
    {
        base.Awake();
        _upperBar = transform.Find("UpperBar").gameObject;
        _lowerBar = transform.Find("LowerBar").gameObject;
        TryGetComponent(out _canvasGroup);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float count = 0f;
        _canvasGroup.alpha = 0;
        SetPlayerMoveState(false);

        while (count < _fadeInSpeed)
        {
            count += Time.deltaTime;
            _canvasGroup.alpha = count;

            yield return null;
        }
    }
}
