using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraBlade : SkillScript
{
    [Header("스킬 발동 확률")]
    [SerializeField] float _percentage = 20f;

    GameObject _slashObject;
    AuraBladeScript _script;

    private void Awake()
    {
        _slashObject = Resources.Load("Effects/GroundSlash") as GameObject;
        _slashObject.TryGetComponent(out _script);
    }

    private void Start()
    {
        PlayerController.instance._attackEvent.AddListener(Slash);
        _script._skill = _skill;
    }

    public void Slash()
    {
        if (!SkillManager.instance.IsReady(_skill))
            return;

        if(Random.Range(0, 101) <= _percentage)
        {
            Instantiate(_slashObject, PlayerController.instance.transform.position, Quaternion.identity);
            SkillManager.instance.Use(_skill);
        }
    }
}
