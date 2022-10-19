using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectInfoForMouse : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    string _name;
    int _level;

    public void SetInfo(Monster monster)
    {
        _name = monster._monsterName;
        _level = monster._monsterLevel;
    }

    public void SetInfo(NPC npc)
    {
        _name = npc._name;
        _level = 0;
    }

    private void OnMouseEnter()
    {
        if (TryGetComponent<MonsterAI>(out var monsterAI))
        {
            Monster monster = monsterAI._monster;
            SetInfo(monster);
        }
        else if (TryGetComponent<NPCAI>(out var npcAI))
        {
            NPC npc = npcAI._npc;
            SetInfo(npc);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (TryGetComponent<Monster>(out var monster))
            SetInfo(monster);
        else if (TryGetComponent<NPC>(out var npc))
            SetInfo(npc);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }
}
