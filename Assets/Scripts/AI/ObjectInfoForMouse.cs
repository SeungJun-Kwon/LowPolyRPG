using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ObjectInfoForMouse : MonoBehaviour
{
    [SerializeField] GameObject _objectInfoUI;
    [SerializeField] Text _text;

    Camera _uiCamera;
    Canvas _canvas;
    RectTransform _rectParent;
    RectTransform _rect;
    string _name;
    int _level;

    public Vector3 _offset;
    [HideInInspector] public Transform _targetTransform;

    private void Awake()
    {
        _canvas = GetComponentInParent<Canvas>();
        _rectParent = _canvas.GetComponent<RectTransform>();
        _rect = GetComponent<RectTransform>();
    }

    private void Start()
    {
        _uiCamera = _canvas.worldCamera;

        _offset = new Vector3(0, 1f, 0);
    }

    private void LateUpdate()
    {
        // 3D ÁÂÇ¥¸¦ ½ºÅ©¸°(2D) ÁÂÇ¥·Î º¯°æ
        var _screenPos = Camera.main.WorldToScreenPoint(_targetTransform.position + _offset);

        var _localPos = Vector2.zero;

        // ½ºÅ©¸°(2D) ÁÂÇ¥¸¦ UI Canvas ÁÂÇ¥·Î º¯°æ
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectParent, _screenPos, _uiCamera, out _localPos);

        _rect.localPosition = _localPos;
    }

    public void SetInfo(Monster monster, Transform target)
    {
        _name = monster._monsterName;
        _level = monster._monsterLevel;
        _text.text = string.Format("Lv.{0} {1}", _level, _name);
        _targetTransform = target;
    }

    public void SetInfo(NPC npc, Transform target)
    {
        _name = npc._name;
        _level = 0;
        _text.text = npc._name;
        _targetTransform = target;
    }
}
