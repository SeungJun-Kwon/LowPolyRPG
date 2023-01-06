using UnityEngine;
using UnityEngine.Playables;

public class TestBehavior : PlayableBehaviour
{
    public Transform _startLocation;
    public Transform _endLocation;
    public Transform _lookRotation;
    public float _speed = 1f;

    public Vector3 _startingPosition;

    public override void PrepareFrame(Playable playable, FrameData info)
    {
        if (_startLocation)
        {
            _startingPosition = _startLocation.position;
        }
    }
}
