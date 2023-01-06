using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockGolemAI : BossAI
{
    [Header("Rock Golem")]
    [SerializeField] GameObject _unityChan;
    [SerializeField] Transform _rockPosition;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        _onDie.AddListener(UnityChanActive);
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
                RaycastHit[] hits = Physics.SphereCastAll(transform.position, _bossSkill[0]._skillRange, transform.up, 1f, LayerMask.GetMask("Player"), QueryTriggerInteraction.Collide);
                if(hits.Length > 0)
                    StartCoroutine(PlayAttackAnim(_bossSkill[0]._skillTrigger));
            }
        }
    }

    void UnityChanActive() => _unityChan.SetActive(true);

    void StrongPunch()
    {
        var hits = Physics.BoxCastAll(_boxCollider.bounds.center, _boxCollider.bounds.size / 2, transform.forward, transform.rotation, _bossRange, LayerMask.GetMask("Player"), QueryTriggerInteraction.Collide);
        if (hits.Length > 0)
        {
            PlayerController.instance.Damaged(_bossDamage, false);
            SoundManager.instance.SFXPlay(_bossMonster._attackSound);
            CameraController.instance.CameraShake(0.3f);
        }
    }

    void ThrowingRock()
    {
        Instantiate(_bossSkillPrefab[0], _rockPosition.position, _bossSkillPrefab[0].transform.rotation);
    }
}
