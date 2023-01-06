using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossEntry : BaseUI
{
    [SerializeField] Image _image;
    [SerializeField] Text _bossName;
    [SerializeField] Text _bossLv;
    [SerializeField] Text _bossHp;

    BossMonster _boss;
    SceneData _nextScene;

    public void SetInfo(BossMonster bossMonster)
    {
        _boss = bossMonster;

        _image.sprite = _boss._bossImage;
        _bossName.text = _boss._monsterName;
        _bossLv.text = "Lv : " + _boss._monsterLevel.ToString();
        _bossHp.text = "Hp : " + _boss._monsterHP.ToString();
        _nextScene = _boss._bossScene;
    }

    public void EnterBossRoom()
    {
        if (PlayerController.instance.PlayerManager._playerLv >= _boss._monsterLevel)
        {
            SceneMoveManager.instance.LoadScene(_nextScene);
            gameObject.SetActive(false);
            return;
        }

        UIController.instance.NoticeArea.GetMessage(string.Format("레벨이 부족합니다.\n입장 레벨 : {0}", _boss._monsterLevel));
    }

    public void Exit()
    {
        gameObject.SetActive(false);
    }
}
