using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoticeArea : MonoBehaviour
{
    [SerializeField] GameObject _noticeMessage;
    [SerializeField] int _maxNumOfMessages;

    [SerializeField] List<GameObject> _activeMessages = new List<GameObject>();
    [SerializeField] List<GameObject> _nonActiveMessages = new List<GameObject>();

    int _currentMessages = 0;

    GameObject CreateMessage()
    {
        var newMessage = Instantiate(_noticeMessage);
        newMessage.transform.SetParent(gameObject.transform, false);
        newMessage.transform.SetAsLastSibling();
        _activeMessages.Add(newMessage);
        return newMessage;
    }

    public void GetMessage(string content)
    {
        // If it have message prefab that is non active
        if(_nonActiveMessages.Count > 0)
        {
            var message = _nonActiveMessages[0];
            message.transform.SetAsLastSibling();
            message.TryGetComponent<NoticeMessage>(out var noticeMessage);
            noticeMessage.SetText(content);
            _activeMessages.Add(message);
            _nonActiveMessages.Remove(message);
            _currentMessages++;
            message.SetActive(true);
        }
        // If it have some message prefabs
        else if(_activeMessages.Count < _maxNumOfMessages)
        {
            var message = CreateMessage();
            message.TryGetComponent<NoticeMessage>(out var noticeMessage);
            noticeMessage.SetText(content);
        }
        // If it have an excess message prefabs
        else
        {
            _activeMessages[0].gameObject.SetActive(false);
            GetMessage(content);
        }
    }

    public void RemoveMessage(GameObject message)
    {
        _activeMessages.Remove(message);
        _nonActiveMessages.Add(message);
        _currentMessages--;
    }
}
