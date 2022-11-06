using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCQuestListsample : MonoBehaviour
{
    [SerializeField] GameObject _questPrefab;

    List<GameObject> _questPrefabList = new List<GameObject>();
    GameObject _contentBox;

    // ������ ����Ʈ ����Ʈ. Value�� �÷��̾��� ����Ʈ ���� ��Ȳ
    // 0 : ���� ��, 1 : ���� ����, 2 : ���� �Ұ�, 3 : �Ϸ� ����
    Dictionary<Questsample, int> _questList = new Dictionary<Questsample, int>();
    Dictionary<Questsample, int> _sortedQuestList = new Dictionary<Questsample, int>();

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
        _questList.Clear();
        PlayerManager playerManager = PlayerController.instance.PlayerManager;
        foreach (Questsample quest in quests)
        {
            if (!quest.CanAccept())
                _questList.Add(quest, 2);
            else if (!playerManager.FindQuestsample(quest))
                _questList.Add(quest, 1);
            else
                _questList.Add(quest, 0);
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
                questSlot.TryGetComponent<NPCQuestsample>(out var npcQuestsample);
                npcQuestsample.SetQuest(quest.Key, quest.Value);
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
                    _questPrefabList[index++].TryGetComponent<NPCQuestsample>(out var npcQuestsample);
                    npcQuestsample.SetQuest(quest.Key, quest.Value);
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
                        _questPrefabList[index++].TryGetComponent<NPCQuestsample>(out var npcQuestsample);
                        npcQuestsample.SetQuest(quest.Key, quest.Value);
                    }
                    else
                    {
                        var questSlot = Instantiate(_questPrefab, _contentBox.transform);
                        _questPrefabList.Add(questSlot);
                        questSlot.TryGetComponent<NPCQuestsample>(out var npcQuestsample);
                        npcQuestsample.SetQuest(quest.Key, quest.Value);
                    }
                }
            }
        }
    }

    Dictionary<Questsample, int> SortQuest(Dictionary<Questsample, int> dics)
    {
        // ���� ����
        // 1. ���� �� ����Ʈ
        // 2. ���� ���� ����Ʈ
        // 3. ���� �Ұ� ����Ʈ
        Dictionary<Questsample, int> result = new Dictionary<Questsample, int>();
        List<KeyValuePair<Questsample, int>> list = dics.ToList();
        list.Sort(delegate (KeyValuePair<Questsample, int> x, KeyValuePair<Questsample, int> y)
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
