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
            if(_isPlayerInRange)
                StartCoroutine(Attack(_attackName[0]));
            else
            {

            }
        }

        Debug.DrawRay(_punchPosition.position, _punchPosition.forward, Color.red);
    }

    IEnumerator Attack(string attackName)
    {
        _state = State.ATTACK;
        _navMesh.isStopped = true;
        _animator.SetFloat("Speed", 0);
        transform.LookAt(_target);
        _animator.SetTrigger(attackName);

        yield return new WaitForSeconds(_bossAttackDelay);

        _state = State.IDLE;
        _navMesh.isStopped = false;
    }

    void StrongPunch()
    {
        var hits = Physics.SphereCastAll(_punchPosition.position, 1.5f, _punchPosition.forward, _bossRange, LayerMask.GetMask("Player"));
        if (hits.Length > 0)
        {
            PlayerController.instance.Damaged(_bossDamage);
        }
    }

    void ThrowingRock()
    {

    }
}
