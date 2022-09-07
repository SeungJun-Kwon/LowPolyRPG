using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DamageText : MonoBehaviour
{
    //public Transform _startTransform;
    public Vector3 _offset = new Vector3(0, 1.5f, 0);
    public int _damage;

    [HideInInspector] public Transform _targetTransform;

    private Camera _uiCamera;
    private Canvas _canvas;
    private RectTransform _rectParent;
    private TextMeshProUGUI _text;

    private float _destroyTime = 2f;

    private void Awake()
    {
        _canvas = GameObject.Find("UI").GetComponent<Canvas>();
        _text = GetComponent<TextMeshProUGUI>();
        _rectParent = _canvas.GetComponent<RectTransform>();
    }
    private void OnEnable()
    {
        transform.SetParent(_canvas.transform, false);
    }

    private void Start()
    {
        _uiCamera = _canvas.worldCamera;
        _text.text = _damage.ToString();

        Destroy(gameObject, _destroyTime);
    }

    private void LateUpdate()
    {
        var _screenPos = Camera.main.WorldToScreenPoint(_targetTransform.position + _offset);

        if (_screenPos.z < 0.0f)
            gameObject.SetActive(false);
        else
            gameObject.SetActive(true);

        var _localPos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectParent, _screenPos, _uiCamera, out _localPos);
        transform.localPosition = _localPos;
    }
}
