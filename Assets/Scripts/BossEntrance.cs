using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossEntrance : MonoBehaviour
{
    [SerializeField] BossMonster _bossMonster;

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            UIController.instance.BossEntry.gameObject.SetActive(true);
            UIController.instance.BossEntry.SetInfo(_bossMonster);
            PlayerController.instance.SetMyState(State.CANTMOVE);
        }
    }
}
