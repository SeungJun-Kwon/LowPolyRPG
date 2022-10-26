using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockGolemAI : BossAI
{
    [SerializeField] Transform _rockPosition;

    private void OnEnable()
    {
        UIController.instance.SetActiveBossHPBar(_bossMonster, true);
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
        if(_state != State.ATTACK)
        {
            if (_isPlayerInRange)
            {
                StartCoroutine(PlayAttackAnim("NormalAttack"));
            }
            else
            {
                StartCoroutine(PlayAttackAnim(_bossSkill[0]._skillTrigger));
            }
        }
    }



    void StrongPunch()
    {
        var hits = Physics.BoxCastAll(_boxCollider.bounds.center, _boxCollider.bounds.size / 2, transform.forward, transform.rotation, _bossRange, LayerMask.GetMask("Player"));
        if (hits.Length > 0)
        {
            PlayerController.instance.Damaged(_bossDamage, false);
        }
    }

    void ThrowingRock()
    {
        Instantiate(_bossSkillPrefab[0], _rockPosition.position, _bossSkillPrefab[0].transform.rotation);
    }
}
