using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class QuestSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] TextMeshProUGUI _title;

    Quest _quest;

    public void SetTitle()
    {
        _title.text = _quest._title;
    }

    public void SetQuest(Quest quest)
    {
        _quest = quest;
    }
    public Quest GetQuest()
    {
        return _quest;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        QuestInfo.instance.UpdateDetail(_quest);
    }
}
