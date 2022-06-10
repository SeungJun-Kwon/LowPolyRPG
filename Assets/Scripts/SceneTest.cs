using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTest : MonoBehaviour
{
    [SerializeField] string _currentScene;
    [SerializeField] string _nextScene;

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            Debug.Log("123");
            SceneManager.LoadScene(_nextScene);
        }
    }
}
