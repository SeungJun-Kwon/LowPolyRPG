using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueNPCAI : NPCAI
{
    GameObject _dialogueUI;

    protected override void Awake()
    {
        base.Awake();
        _dialogueUI = _canvas.transform.Find("Dialogue").gameObject;
    }

    protected override void Action()
    {
        base.Action();
        _dialogueUI.GetComponent<Dialogue>().SetNPC((DialogueNPC)_npc, this);
        _dialogueUI.SetActive(true);
    }

    public void End()
    {
        if (_npc._isTimeline)
            TimelineController.instance.PlayFromTimeline(_npc._playableDirector, _npc._timeline);
    }
}
