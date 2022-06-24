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
        int index = 0;
        PlayerManager playerManager = PlayerController.instance.PlayerManager;
        List<Quest> currentQuests, completedQuests;
        currentQuests = playerManager.GetCurrentQuests();
        completedQuests = playerManager.GetCompletedQuests();
        if (completedQuests.Contains(_quest))
            index = 2;
        else if (currentQuests.Contains(_quest))
            index = 1;
        else
            index = 0;

        UIController.instance.QuestInfo.UpdateDetail(_quest, index);
    }
}
