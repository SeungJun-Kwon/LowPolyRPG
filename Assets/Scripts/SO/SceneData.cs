using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneData", menuName = "SceneData/SceneData")]
public class SceneData : ScriptableObject
{
    public enum SceneType { NONHUNTING, HUNTING, BOSS, }
    public SceneType _type;
    public string _name;
    public int _properLevel;
    public AudioClip _bgm;
}
