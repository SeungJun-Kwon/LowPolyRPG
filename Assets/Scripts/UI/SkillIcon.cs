using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image _skillImage, _skillFill;

    PlayerSkill _skill;
    SkillSlot _skillSlot;

    float _currentCoolTime;
    float _skillInfoYPos = 5f;

    private void Awake()
    {
        _skillSlot = GetComponentInParent<SkillSlot>();
    }

    public void SetSkill(PlayerSkill skill)
    {
        _skill = skill;
        _skillImage.sprite = _skill._skillIconImage;
        _skillFill.sprite = _skill._skillIconImage;
    }

    public void UseSkill()
    {
        _currentCoolTime = 0f;
        StartCoroutine(StartCoolDown());
    }

    IEnumerator StartCoolDown()
    {
        while(_currentCoolTime < _skill._skillCoolTime)
        {
            _skillFill.fillAmount = (_skill._skillCoolTime - _currentCoolTime) / _skill._skillCoolTime;
            _currentCoolTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        yield break;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Vector3 position = new Vector3(transform.position.x, transform.position.y + _skillInfoYPos, transform.position.z);
        _skillSlot._skillInfo.transform.position = position;
        _skillSlot._skillInfo.gameObject.SetActive(true);
        _skillSlot._skillInfo.GetSkill(_skill);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _skillSlot._skillInfo.gameObject.SetActive(false);
    }
}
