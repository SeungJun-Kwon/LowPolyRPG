using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivineShock : MonoBehaviour
{
    [SerializeField] private Skill _skill;

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

    private void OnEnable()
    {
        _skillDamageMultiplier = _skill._skillDamageMultiplier;
        _skillRange = _skill._skillRange;
        _skillDuration = _skill._skillDuration;
        _skillNumberOfAttack = _skill._skillNumberOfAttack;
        transform.forward = PlayerController.instance.transform.forward;
        transform.position += transform.forward * _skillRange;
        StartCoroutine(Attack());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator Attack()
    {
        float _time = 0f;
        while(true)
        {
            _sphereCollider.enabled = true;
            _time += 0.1f;
            yield return new WaitForSeconds(0.1f);
            _sphereCollider.enabled = false;
            _time += 0.1f;
            yield return new WaitForSeconds(0.1f);
            if (_time >= _skillDuration)
            {
                _sphereCollider.enabled = false;
                _particleSystem.Stop();
                if(_particleSystem.isStopped)
                    gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.layer.ToString());
        if (other.gameObject.tag == "Enemy")
        {
            other.TryGetComponent<MonsterAI>(out var _monsterAI);
            PlayerManager _playerManager = PlayerController.instance.PlayerManager;
            int minDamage = (int)(_playerManager._playerMinPower * _skillDamageMultiplier);
            int maxDamage = (int)(_playerManager._playerMaxPower * _skillDamageMultiplier);
            _monsterAI.Damaged(minDamage, maxDamage, _skillNumberOfAttack, PlayerController.instance.transform);
        }
    }
}
