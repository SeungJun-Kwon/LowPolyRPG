using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingRock : MonoBehaviour
{
    [SerializeField] MonsterSkill _skill;

    AudioClip _rockDestroySound;
    Rigidbody _rigidBody;
    Vector3 _originPos;

    int _skillDamage;
    float _skillRange, _skillDuration, _skillDelay;
    bool _isPercentageAttack;

    private void Awake()
    {
        TryGetComponent<Rigidbody>(out _rigidBody);
    }

    private void Start()
    {
        _skillDamage = _skill._skillDamage;
        _skillRange = _skill._skillRange;
        _skillDuration = _skill._skillDuration;
        _skillDelay = _skill._skillDelay;
        _isPercentageAttack = _skill._isPercentageAttack;
        _rockDestroySound = _skill._skillHitSound;
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            CameraController.instance.CameraShake(0.5f);
            other.gameObject.TryGetComponent<PlayerController>(out var playerController);
            playerController.Damaged(_skillDamage, false, _isPercentageAttack);
            Destroy(this.gameObject);
        }   
    }

    private void OnDestroy()
    {
        SoundManager.instance.SFXPlay(_rockDestroySound);
    }
}
