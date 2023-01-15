using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class NPCAI : MonoBehaviour
{
    [SerializeField] TextMeshPro _textMeshPro;

    public NPC _npc;
    GameObject _actionUI;
    GameObject _dialogueUI;
    BoxCollider _boxCollider;
    QuestManager _questManager;

    KeyCode _action;

    [HideInInspector]
    public bool _isAction = false;

    bool _isAroundPlayer;

    // NPC와 플레이어의 퀘스트 관계. 0이면 없음, 1이면 수락 가능, 2면 진행 중, 3이면 완료 가능
    // 우선순위 : 3 > 1 > 2 > 0
    int _questState = 0;
    public int QuestState
    {
        get
        {
            return _questState;
        }
        set
        {
            _questState = value;
            if (QuestState == 3)
            {
                _textMeshPro.text = "?";
                _textMeshPro.color = Color.yellow;
            }
            else if (QuestState == 1)
            {
                _textMeshPro.text = "!";
                _textMeshPro.color = Color.yellow;
            }
            else if (QuestState == 2)
            {
                _textMeshPro.text = "?";
                _textMeshPro.color = Color.gray;
            }
            else
                _textMeshPro.text = "";
        }
    }

    protected virtual void Awake()
    {
        TryGetComponent(out SphereCollider sphereCollider);
        if(sphereCollider == null)
        {
            sphereCollider = gameObject.AddComponent<SphereCollider>();
            sphereCollider.center = new Vector3(0, 1f, 0);
            sphereCollider.radius = 2f;
            sphereCollider.isTrigger = true;
        }

        TryGetComponent(out NavMeshObstacle navMeshObstacle);
        if(navMeshObstacle == null)
        {
            navMeshObstacle = gameObject.AddComponent<NavMeshObstacle>();
            navMeshObstacle.center = new Vector3(0, 0.5f, 0);
            navMeshObstacle.size = Vector3.one;
        }

        TryGetComponent(out _boxCollider);
        if(_boxCollider == null)
        {
            _boxCollider = gameObject.AddComponent<BoxCollider>();
            _boxCollider.center = new Vector3(0, 1f, 0);
            _boxCollider.size = new Vector3(1f, 2f, 1f);
        }

        _textMeshPro = Instantiate<TextMeshPro>(_textMeshPro, transform);
    }

    private void Start()
    {
        _actionUI = UIController.instance.PlayUI.transform.Find("DoConversation").gameObject;
        _dialogueUI = UIController.instance.PlayUI.transform.Find("NPCDialogue").gameObject;
        _questManager = PlayerController.instance.QuestManager;
        _action = PlayerController.instance.PlayerKeySetting._action;

        StartCoroutine(UpdateCor());

        _textMeshPro.rectTransform.localPosition = new Vector3(0, _boxCollider.bounds.size.y + 1f, 0);
    }

    IEnumerator UpdateCor()
    {
        float count = 0f;

        while(true)
        {
            count += Time.deltaTime;
            if(count > 1f)
            {
                CheckQuestState();
                count = 0f;
            }

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

    public void CheckQuestState()
    {
        List<QuestData> playerCurrentQuests = _questManager.GetCurrentQuestData(_npc);
        QuestState = 0;

        foreach (var quest in playerCurrentQuests)
        {
            if (quest.CanComplete)
            {
                QuestState = 3;
                return;
            }
        }
        foreach (var quest in _npc._quests)
        {
            if (_questManager.CurrentQuestContain(quest))
            {
                if(quest._type != Quest.Type.DIALOGUE)
                    continue;

                DialogueQuestData dqd = _questManager.GetQuest(quest) as DialogueQuestData;
                if (dqd._currentIndex < dqd._quest._targetNPC.Length)
                {
                    if (_npc == dqd._quest._targetNPC[dqd._currentIndex])
                        QuestState = 3;
                    else
                        QuestState = 2;
                }
            }
            else if (!_questManager.CompletedQuestContain(quest) && quest.CanAccept())
            {
                QuestState = 1;
                return;
            }
        }
        if(playerCurrentQuests.Count > 0)
            if(QuestState == 0)
                QuestState = 2;
    }

    protected virtual void Action()
    {
        PlayerController.instance.SetMyState(State.CANTMOVE);
        _isAction = true;
        _actionUI.SetActive(false);
        _dialogueUI.SetActive(true);
        _dialogueUI.TryGetComponent<NPCDialogue>(out var npcDialogue);
        npcDialogue.SetNPC(_npc, this);
        CameraController.instance.StartCoroutine(CameraController.instance.LookNPC(transform, _boxCollider.bounds.size.y / 2f));
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
