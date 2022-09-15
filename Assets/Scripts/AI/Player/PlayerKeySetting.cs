using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KeyAction { NONE, MOVE, ATTACK, ACTION, SKILL, PLAYERINFO, QUESTINFO, PAUSE, ESC }

public static class KeySetting { public static Dictionary<KeyCode, KeyAction> keys = new Dictionary<KeyCode, KeyAction>(); }

public class PlayerKeySetting : MonoBehaviour
{
    public KeyCode _attack = KeyCode.Mouse0;
    public KeyCode _move = KeyCode.Mouse1;
    public KeyCode _action = KeyCode.F;
    public KeyCode _playerInfoOpen = KeyCode.P;
    public KeyCode _questInfoOpen = KeyCode.L;
    public KeyCode[] _skill = { KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R };
    public KeyCode _esc = KeyCode.Escape;

    private void Awake()
    {
        KeySetting.keys.Add(_move, KeyAction.MOVE);
        KeySetting.keys.Add(_attack, KeyAction.ATTACK);
        KeySetting.keys.Add(_action, KeyAction.ACTION);
        foreach (var i in _skill) KeySetting.keys.Add(i, KeyAction.SKILL);
        KeySetting.keys.Add(_playerInfoOpen, KeyAction.PLAYERINFO);
        KeySetting.keys.Add(_questInfoOpen, KeyAction.QUESTINFO);
        KeySetting.keys.Add(_esc, KeyAction.ESC);
    }

    public KeyAction GetKeyAction(string key)
    {
        KeyAction keyAction = KeyAction.NONE;

        foreach (var keys in KeySetting.keys)
        {
            if (key == keys.Key.ToString())
            {
                keyAction = keys.Value;
                break;
            }
        }

        return keyAction;
    }
}
