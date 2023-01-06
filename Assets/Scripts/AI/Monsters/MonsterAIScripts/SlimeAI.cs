using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAI : MonsterAI
{
    protected override void Update()
    {
        base.Update();
        if (!_isChase && !_isAttack)
        {
            int index = Random.Range(0, 2);
            _isAttack = true;
            StartCoroutine(Attack(index));
        }
    }

    private void TryAttack()
    {
        RaycastHit[] hits = Physics.SphereCastAll(_attackPosition.position, _monsterRange, _attackPosition.up, 0f, LayerMask.GetMask("Player"), QueryTriggerInteraction.Collide);
        if (hits.Length > 0)
            PlayerController.instance.Damaged(_monsterDamage, true);
    }

    IEnumerator Attack(int index)
    {
        transform.LookAt(_target);
        _animator.SetTrigger("Attack");
        _animator.SetInteger("AttackIndex", index);
        yield return new WaitForSeconds(_monsterAttackDelay);

        _isChase = true;
        _isAttack = false;
        _navMesh.isStopped = false;
    }
}
