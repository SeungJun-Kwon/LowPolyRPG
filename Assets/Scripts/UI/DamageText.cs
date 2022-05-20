using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DamageText : MonoBehaviour
{
    //public Transform _startTransform;
    public Vector3 _offset = new Vector3(0, 1.5f, 0);
    public int _damage;

    private TextMeshPro _text;
    private float _moveSpeed = 1f;
    private float _alphaSpeed = 2f;
    private float _destroyTime = 2f;
    private Color _alpha, _originColor;

    private void Start()
    {
        _text = GetComponent<TextMeshPro>();
        //_originColor = _text.color;
        //_text.color = _originColor;
        _alpha = _text.color;
        _text.text = _damage.ToString();
        transform.position = transform.position + _offset;

        Destroy(gameObject, _destroyTime);
    }

    private void Update()
    {
        transform.Translate(new Vector3(0, _moveSpeed * Time.deltaTime, 0));
        _alpha.a = Mathf.Lerp(_alpha.a, 0, _alphaSpeed * Time.deltaTime);

        _text.color = _alpha;
    }
}
