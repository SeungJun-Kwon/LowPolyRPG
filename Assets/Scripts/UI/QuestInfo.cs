using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class QuestInfo : MonoBehaviour
{
    [SerializeField] GameObject _content;
    [SerializeField] GameObject _detail;
    [SerializeField] GameObject _acceptButton, _completeButton, _cancelButton;
    [SerializeField] QuestSlot _questPrefab;

    public TextMeshProUGUI _detailText;
    public TextMeshProUGUI _expText;
    public TextMeshProUGUI _goldText;

    List<Quest> _quest;
    Quest _selectedQuest;

    // 0 : 퀘스트 수락 전, 1 : 퀘스트 진행 중, 2 : 퀘스트 완료
    int _confirmState = 0;

    private void OnEnable()
    {
        _detailText.text = "";

        List<QuestSlot> questList = new List<QuestSlot>();
        foreach (QuestSlot child in _content.GetComponentsInChildren<QuestSlot>())
            questList.Add(child);

        if (_quest != null)
        {
            if (questList.Count < _quest.Count)
            {
                int cnt = _quest.Count - questList.Count;
                for (int i = 0; i < cnt; i++)
                {
                    var questObject = Instantiate(_questPrefab, _content.transform);
                    questObject.gameObject.SetActive(false);
                    questList.Add(questObject);
                }
            }
            else if (questList.Count > _quest.Count)
            {
                for (int i = _quest.Count; i < questList.Count; i++)
                {
                    questList[i].gameObject.SetActive(false);
                }
            }

            if (_quest.Count > 0)
                for (int i = 0; i < questList.Count; i++)
                {
                    if (PlayerController.instance.PlayerManager._playerLv >= _quest[i]._requiredLevel)
                    {
                        questList[i].SetQuest(_quest[i]);
                        questList[i].SetTitle();
                        questList[i].gameObject.SetActive(true);
                    }
                }
        }

        _detail.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(PlayerController.instance.PlayerKeySetting._esc))
        {
            gameObject.SetActive(false);
        }
    }

    public void SetQuest(QuestNPC npc)
    {
        _quest = npc._quest;
    }

    public void SetQuest(List<Quest> quest)
    {
        _quest = quest;
    }

    public void AcceptQuest()
    {
        _acceptButton.SetActive(false);
        PlayerController.instance.PlayerManager.AddQuest(_selectedQuest);
    }

    public void CompleteQuest()
    {
        if (_selectedQuest._canComplete)
        {
            _completeButton.SetActive(false);
            Reward reward = _selectedQuest._reward;
            PlayerManager playerManager = PlayerController.instance.PlayerManager;
            playerManager.GainExp(reward._exp);
            playerManager.CompleteQuest(_selectedQuest);
        }
    }

    public void UpdateDetail(Quest currentQuest, int questState)
    {
        _selectedQuest = currentQuest;
        _detailText.text = _selectedQuest._desc;
        _expText.text = _selectedQuest._reward._exp.ToString();
        _goldText.text = _selectedQuest._reward._gold.ToString();
        switch(questState)
        {
            case 0:
                _acceptButton.SetActive(true);
                _completeButton.SetActive(false);
                break;
            case 1:
                _acceptButton.SetActive(false);
                _completeButton.SetActive(true);
                break;
            case 2:
                _acceptButton.SetActive(false);
                _completeButton.SetActive(false);
                break;
        }
        _detail.SetActive(true);
    }
}
