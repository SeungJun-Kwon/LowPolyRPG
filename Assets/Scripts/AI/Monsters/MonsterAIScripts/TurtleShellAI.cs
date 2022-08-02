using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleShellAI : MonsterAI
{
    [Header("Attack")]
    [SerializeField] string _attackTrigger = "Attack";
    [SerializeField] string _spinTrigger = "SpinAttack";
    [SerializeField] float _spinCoolTime = 5f;

    float _currentSpinCoolTime = 0;

    protected override void Update()
    {
        base.Update();
        if (!_isChase && !_isAttack)
        {
            _isAttack = true;
            if(_currentSpinCoolTime <= 0f)
                StartCoroutine(Attack(_spinTrigger));
            else
                StartCoroutine(Attack(_attackTrigger));
        }
    }

    private void TryAttack()
    {
        RaycastHit[] hits = Physics.SphereCastAll(_attackPosition.position, _monsterRange, _attackPosition.up, 0f, LayerMask.GetMask("Player"));
        if (hits.Length > 0)
            PlayerController.instance.Damaged(_monsterDamage);
    }

    private void TrySpinAttack()
    {
        RaycastHit[] hits = Physics.SphereCastAll(_attackPosition.position, _monsterRange, _attackPosition.up, 0f, LayerMask.GetMask("Player"));
        if (hits.Length > 0)
            PlayerController.instance.Damaged(_monsterDamage * 2);
        StartCoroutine(SpinCoolDown());
    }

    IEnumerator Attack(string trigger)
    {
        transform.LookAt(_target);
        _animator.SetTrigger(trigger);

        yield return new WaitForSeconds(_monsterAttackDelay);

        _isChase = true;
        _isAttack = false;
        _navMesh.isStopped = false;
    }

    IEnumerator SpinCoolDown()
    {
        _currentSpinCoolTime = _spinCoolTime;

        yield return new WaitForSeconds(_spinCoolTime);

        _currentSpinCoolTime = 0;
    }
}
