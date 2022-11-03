using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCsampleAI : MonoBehaviour
{
    public NPCsample _npc;
    GameObject _actionUI;
    GameObject _dialogueUI;

    protected GameObject _canvas;

    KeyCode _action;

    [HideInInspector]
    public bool _isAction = false;

    bool _isAroundPlayer;

    protected virtual void Awake()
    {
        _canvas = GameObject.Find("UI");
        _actionUI = _canvas.transform.Find("DoConversation").gameObject;
        _dialogueUI = _canvas.transform.Find("NPCDialoguesample").gameObject;
    }

    private void Start()
    {
        _action = PlayerController.instance.PlayerKeySetting._action;

        StartCoroutine(UpdateCor());
    }

    IEnumerator UpdateCor()
    {
        while(true)
        {
            if (_isAroundPlayer && !_isAction)
            {
                if (Input.GetKey(_action))
                {
                    Action();
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }

    protected virtual void Action()
    {
        PlayerController.instance.SetMyState(State.CANTMOVE);
        _isAction = true;
        _actionUI.SetActive(false);
        _dialogueUI.SetActive(true);
        _dialogueUI.TryGetComponent<NPCDialoguesample>(out var npcDialoguesample);
        npcDialoguesample.SetNPC(_npc);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            _isAroundPlayer = true;
            _actionUI.SetActive(_isAroundPlayer);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (_isAction && !_dialogueUI.gameObject.activeSelf)
            {
                _isAction = false;
                _actionUI.SetActive(_isAroundPlayer);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            _isAroundPlayer = false;
            _actionUI.SetActive(_isAroundPlayer);
        }
    }
}
