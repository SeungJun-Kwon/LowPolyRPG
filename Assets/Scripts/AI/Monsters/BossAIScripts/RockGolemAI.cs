using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockGolemAI : BossAI
{
    Collider coll;

    protected override void Update()
    {
        base.Update();

        if (_isPlayerInRange && _state != State.ATTACK)
            StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        _state = State.ATTACK;
        _navMesh.isStopped = true;
        _animator.SetFloat("Speed", 0);
        transform.LookAt(_target);
        _animator.SetTrigger("StrongPunch");

        yield return new WaitForSeconds(_bossAttackDelay);

        _state = State.IDLE;
        _navMesh.isStopped = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
