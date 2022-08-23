using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHPBar : MonoBehaviour
{
    [SerializeField] Image _bossImageArea;
    [SerializeField] Text _bossNameArea;
    [SerializeField] Image _bossHPBar;

    int _bossHP, _bossCurrentHP;

    public void GetBossInformation(BossMonster bossMonster)
    {
        _bossImageArea.sprite = bossMonster._bossImage;
        _bossNameArea.text = bossMonster._monsterName;
        _bossHP = bossMonster._monsterHP;
        _bossCurrentHP = _bossHP;
        _bossHPBar.fillAmount = (float)_bossCurrentHP / (float)_bossHP;
    }

    public void ReduceHPBar(int value)
    {
        if (_bossCurrentHP <= 0)
            return;
        _bossCurrentHP -= value;
        if (_bossCurrentHP <= 0)
            _bossCurrentHP = 0;
        _bossHPBar.fillAmount = (float)_bossCurrentHP / (float)_bossHP;
    }
}
