using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockGolemAI : BossAI
{
    [SerializeField] Transform _punchPosition;

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
                StartCoroutine(NormalAttack());
            }
            else
            {

            }
        }
    }

    void StrongPunch()
    {
        var hits = Physics.BoxCastAll(_boxCollider.bounds.center, _boxCollider.bounds.size / 2, transform.forward, transform.rotation, _bossRange, LayerMask.GetMask("Player"));
        if (hits.Length > 0)
        {
            PlayerController.instance.Damaged(_bossDamage);
        }
    }

    void ThrowingRock()
    {

    }
}
