using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkillScript : MonoBehaviour
{
    public PlayerSkill _skill;
    
    protected KeyCode _skillKey;

    private void OnEnable()
    {
        OnStart();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public virtual void OnStart()
    {

    }

    public virtual void OnEnd()
    {

    }
}
