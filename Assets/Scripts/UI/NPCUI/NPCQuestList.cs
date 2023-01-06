using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCQuestList : MonoBehaviour
{
    [SerializeField] GameObject _questPrefab;

    List<GameObject> _questPrefabList = new List<GameObject>();
    GameObject _contentBox;

    // 보여질 퀘스트 리스트. Value는 플레이어의 퀘스트 진행 상황
    // 0 : 완료 가능, 1 : 진행 중, 2 : 수락 가능, 3 : 수락 불가
    Dictionary<Quest, int> _questList = new Dictionary<Quest, int>();
    Dictionary<Quest, int> _sortedQuestList = new Dictionary<Quest, int>();

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

    public void SetQuest(List<Quest> quests)
    {
        _questList.Clear();
        QuestManager questManager = PlayerController.instance.QuestManager;
        List<Quest> completedQuest = questManager.GetCompletedQuest();

        foreach (Quest quest in quests)
        {
            if (completedQuest.Contains(quest))
                continue;

            if (!quest.CanAccept())
                _questList.Add(quest, 3);
            else if (!questManager.FindQuest(quest))
                _questList.Add(quest, 2);
            else if (questManager.FindQuest(quest))
            {
                QuestData playerQuest = questManager.GetQuest(quest);
                if (playerQuest.CanComplete)
                    _questList.Add(quest, 0);
                else
                    _questList.Add(quest, 1);
            }
        }

        _sortedQuestList = SortQuest(_questList);

        ShowQuestList();
    }

    void ShowQuestList()
    {
        if(_questPrefabList.Count == 0)
        {
            foreach (var quest in _sortedQuestList)
            {
                var questSlot = Instantiate(_questPrefab, _contentBox.transform);
                _questPrefabList.Add(questSlot);
                questSlot.TryGetComponent<NPCQuest>(out var npcQuest);
                npcQuest.SetQuest(quest.Key, quest.Value);
            }
        }
        else
        {
            if(_questPrefabList.Count >= _sortedQuestList.Count)
            {
                int index = 0;
                foreach (var quest in _sortedQuestList)
                {
                    _questPrefabList[index].SetActive(true);
                    _questPrefabList[index++].TryGetComponent<NPCQuest>(out var npcQuest);
                    npcQuest.SetQuest(quest.Key, quest.Value);
                }
                for (; index < _questPrefabList.Count; index++)
                    _questPrefabList[index].SetActive(false);
            }
            else
            {
                int index = 0;
                foreach (var quest in _sortedQuestList)
                {
                    if (index < _questPrefabList.Count)
                    {
                        _questPrefabList[index].SetActive(true);
                        _questPrefabList[index++].TryGetComponent<NPCQuest>(out var npcQuest);
                        npcQuest.SetQuest(quest.Key, quest.Value);
                    }
                    else
                    {
                        var questSlot = Instantiate(_questPrefab, _contentBox.transform);
                        _questPrefabList.Add(questSlot);
                        questSlot.TryGetComponent<NPCQuest>(out var npcQuest);
                        npcQuest.SetQuest(quest.Key, quest.Value);
                    }
                }
            }
        }
    }

    Dictionary<Quest, int> SortQuest(Dictionary<Quest, int> dics)
    {
        // 정렬 순위
        // 1. 진행 중 퀘스트
        // 2. 시작 가능 퀘스트
        // 3. 시작 불가 퀘스트
        Dictionary<Quest, int> result = new Dictionary<Quest, int>();
        List<KeyValuePair<Quest, int>> list = dics.ToList();
        list.Sort(delegate (KeyValuePair<Quest, int> x, KeyValuePair<Quest, int> y)
        {
            if (x.Value < y.Value)
                return -1;
            else
                return 1;
        });

        foreach (var item in list)
            result.Add(item.Key, item.Value);

        return result;
    }
}
