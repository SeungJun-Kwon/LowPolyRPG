using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHPBar : MonoBehaviour
{
    [SerializeField] Image _bossImageArea;
    [SerializeField] Text _bossNameArea;
    [SerializeField] Image _bossHPBar;

    BossAI _bossAI;

    int _bossHP, _bossCurrentHP;

    public void GetBossInformation(BossMonster bossMonster, BossAI bossAI)
    {
        _bossImageArea.sprite = bossMonster._bossImage;
        _bossNameArea.text = bossMonster._monsterName;
        _bossHP = bossMonster._monsterHP;
        _bossCurrentHP = _bossHP;
        _bossHPBar.fillAmount = (float)_bossCurrentHP / (float)_bossHP;
        _bossAI = bossAI;
    }

    public void SetHPBar(int value)
    {
        _bossCurrentHP = value;
        _bossHPBar.fillAmount = (float)_bossCurrentHP / (float)_bossHP;
    }
}
