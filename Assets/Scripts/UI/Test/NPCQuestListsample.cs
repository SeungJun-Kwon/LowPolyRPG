using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCQuestListsample : MonoBehaviour
{
    [SerializeField] GameObject _quest;

    GameObject _contentBox;

    List<Questsample> _quests;

    private void Awake()
    {
        RectTransform[] allChildren = GetComponentsInChildren<RectTransform>();
        foreach (RectTransform child in allChildren)
        {
            if(child.name == "Content")
            {
                _contentBox = child.gameObject;
                break;
            }
        }
    }

    public void SetQuest(List<Questsample> quests)
    {
        _quests = quests;
        SortQuest();
        foreach (Questsample quest in _quests)
            Debug.Log(quest._title);
    }

    void SortQuest()
    {
        // 정렬 순위
        // 1. 진행 중 퀘스트
        // 2. 시작 가능 퀘스트
        // 3. 시작 불가 퀘스트
        PlayerManager playerManager = PlayerController.instance.PlayerManager;

        _quests.Sort(delegate (Questsample a, Questsample b)
        {
            if (playerManager.FindQuestsample(a) && playerManager.FindQuestsample(b))
            {
                if (a._title.CompareTo(b._title) <= 0)
                    return -1;
                else
                    return 1;
            }
            else if (!playerManager.FindQuestsample(a) && !playerManager.FindQuestsample(b))
            {
                if (a._title.CompareTo(b._title) <= 0)
                    return -1;
                else
                    return 1;
            }
            else if (playerManager.FindQuestsample(a))
                return -1;
            else
                return 1;
        });
    }
}
