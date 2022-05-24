using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHpBar : MonoBehaviour
{
    private Camera _uiCamera;
    private Canvas _canvas;
    private RectTransform _rectParent;
    private RectTransform _rectHp;

    [HideInInspector] public Vector3 _offset = Vector3.zero;
    [HideInInspector] public Transform _targetTransform;

    private void Start()
    {
        _canvas = GetComponentInParent<Canvas>();
        _uiCamera = _canvas.worldCamera;
        _rectParent = _canvas.GetComponent<RectTransform>();
        _rectHp = GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        // 3D ÁÂÇ¥¸¦ ½ºÅ©¸°(2D) ÁÂÇ¥·Î º¯°æ
        var _screenPos = Camera.main.WorldToScreenPoint(_targetTransform.position + _offset);

        //if (_screenPos.z < 0.0f)
        //    _rectHp.gameObject.SetActive(false);
        //else
        //    _rectHp.gameObject.SetActive(true);

        var _localPos = Vector2.zero;

        // ½ºÅ©¸°(2D) ÁÂÇ¥¸¦ UI Canvas ÁÂÇ¥·Î º¯°æ
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectParent, _screenPos, _uiCamera, out _localPos);

        _rectHp.localPosition = _localPos;
    }
}
