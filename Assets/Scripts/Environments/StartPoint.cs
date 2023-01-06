using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartPoint : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(cor());
    }

    IEnumerator cor()
    {
        GameObject go = null;
        while(go == null)
        {
            go = GameObject.FindGameObjectWithTag("Player").gameObject;
            yield return null;
        }

        PlayerController.instance.ToTheStartPoint();
    }
}
