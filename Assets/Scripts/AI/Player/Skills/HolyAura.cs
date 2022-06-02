using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyAura : MonoBehaviour
{
    [SerializeField] private Skill _skill;

    ParticleSystem _particleSystem;

    private float _skillDamageMultiplier;
    private float _skillDuration;

    private void Awake()
    {
        TryGetComponent<ParticleSystem>(out _particleSystem);
        if (!_particleSystem)
            _particleSystem = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        _skillDamageMultiplier = _skill._skillDamageMultiplier;
        _skillDuration = _skill._skillDuration;
        StartCoroutine(Buff());
    }

    IEnumerator Buff()
    {
        PlayerController.instance.PlayerManager.BuffPower(_skillDamageMultiplier);
        UIController.instance.PlayerInfo.UpdatePlayerStatus();

        yield return new WaitForSeconds(_skillDuration);

        PlayerController.instance.PlayerManager.BuffPower(0);
        UIController.instance.PlayerInfo.UpdatePlayerStatus();
        _particleSystem.Stop();
        if (_particleSystem.isStopped)
            gameObject.SetActive(false);
    }
}
