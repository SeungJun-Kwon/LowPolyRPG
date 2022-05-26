using UnityEngine;

[CreateAssetMenu(fileName = "DialogueNPC", menuName = "NPC/DialogueNPC")]
public class DialogueNPC : NPC
{
    public string[] _dialogue;

    private void Awake()
    {
        this._type = Type.DialogueNPC;
    }
}
