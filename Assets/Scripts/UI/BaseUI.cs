using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    protected RectTransform _rectTransform;

    protected virtual void Awake()
    {
        TryGetComponent<RectTransform>(out _rectTransform);
    }

    protected virtual void OnEnable()
    {
        _rectTransform.SetAsLastSibling();
    }

    protected void SetPlayerMoveState(bool trigger = true)
    {
        if (trigger)
            PlayerController.instance.SetMyState(State.CANMOVE);
        else
            PlayerController.instance.SetMyState(State.CANTMOVE);
    }

    protected virtual void OnDisable()
    {
        SetPlayerMoveState();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _rectTransform.SetAsLastSibling();
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        SetPlayerMoveState(false);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        SetPlayerMoveState();
    }
}