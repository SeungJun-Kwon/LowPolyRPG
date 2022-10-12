using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dialogue : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _name;
    [SerializeField] TextMeshProUGUI _dialogue;

    DialogueNPC _npc;
    DialogueNPCAI _npcAI;
    int _cnt;

    private void OnEnable()
    {
        _cnt = 0;
        _name.text = _npc._name;
        _dialogue.text = _npc._dialogue[_cnt];
    }

    private void OnDisable()
    {
        PlayerController.instance.SetMyState(State.CANMOVE);
        _npcAI.End();
    }

    private void Update()
    {
        if(Input.GetKeyDown(PlayerController.instance.PlayerKeySetting._action))
        {
            _cnt++;
            if(_cnt >= _npc._dialogue.Length)
                gameObject.SetActive(false);
            else
                _dialogue.text = _npc._dialogue[_cnt];
        }
        else if(Input.GetKeyDown(PlayerController.instance.PlayerKeySetting._esc))
        {
            gameObject.SetActive(false);
        }
    }

    public void SetNPC(DialogueNPC npc, DialogueNPCAI npcAI)
    {
        _npc = npc;
        _npcAI = npcAI;
    }
}
