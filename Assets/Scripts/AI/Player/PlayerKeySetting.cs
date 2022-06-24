using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKeySetting : MonoBehaviour
{
    public KeyCode _attack = KeyCode.Mouse0;
    public KeyCode _move = KeyCode.Mouse1;
    public KeyCode _action = KeyCode.F;
    public KeyCode _playerInfoOpen = KeyCode.P;
    public KeyCode _questInfoOpen = KeyCode.L;
    public KeyCode[] _skill = { KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R };
    public KeyCode _esc = KeyCode.Escape;
}
