using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum NPCDialogueState { INIT = 0, DIALOGUE, QUESTLIST, QUESTDETAIL, QUESTDIALOGUE };
public class NPCDialogue : BaseUI
{
    [SerializeField] GameObject _questListBox, _questDetailBox;
    [SerializeField] Text _name;
    [SerializeField] Text _contentText;
    [SerializeField] Button _questButton, _questAcceptButton, _questCompleteButton, _dialogueButton, _nextButton, _exitButton;

    [HideInInspector] public NPCQuestList _npcQuestList;
    [HideInInspector] public NPCQuestDetail _npcQuestDetail;
    [HideInInspector] public NPC _npc;
    [HideInInspector] public NPCAI _npcAI;
    [HideInInspector] public Quest _selectedQuest;
    [HideInInspector] public List<string> _questDialogue;
    [HideInInspector] public int _questDialogueCount;

    CanvasGroup _canvasGroup;

    QuestManager _questManager;

    NPCDialogueState _state;

    
    protected override void Awake()
    {
        base.Awake();
        TryGetComponent<CanvasGroup>(out _canvasGroup);

        _questListBox.TryGetComponent<NPCQuestList>(out _npcQuestList);
        _questDetailBox.TryGetComponent<NPCQuestDetail>(out _npcQuestDetail);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(FadeIn());
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        CameraController.instance._isAble = true;
    }

    public void SetNPC(NPC npc, NPCAI npcAI)
    {
        _npc = npc;
        _npcAI = npcAI;
        _name.text = _npc._name;
        _state = NPCDialogueState.INIT;
        SetUI();
    }

    public void SetContentText(string s) => _contentText.text = s;

    public void NormalDialogue() => SetContentText(_npc._dialogues[Random.Range(0, _npc._dialogues.Count)]);

    public void QuestDialogue()
    {
        if (_questDialogueCount >= _questDialogue.Count)
        {
            _questManager = PlayerController.instance.QuestManager;
            if (_questManager.FindQuest(_selectedQuest))
            {
                DialogueQuestData quest = (DialogueQuestData)_questManager.GetQuest(_selectedQuest);
                quest.IncreaseCurrentIndex();
            }
            else
            {
                _questManager.AcceptQuest(_selectedQuest);
            }
            SetState(0);
            _questDialogue = null;
            return;
        }
        SetContentText(_questDialogue[_questDialogueCount]);
        _questDialogueCount += 1;
    }

    public void SetQuestDialogue(List<string> s, int index = 0)
    {
        _questDialogue = s;
        _questDialogueCount = index;
        QuestDialogue();
    }

    public void SetState(int index)
    {
        if (_state == NPCDialogueState.INIT && (NPCDialogueState)index == NPCDialogueState.INIT) {
            gameObject.SetActive(false);
            return;
        }
        _state = (NPCDialogueState)index;
        SetUI();
    }

    private void SetUI()
    {
        _questManager = PlayerController.instance.QuestManager;

        switch (_state)
        {
            case NPCDialogueState.INIT:
                if (_npc._quests.Count > 0)
                    _questButton.gameObject.SetActive(true);
                else
                    _questButton.gameObject.SetActive(false);
                _questAcceptButton.gameObject.SetActive(false);
                _questCompleteButton.gameObject.SetActive(false);
                _dialogueButton.gameObject.SetActive(true);
                _nextButton.gameObject.SetActive(false);
                _contentText.gameObject.SetActive(true);
                _questListBox.gameObject.SetActive(false);
                _questDetailBox.gameObject.SetActive(false);
                _contentText.text = _npc._firstSentence;
                break;
            case NPCDialogueState.DIALOGUE:
                _questButton.gameObject.SetActive(false);
                _questAcceptButton.gameObject.SetActive(false);
                _questCompleteButton.gameObject.SetActive(false);
                _dialogueButton.gameObject.SetActive(false);
                _nextButton.gameObject.SetActive(false);
                _contentText.gameObject.SetActive(true);
                _questListBox.gameObject.SetActive(false);
                _questDetailBox.gameObject.SetActive(false);
                break;
            case NPCDialogueState.QUESTLIST:
                _questButton.gameObject.SetActive(false);
                _questAcceptButton.gameObject.SetActive(false);
                _questCompleteButton.gameObject.SetActive(false);
                _dialogueButton.gameObject.SetActive(false);
                _nextButton.gameObject.SetActive(false);
                _contentText.gameObject.SetActive(false);
                _questListBox.gameObject.SetActive(true);
                _npcQuestList.SetQuest(_npc._quests);
                break;
            case NPCDialogueState.QUESTDETAIL:
                _questButton.gameObject.SetActive(false);
                if (_questManager.FindQuest(_selectedQuest))
                {
                    _questAcceptButton.gameObject.SetActive(false);
                    QuestData questData = _questManager.GetQuest(_selectedQuest);
                    if (questData.CanComplete)
                        _questCompleteButton.gameObject.SetActive(true);
                    else
                        _questCompleteButton.gameObject.SetActive(false);
                }
                else
                {
                    _questAcceptButton.gameObject.SetActive(true);
                    _questCompleteButton.gameObject.SetActive(false);
                }
                _dialogueButton.gameObject.SetActive(false);
                _nextButton.gameObject.SetActive(false);
                _contentText.gameObject.SetActive(false);
                _questListBox.gameObject.SetActive(false);
                _questDetailBox.gameObject.SetActive(true);
                _npcQuestDetail.SetQuest(_selectedQuest);
                break;
            case NPCDialogueState.QUESTDIALOGUE:
                _questButton.gameObject.SetActive(false);
                _questAcceptButton.gameObject.SetActive(false);
                _dialogueButton.gameObject.SetActive(false);
                _nextButton.gameObject.SetActive(true);
                _contentText.gameObject.SetActive(true);
                _questListBox.gameObject.SetActive(false);
                _questDetailBox.gameObject.SetActive(false);
                break;
        }
    }

    IEnumerator FadeIn()
    {
        float fadeCount = 0;
        while(fadeCount < 1f)
        {
            fadeCount += Time.deltaTime;
            yield return null;
            _canvasGroup.alpha = fadeCount;
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public override void OnPointerExit(PointerEventData eventData)
    {

    }
}
