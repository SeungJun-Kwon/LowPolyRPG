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
    }

    private void Start()
    {
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

    public IEnumerator BossEnterCameraWork(Transform boss, float time)
    {
        PlayerController.instance.SetMyState(State.CANTMOVE);
        UIController.instance.PlayUI.SetActive(false);
        UIController.instance.CinemachineUI.SetActive(true);
        _isAble = false;
        float count = 0f;

        Vector3 offset;
        offset = new Vector3(boss.position.x + 4f, boss.position.y + 8f, boss.position.z - 10f);
        transform.position = offset;
        while (count < time / 2)
        {
            count += Time.deltaTime;

            transform.position = new Vector3(transform.position.x - 0.01f, transform.position.y, transform.position.z);
            transform.rotation = Quaternion.LookRotation(boss.position - transform.position);

            yield return null;
        }

        offset = new Vector3(boss.position.x, boss.position.y + 3f, boss.position.z - 3f);
        transform.position = offset;
        while (count < time)
        {
            count += Time.deltaTime;

            transform.position = new Vector3(transform.position.x - 0.001f, transform.position.y, transform.position.z);
            transform.rotation = Quaternion.LookRotation(boss.position - transform.position);

            yield return null;
        }

        boss.gameObject.TryGetComponent<BossAI>(out var bossAI);
        bossAI.SetMyState(State.CANMOVE);
        PlayerController.instance.SetMyState(State.CANMOVE);
        UIController.instance.PlayUI.SetActive(true);
        UIController.instance.CinemachineUI.SetActive(false);
        _isAble = true;
    }

    public IEnumerator BossEndCameraWork(Transform boss, float time)
    {
        Time.timeScale = 0.5f;
        _isAble = false;
        float count = 0f;

        Vector3 offset;
        offset = new Vector3(boss.position.x + 2f, boss.position.y + 4f, boss.position.z - 5f);
        transform.position = offset;

        while(count < time)
        {
            count += Time.deltaTime;

            transform.RotateAround(new Vector3(boss.position.x, boss.position.y + 4f, boss.position.z), Vector3.up, Time.deltaTime * 20);

            yield return null;
        }

        Time.timeScale = 1f;
        _isAble = true;
    }

    public IEnumerator LookNPC(Transform transform, float size = 1f)
    {
        _isAble = false;
        float count = 0f;

        Vector3 offset = new Vector3(transform.position.x, transform.position.y + size, transform.position.z);

        while(count < 1f)
        {
            count += Time.deltaTime;

            this.transform.position = Vector3.Lerp(this.transform.position, offset + transform.forward * 2, Time.deltaTime * 10);
            this.transform.LookAt(offset);

            yield return null;
        }
    }
}
