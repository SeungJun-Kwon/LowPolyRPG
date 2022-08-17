using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingRock : MonoBehaviour
{
    [SerializeField] Skill _skill;
    [SerializeField] AudioClip _rockDestroySound;

    Rigidbody _rigidBody;
    Vector3 _originPos;

    float _skillNumberOfAttack = 0, _skillNumberOfEnemy = 0, _skillDamageMultiplier = 0, _skillRange = 0, _skillDuration = 0, _skillDelay = 0, _skillCoolTime = 0;

    private void Awake()
    {
        TryGetComponent<Rigidbody>(out _rigidBody);
    }

    private void Start()
    {
        _skillNumberOfAttack = _skill._skillNumberOfAttack;
        _skillNumberOfEnemy = _skill._skillNumberOfEnemy;
        _skillDamageMultiplier = _skill._skillDamageMultiplier;
        _skillRange = _skill._skillRange;
        _skillDuration = _skill._skillDuration;
        _skillDelay = _skill._skillDelay;
        _skillCoolTime = _skill._skillCoolTime;
    }

    private void OnEnable()
    {
        _originPos = transform.position;
        Vector3 dir = PlayerController.instance.transform.position - _originPos;
        _rigidBody.AddForce(dir * 100f, ForceMode.Force);
    }

    private void Update()
    {
        if ((transform.position - _originPos).magnitude > _skillRange)
            Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
            Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        SoundManager.instance.SFXPlay(_rockDestroySound);
    }
}
