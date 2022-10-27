using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartPoint : MonoBehaviour
{
    private void Start()
    {
        PlayerController.instance.ToTheStartPoint();
    }
}
