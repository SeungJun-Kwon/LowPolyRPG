using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossEntry : MonoBehaviour
{
    [SerializeField] Image _image;
    [SerializeField] Text _bossName;
    [SerializeField] Text _bossLv;
    [SerializeField] Text _bossHp;

    string _nextScene;

    public void SetInfo(BossMonster bossMonster)
    {
        _image.sprite = bossMonster._bossImage;
        _bossName.text = bossMonster._monsterName;
        _bossLv.text = "Lv : " + bossMonster._monsterLevel.ToString();
        _bossHp.text = "Hp : " + bossMonster._monsterHP.ToString();
        _nextScene = bossMonster._bossScene;
    }

    public void EnterBossRoom()
    {
        SceneManager.LoadScene(_nextScene);
        gameObject.SetActive(false);
    }

    public void Exit()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        PlayerController.instance.SetMyState(State.CANMOVE);
    }
}
