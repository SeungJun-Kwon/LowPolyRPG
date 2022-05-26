using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class QuestInfo : MonoBehaviour
{
    [SerializeField] GameObject _content;
    [SerializeField] TextMeshProUGUI _detailText;
    [SerializeField] GameObject _questPrefab;

    List<GameObject> _questList = new List<GameObject>();
    QuestNPC _npc;
    Quest[] _quest;

    private void OnEnable()
    {
        _detailText.text = "";
        _quest = _npc._quest;

        Transform[] allChild = _content.GetComponentsInChildren<Transform>();
        foreach(Transform child in allChild)
            if (child.name != _content.transform.name)
                _questList.Add(child.gameObject);

        if (_questList.Count < _quest.Length)
        {
            int cnt = _quest.Length - _questList.Count;
            for (int i = 0; i < cnt; i++)
            {
                var quest = Instantiate<GameObject>(_questPrefab, _content.transform);
                _questList.Add(quest);
            }
        }

        else if (_questList.Count > _quest.Length)
        {
            int cnt = _questList.Count - _quest.Length;
            for (int i = 0; i < cnt; i++)
            {
                Destroy(_content.GetComponentInChildren<GameObject>());
                _questList.RemoveAt(0);
            }
        }
        for (int i = 0; i < _questList.Count; i++)
            _questList[i].GetComponent<TextMeshProUGUI>().text = _quest[i]._title;
    }

    public void SetNPC(QuestNPC npc) => _npc = npc;

    public void GetQuest()
    {
        GameObject currentQuest = EventSystem.current.currentSelectedGameObject;
        int index = currentQuest.transform.GetSiblingIndex();
        _detailText.text = _quest[index]._desc;
    }
}
