using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCDialoguesample : MonoBehaviour
{
    [SerializeField] Text _name;
    [SerializeField] Text _contentText;
    [SerializeField] Button _questButton, _dialogueButton, _exitButton;

    CanvasGroup _canvasGroup;
    NPCsample _npc;

    private void Awake()
    {
        TryGetComponent<CanvasGroup>(out _canvasGroup);
    }

    private void OnEnable()
    {
        StartCoroutine(FadeIn());
    }

    public void SetNPC(NPCsample npc)
    {
        _npc = npc;
        _name.text = _npc._name;
        _contentText.text = _npc._firstSentence;
        if (_npc._quests.Count > 0)
            _questButton.enabled = true;
        else
            _questButton.enabled = false;
    }

    public void ShowQuest()
    {

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
        _questButton.gameObject.SetActive(true);
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
