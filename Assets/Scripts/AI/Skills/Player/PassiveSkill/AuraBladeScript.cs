using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraBladeScript : MonoBehaviour
{
    [SerializeField] float _duration = 1f;

    [HideInInspector] public PlayerSkill _skill;

    BoxCollider _boxCollider;
    Vector3 _originScale;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
        _originScale = transform.localScale;
    }

    private void Start()
    {
    }

    private void OnEnable()
    {
        transform.forward = PlayerController.instance.transform.forward;
        StartCoroutine(Action());
    }

    IEnumerator Action()
    {
        float count = 0;
        transform.localScale = Vector3.one * 0.5f;

        while (count < _duration)
        {
            count += Time.deltaTime;

            if(transform.localScale.magnitude < _originScale.magnitude)
                transform.localScale *= 1.05f;
            transform.position += transform.forward * 0.2f;

            yield return new WaitForFixedUpdate();
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            PlayerManager playerManager = PlayerController.instance.PlayerManager;
            int minDmg = (int)(playerManager._playerMinPower * _skill._skillDamageMultiplier);
            int maxDmg = (int)(playerManager._playerMaxPower * _skill._skillDamageMultiplier);

            if(other.gameObject.layer == LayerMask.NameToLayer("Boss"))
            {
                other.gameObject.TryGetComponent<BossAI>(out var bossAI);
                bossAI.Damaged(minDmg, maxDmg, _skill._skillNumberOfAttack);
            }
            else if(other.gameObject.layer == LayerMask.NameToLayer("Monster"))
            {
                other.gameObject.TryGetComponent<MonsterAI>(out var monsterAI);
                monsterAI.Damaged(minDmg, maxDmg, _skill._skillNumberOfAttack);
            }

            SoundManager.instance.SFXPlay(_skill._skillHitSound);
        }
    }
}
