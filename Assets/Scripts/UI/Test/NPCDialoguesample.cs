using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCDialoguesample : MonoBehaviour
{
    [SerializeField] GameObject _questListBox;
    [SerializeField] Text _name;
    [SerializeField] Text _contentText;
    [SerializeField] Button _questButton, _questAcceptButton, _dialogueButton, _exitButton;

    NPCQuestListsample _npcQuestList;
    CanvasGroup _canvasGroup;
    NPCsample _npc;

    private void Awake()
    {
        TryGetComponent<CanvasGroup>(out _canvasGroup);

        _questListBox.TryGetComponent<NPCQuestListsample>(out _npcQuestList);
    }

    private void OnEnable()
    {
        StartCoroutine(FadeIn());
    }

    public void SetNPC(NPCsample npc)
    {
        _npc = npc;
        _name.text = _npc._name;
        SetInit();
    }

    public void ShowQuest()
    {
        _contentText.gameObject.SetActive(false);
        _questListBox.gameObject.SetActive(true);
        _questButton.gameObject.SetActive(false);
        _dialogueButton.gameObject.SetActive(false);
        _npcQuestList.SetQuest(_npc._quests);
    }

    public void DoConversation()
    {
        _questButton.gameObject.SetActive(false);
        _dialogueButton.gameObject.SetActive(false);
        int range = _npc._dialogues.Count;
        _contentText.text = _npc._dialogues[Random.Range(0, range)];
    }

    public void Exit()
    {
        if (_questButton.gameObject.activeSelf && _dialogueButton.gameObject.activeSelf)
            gameObject.SetActive(false);
        else
            SetInit();
    }

    private void SetInit()
    {
        _contentText.text = _npc._firstSentence;
        if(_npc._quests.Count > 0)
            _questButton.gameObject.SetActive(true);
        else
            _questButton.gameObject.SetActive(false);
        _dialogueButton.gameObject.SetActive(true);
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
