using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Animation/Frame Character")]
public class FrameAnimation : ScriptableObject
{
    [Header("Idle")]
    public Sprite[] bodyFramesIdle;
    public Sprite[] footFramesIdle;

    [Header("Run")]
    public Sprite[] bodyFramesRun;
    public Sprite[] footFramesRun;

    [Header("Jump")]
    public Sprite[] bodyFramesJump;
    public Sprite[] footFramesJump;

    [Header("Attack")]
    public Sprite[] bodyFramesAttack;
    public Sprite[] footFramesAttack;
}
