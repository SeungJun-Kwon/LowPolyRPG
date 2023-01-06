using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KeyAction { NONE, MOVE, ATTACK, ACTION, SKILL, PLAYERINFO, PAUSE, ESC, HEALTHPOTION, MANAPOTION }

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
    public KeyCode _healthPotion = KeyCode.Alpha1;
    public KeyCode _manaPotion = KeyCode.Alpha2;

    private void Awake()
    {
        if (!KeySetting.keys.ContainsKey(_move))
            KeySetting.keys.Add(_move, KeyAction.MOVE);
        if (!KeySetting.keys.ContainsKey(_attack))
            KeySetting.keys.Add(_attack, KeyAction.ATTACK);
        if (!KeySetting.keys.ContainsKey(_action))
            KeySetting.keys.Add(_action, KeyAction.ACTION);
        foreach (var i in _skill)
        {
            if (!KeySetting.keys.ContainsKey(i))
                KeySetting.keys.Add(i, KeyAction.SKILL);
        }
        if (!KeySetting.keys.ContainsKey(_playerInfoOpen))
            KeySetting.keys.Add(_playerInfoOpen, KeyAction.PLAYERINFO);
        if (!KeySetting.keys.ContainsKey(_esc))
            KeySetting.keys.Add(_esc, KeyAction.ESC);
        if (!KeySetting.keys.ContainsKey(_healthPotion))
            KeySetting.keys.Add(_healthPotion, KeyAction.HEALTHPOTION);
        if (!KeySetting.keys.ContainsKey(_manaPotion))
            KeySetting.keys.Add(_manaPotion, KeyAction.MANAPOTION);
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
