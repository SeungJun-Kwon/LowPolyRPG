using System.Collections;
using TMPro;
using UnityEngine;

public class ObjectName : MonoBehaviour
{
    public TextMeshPro _text;

    RectTransform _rectTransform;
    Transform _camera;

    private void Awake()
    {
        TryGetComponent(out _rectTransform);
    }

    private void Start()
    {
        _camera = CameraController.instance.transform;
    }

    private void LateUpdate()
    {
        transform.rotation = _camera.transform.rotation;
    }

    public void SetText(Monster monster)
    {
        _text.text = string.Format("Lv.{0} {1}", monster._monsterLevel, monster._monsterName);
        _rectTransform.localPosition = new Vector3(0, -0.25f, 0);
    }

    public void SetText(NPC npc)
    {
        _text.text = string.Format("{0}", npc._name);
        _rectTransform.localPosition = new Vector3(0, -0.25f, 0);
    }
}
