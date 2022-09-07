using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestNPCAI : NPCAI
{
    [SerializeField] GameObject _questUI;

    protected override void Awake()
    {
        base.Awake();
        _questUI = _canvas.transform.Find("QuestInfo").gameObject;
    }

    protected override void Action()
    {
        base.Action();
        _questUI.GetComponent<QuestInfo>().SetQuest((QuestNPC)_npc);
        _questUI.SetActive(true);
    }
}
