using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueNPCAI : NPCAI
{
    [SerializeField] GameObject _dialogueUI;

    protected override void Awake()
    {
        base.Awake();
        _dialogueUI = _canvas.transform.Find("Dialogue").gameObject;
    }

    protected override void Action()
    {
        base.Action();
        _dialogueUI.GetComponent<Dialogue>().SetNPC((DialogueNPC)_npc);
        _dialogueUI.SetActive(true);
    }
}
