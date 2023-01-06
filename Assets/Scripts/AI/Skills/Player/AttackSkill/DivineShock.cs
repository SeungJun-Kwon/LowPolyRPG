using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DivineShock : SkillScript
{
    SphereCollider _sphereCollider;
    ParticleSystem _particleSystem;

    private float _skillDamageMultiplier;
    private float _skillRange;
    private float _skillDuration;
    private int _skillNumberOfAttack;

    private void Awake()
    {
        TryGetComponent<SphereCollider>(out _sphereCollider);
        if (!_sphereCollider)
            _sphereCollider = GetComponent<SphereCollider>();
        TryGetComponent<ParticleSystem>(out _particleSystem);
        if (!_particleSystem)
            _particleSystem = GetComponent<ParticleSystem>();
    }

    public override void OnStart()
    {
        _skillDamageMultiplier = _skill._skillDamageMultiplier;
        _skillRange = _skill._skillRange;
        _skillDuration = _skill._skillDuration;
        _skillNumberOfAttack = _skill._skillNumberOfAttack;
        _skillKey = SkillManager.instance.GetSkillKey(_skill);
        transform.forward = PlayerController.instance.transform.forward;
        transform.position += transform.forward * _skillRange;
        StartCoroutine(Attack());
    }

    public override void OnEnd()
    {
        _sphereCollider.enabled = false;
        _particleSystem.Stop();
        PlayerController.instance.SetMyState(State.IDLE);
        PlayerController.instance.Animator.SetTrigger(_skill._skillTrigger);
    }

    private void Update()
    {
        if (Input.GetKeyDown(_skillKey))
            SkillManager.instance.Destroy(_skill);
    }

    IEnumerator Attack()
    {
        float time = 0f;
        while(time < _skillDuration)
        {
            _sphereCollider.enabled = true;
            time += 0.1f;
            yield return new WaitForSeconds(0.1f);
            _sphereCollider.enabled = false;
            time += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        SkillManager.instance.Destroy(_skill);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            PlayerManager _playerManager = PlayerController.instance.PlayerManager;
            int minDamage = (int)(_playerManager._playerMinPower * _skillDamageMultiplier);
            int maxDamage = (int)(_playerManager._playerMaxPower * _skillDamageMultiplier);

            if (other.gameObject.layer == LayerMask.NameToLayer("Monster"))
            {
                other.TryGetComponent<MonsterAI>(out var _monsterAI);
                _monsterAI.Damaged(minDamage, maxDamage, _skillNumberOfAttack, PlayerController.instance.transform);
            }
            else if(other.gameObject.layer == LayerMask.NameToLayer("Boss")) {
                other.TryGetComponent<BossAI>(out var _bossAI);
                _bossAI.Damaged(minDamage, maxDamage, _skillNumberOfAttack);
            }

            SoundManager.instance.SFXPlay(_skill._skillHitSound);
        }
    }
}
