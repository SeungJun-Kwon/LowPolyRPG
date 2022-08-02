using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockGolemAI : BossAI
{
    Transform _punchPosition;

    protected override void Awake()
    {
        base.Awake();
        _punchPosition = transform.Find("StrongPunch");
    }

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

    void StrongPunch()
    {
        var hits = Physics.SphereCastAll(_punchPosition.position, 5f, _punchPosition.forward, LayerMask.GetMask("Player"));
        if (hits.Length > 0)
        {
            PlayerController.instance.Damaged(_bossDamage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
