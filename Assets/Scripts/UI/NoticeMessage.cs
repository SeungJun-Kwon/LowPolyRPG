using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoticeMessage : MonoBehaviour
{
    [SerializeField] Text _messageText;
    [SerializeField] float _destroyTime;

    RectTransform _rectTransform;
    Color _originColor;

    private void Awake()
    {
        _originColor = _messageText.color;
    }

    public void SetText(string content) => _messageText.text = content;

    private void OnEnable()
    {
        _messageText.color = _originColor;
        StartCoroutine(Action());
    }

    IEnumerator Action()
    {
        for (int i = (int)_destroyTime * 10; i >= 0; i--)
        {
            float f = i / 10.0f;
            Color color = _messageText.color;
            color.a = f;
            _messageText.color = color;
            yield return new WaitForSeconds(0.1f);
        }

        gameObject.SetActive(false);
    }

    private void OnDisable() => UIController.instance.NoticeArea.RemoveMessage(gameObject);
}
