using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class QuestInfo : MonoBehaviour
{
    [SerializeField] GameObject _content;
    [SerializeField] QuestSlot _questPrefab;

    public TextMeshProUGUI _detailText;
    public TextMeshProUGUI _expText;
    public TextMeshProUGUI _goldText;

    QuestNPC _npc;

    private void OnEnable()
    {
        //_detailText.text = "";
        Quest[] quest = _npc._quest;

        List<QuestSlot> questList = new List<QuestSlot>();
        foreach(QuestSlot child in _content.GetComponentsInChildren<QuestSlot>())
            questList.Add(child);

        if (questList.Count < quest.Length)
        {
            int cnt = quest.Length - questList.Count;
            for (int i = 0; i < cnt; i++)
            {
                var questObject = Instantiate(_questPrefab, _content.transform);
                questObject.gameObject.SetActive(false);
                questList.Add(questObject);
            }
        }
        else if (questList.Count > quest.Length)
        {
            for(int i = quest.Length; i < questList.Count; i++)
            {
                questList[i].gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < questList.Count; i++)
        {
            questList[i].SetQuest(quest[i]);
            questList[i].SetTitle();
            questList[i].gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(PlayerKeySetting.instance._esc))
        {
            gameObject.SetActive(false);
        }
    }

    public void SetNPC(QuestNPC npc)
    {
        _npc = npc;
    }

    public void UpdateDetail(Quest currentQuest)
    {
        _detailText.text = currentQuest._desc;
        _expText.text = currentQuest._reward._exp.ToString();
        _goldText.text = currentQuest._reward._gold.ToString();
    }
}
