using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHPBar : MonoBehaviour
{
    public Image _fill;

    RectTransform _rectTransform;

    private void Awake()
    {
        TryGetComponent(out _rectTransform);
    }

    private void Start()
    {
        StartCoroutine(UpdateCor());
    }

    private void OnDisable()
    {
        StopCoroutine(UpdateCor());
    }

    IEnumerator UpdateCor()
    {
        while(true)
        {
            transform.LookAt(transform.position + CameraController.instance.transform.rotation * Vector3.back, CameraController.instance.transform.rotation * Vector3.down);

            yield return null;
        }
    }

    public void SetTransform(Vector3 size)
    {
        _rectTransform.localPosition = new Vector3(0f, size.y + 0.5f, 0f);
        _rectTransform.localScale = new Vector3(size.x, size.y * 0.2f);
    }
}
