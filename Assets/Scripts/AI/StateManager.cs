using System;
using System.Collections.Generic;
using UnityEngine;

public enum State { IDLE, MOVE, ATTACK, CANMOVE, CANTMOVE, DEAD, };
public class StateManager : MonoBehaviour
{
    bool _canMove;
    bool _isMove;
    bool _isAttack;
    bool _isRigidImmuntity;
    bool _isDead;

    public void SetInitState()
    {
        _canMove = true;
        _isMove = false;
        _isAttack = false;
        _isRigidImmuntity = false;
        _isDead = false;
    }

    public void SetState(State state)
    {
        switch(state)
        {
            case State.IDLE:
                _canMove = true;
                _isMove = false;
                _isAttack = false;
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
                SetInitState();
                _canMove = false;
                break;
            case State.DEAD:
                _canMove = false;
                _isDead = true;
                break;
        }
    }

    public bool IsCanMove()
    {
        return (_canMove == true);
    }

    public bool IsMoving()
    {
        return (_canMove == true) && (_isMove == true);
    }

    public bool IsAttacking()
    {
        return (_canMove == false) && (_isMove == false) && (_isAttack == true);
    }
}
