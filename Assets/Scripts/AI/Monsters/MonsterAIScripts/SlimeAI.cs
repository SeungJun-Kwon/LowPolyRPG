using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAI : MonsterAI
{
    protected override void Update()
    {
        base.Update();
    }

    private void TryAttack()
    {
        Transform _attackPosition = transform.Find("Center");
        RaycastHit[] hits = Physics.SphereCastAll(_attackPosition.position, _monsterRange, _attackPosition.up, 0f, LayerMask.GetMask("Player"));
        if (hits.Length > 0)
            PlayerController.instance.Damaged(_monsterDamage);
    }
}
