using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum NPCDialogueState { INIT = 0, DIALOGUE, QUESTLIST, QUESTDETAIL, QUESTDIALOGUE };
public class NPCDialoguesample : MonoBehaviour
{
    [SerializeField] GameObject _questListBox, _questDetailBox;
    [SerializeField] Text _name;
    [SerializeField] Text _contentText;
    [SerializeField] Button _questButton, _questAcceptButton, _dialogueButton, _nextButton, _exitButton;

    [HideInInspector] public NPCQuestListsample _npcQuestList;
    [HideInInspector] public NPCQuestDetail _npcQuestDetail;
    [HideInInspector] public NPCsample _npc;
    [HideInInspector] public Questsample _selectedQuest;
    [HideInInspector] public List<string> _questDialogue;
    [HideInInspector] public int _questDialogueCount;

    CanvasGroup _canvasGroup;

    PlayerManager _playerManager;

    NPCDialogueState _state;

    private void Awake()
    {
        TryGetComponent<CanvasGroup>(out _canvasGroup);

        _questListBox.TryGetComponent<NPCQuestListsample>(out _npcQuestList);
        _questDetailBox.TryGetComponent<NPCQuestDetail>(out _npcQuestDetail);
    }

    private void OnEnable()
    {
        StartCoroutine(FadeIn());
    }

    public void SetNPC(NPCsample npc)
    {
        _npc = npc;
        _name.text = _npc._name;
        _state = NPCDialogueState.INIT;
        SetUI();
    }

    public void SetContentText(string s) => _contentText.text = s;

    public void NormalDialogue() => SetContentText(_npc._dialogues[Random.Range(0, _npc._dialogues.Count)]);

    public void QuestDialogue()
    {
        Debug.Log(_questDialogueCount);
        if (_questDialogueCount >= _questDialogue.Count)
        {
            Debug.LogFormat("{0} {1}", _questDialogueCount, _questDialogue.Count);
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
        _playerManager = PlayerController.instance.PlayerManager;

        switch (_state)
        {
            case NPCDialogueState.INIT:
                _questAcceptButton.gameObject.SetActive(false);
                if (_npc._quests.Count > 0)
                    _questButton.gameObject.SetActive(true);
                else
                    _questButton.gameObject.SetActive(false);
                _dialogueButton.gameObject.SetActive(true);
                _nextButton.gameObject.SetActive(false);
                _contentText.gameObject.SetActive(true);
                _questListBox.gameObject.SetActive(false);
                _questDetailBox.gameObject.SetActive(false);
                _contentText.text = _npc._firstSentence;
                break;
            case NPCDialogueState.DIALOGUE:
                _questAcceptButton.gameObject.SetActive(false);
                _questButton.gameObject.SetActive(false);
                _dialogueButton.gameObject.SetActive(false);
                _nextButton.gameObject.SetActive(false);
                _contentText.gameObject.SetActive(true);
                _questListBox.gameObject.SetActive(false);
                _questDetailBox.gameObject.SetActive(false);
                break;
            case NPCDialogueState.QUESTLIST:
                _questAcceptButton.gameObject.SetActive(false);
                _questButton.gameObject.SetActive(false);
                _dialogueButton.gameObject.SetActive(false);
                _nextButton.gameObject.SetActive(false);
                _contentText.gameObject.SetActive(false);
                _questListBox.gameObject.SetActive(true);
                _npcQuestList.SetQuest(_npc._quests);
                break;
            case NPCDialogueState.QUESTDETAIL:
                if(_playerManager.FindQuestsample(_selectedQuest))
                    _questAcceptButton.gameObject.SetActive(false);
                else
                    _questAcceptButton.gameObject.SetActive(true);
                _questButton.gameObject.SetActive(false);
                _dialogueButton.gameObject.SetActive(false);
                _nextButton.gameObject.SetActive(false);
                _contentText.gameObject.SetActive(false);
                _questListBox.gameObject.SetActive(false);
                _questDetailBox.gameObject.SetActive(true);
                _npcQuestDetail.SetQuest(_selectedQuest);
                break;
            case NPCDialogueState.QUESTDIALOGUE:
                _questAcceptButton.gameObject.SetActive(false);
                _questButton.gameObject.SetActive(false);
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

    private void OnDisable()
    {
        PlayerController.instance.SetMyState(State.CANMOVE);
    }
}
