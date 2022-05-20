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
        RaycastHit hit;
        if(Physics.Raycast(_attackPosition.position, _attackPosition.forward, out hit, _monsterRange))
        {
            if(hit.transform.root.tag == "Player")
            {
                hit.transform.TryGetComponent<PlayerController>(out var _thePC);
                _thePC.Damaged(_monsterDamage);
            }
        }
    }
}
