using UnityEngine;
using UnityEngine.Playables;

public class TestMixerBehavior : PlayableBehaviour
{
    bool _firstHappened;
    Transform _lookAt;
    int _index = 0;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        Transform trackBinding = playerData as Transform;
        if (!trackBinding)
            return;

        Vector3 defaultPosition = trackBinding.position;

        int inputCount = playable.GetInputCount();

        Vector3 blendedPosition = Vector3.zero;
        Quaternion desiredRotation = trackBinding.rotation;

        for (int i = 0; i < inputCount; i++)
        {
            ScriptPlayable<TestBehavior> inputPlayable = (ScriptPlayable<TestBehavior>)playable.GetInput(i);
            TestBehavior input = inputPlayable.GetBehaviour();

            if (!input._endLocation)
                continue;

            float inputWeight = playable.GetInputWeight(i);

            if (!_firstHappened && !input._startLocation)
            {
                input._startingPosition = defaultPosition;
                trackBinding.rotation = Camera.main.transform.rotation;
            }

            float time = (float)(inputPlayable.GetTime() / inputPlayable.GetDuration());
            
            blendedPosition += Vector3.Lerp(input._startingPosition, input._endLocation.position, time * input._speed) * inputWeight;

            if (playable.GetInputWeight(_index) == 1f)
            {
                ScriptPlayable<TestBehavior> inputPlayableIndex = (ScriptPlayable<TestBehavior>)playable.GetInput(_index);
                TestBehavior inputIndex = inputPlayableIndex.GetBehaviour();
                Quaternion endRotation = Quaternion.LookRotation(inputIndex._lookRotation.position - trackBinding.position);
                desiredRotation = Quaternion.Slerp(trackBinding.rotation, endRotation, Time.deltaTime * input._speed);
            }
            else if (playable.GetInputWeight(_index) < 1f && playable.GetInputWeight(_index) > 0f)
            {
                if (_index < inputCount - 1)
                {
                    ScriptPlayable<TestBehavior> inputPlayableIndex = (ScriptPlayable<TestBehavior>)playable.GetInput(_index + 1);
                    TestBehavior inputIndex = inputPlayableIndex.GetBehaviour();
                    Quaternion endRotation = Quaternion.LookRotation(inputIndex._lookRotation.position - trackBinding.position);
                    desiredRotation = Quaternion.Slerp(trackBinding.rotation, endRotation, Time.deltaTime * input._speed);
                }
            }
            else if (playable.GetInputWeight(_index) == 0f)
            {
                if (_index < inputCount - 1)
                    _index++;
            }
        }

        trackBinding.position = blendedPosition;
        trackBinding.rotation = desiredRotation;

        _firstHappened = true;
    }

    public override void OnPlayableDestroy(Playable playable)
    {
        _firstHappened = false;
        _index = 0;
    }
}
