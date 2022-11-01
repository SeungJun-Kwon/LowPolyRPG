using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    [Header("Follow Player")]
    [SerializeField] Transform _targetTransform;
    [SerializeField] float _cameraSpeed;
    [SerializeField] Vector3 _offset;
    [SerializeField] Vector3 _minOffset, _maxOffset;
    [SerializeField] Vector3 _rotation;
    [SerializeField] float _scrollSpeed = 5f;

    [Header("Event Camera")]
    [SerializeField] Transform _eventCam;

    [HideInInspector] public bool _isAble = true;
    [HideInInspector] public bool _isEvent = false;

    private void Awake()
    {
        #region Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
        #endregion

        if (!_targetTransform)
            _targetTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void LateUpdate()
    {
        if (_isAble)
        {
            transform.position = Vector3.Lerp(transform.position, _targetTransform.position + _offset, Time.deltaTime * _cameraSpeed);
            transform.rotation = Quaternion.Euler(_rotation);

            float scroll = Input.GetAxis("Mouse ScrollWheel");

            if (scroll > 0 && _offset.magnitude > _minOffset.magnitude)
                _offset -= new Vector3(0, 0.2f, -0.1f) * _scrollSpeed;
            else if (scroll < 0 && _offset.magnitude < _maxOffset.magnitude)
                _offset += new Vector3(0, 0.2f, -0.1f) * _scrollSpeed;
        }
        else if (_isEvent)
        {
            transform.position = _eventCam.position;
            transform.rotation = _eventCam.rotation;
        }

        if (Input.GetKeyDown(KeyCode.A))
            CameraShake(1f);
    }

    public void CameraShake(float duration, float magnitude = 1f)
    {
        _isAble = false;
        StartCoroutine(Shake(duration));
    }

    IEnumerator Shake(float duration, float magnitude = 1f)
    {
        Vector3 startPos = transform.position;
        float timer = 0;

        while(timer < duration)
        {
            transform.position = startPos + new Vector3(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f)) * magnitude;

            timer += Time.deltaTime;
            yield return null;
        }

        transform.position = startPos;
        _isAble = true;
    }
}
