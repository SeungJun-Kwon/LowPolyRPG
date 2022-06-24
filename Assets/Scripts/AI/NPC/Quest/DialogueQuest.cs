using UnityEngine;

[CreateAssetMenu(fileName = "DialogueQuest", menuName = "Quest/DialogueQuest")]
public class DialogueQuest : Quest
{
    public NPC[] _targetNPC;
    public bool[] _finishDialogue;

    private void Awake()
    {
        this._type = Type.DIALOGUE;
    }
}
