using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAI : MonoBehaviour
{
    [SerializeField] protected NPC _npc;
    [SerializeField] GameObject _actionUI;

    protected GameObject _canvas;

    KeyCode _action;

    bool _isAroundPlayer;

    protected virtual void Awake()
    {
        _canvas = GameObject.Find("UI");
        _actionUI = _canvas.transform.Find("DoConversation").gameObject;
    }

    private void Start()
    {
        _action = PlayerController.instance.PlayerKeySetting._action;
    }

    private void Update()
    {
        if(_isAroundPlayer)
        {
            if (Input.GetKeyDown(_action))
            {
                _isAroundPlayer = false;
                _actionUI.SetActive(_isAroundPlayer);
                Action();
            }
        }
    }

    protected virtual void Action()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            _isAroundPlayer = true;
            _actionUI.SetActive(_isAroundPlayer);
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
