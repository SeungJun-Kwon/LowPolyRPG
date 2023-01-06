using System;
using System.Collections.Generic;
using UnityEngine;

public enum State { IDLE, MOVE, ATTACK, CANMOVE, CANTMOVE, DEAD, STUNNED };
public class StateManager : MonoBehaviour
{
    bool _canMove;
    bool _isMove;
    bool _isAttack;

    public void SetInitState()
    {
        _canMove = true;
        _isMove = false;
        _isAttack = false;
    }

    public void SetState(State state)
    {
        switch(state)
        {
            case State.IDLE:
                SetInitState();
                break;
            case State.MOVE:
                _isMove = true;
                break;
            case State.ATTACK:
                _canMove = false;
                _isMove = false;
                _isAttack = true;
                break;
            case State.CANMOVE:
                _canMove = true;
                break;
            case State.CANTMOVE:
                _isMove = false;
                _canMove = false;
                break;
            case State.DEAD:
                _canMove = false;
                _isMove = false;
                break;
            case State.STUNNED:
                _canMove = false;
                _isMove = false;
                break;
        }
    }

    public bool IsCanMove()
    {
        return _canMove == true;
    }

    public bool IsMoving()
    {
        return _isMove == true;
    }

    public bool IsAttacking()
    {
        return (_canMove == false) && (_isMove == false) && (_isAttack == true);
    }
}
