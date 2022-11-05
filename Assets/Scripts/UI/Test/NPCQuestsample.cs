using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCQuestsample : MonoBehaviour
{
    Text _name;

    Questsample _quest;

    private void Awake()
    {
        TryGetComponent<Text>(out _name);
    }

    public void SetQuest(Questsample quest)
    {
        _quest = quest;
        _name.text = _quest._title;
    }
}
