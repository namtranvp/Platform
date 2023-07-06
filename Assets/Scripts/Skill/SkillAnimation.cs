using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SkillAnimation : MonoBehaviour
{
    SpriteRenderer skillSprite;
    Vector2 positionStart;
    [SerializeField] Transform target;
    void Awake()
    {
        positionStart = transform.localPosition;
        skillSprite = GetComponent<SpriteRenderer>();
    }
    public void AnimationSkill(FrameSkill _frameSkill)
    {
        gameObject.SetActive(true);
        StartCoroutine(AnimatorFrame.FrameGame(skillSprite, _frameSkill.skillFrames, true, null, 0.1f));
        MoveSkill();
    }
    void MoveSkill()
    {
        transform.DOKill();
        transform.localPosition = positionStart;
        var t = transform.DOMove(target.position, 0.5f).OnComplete(()=> gameObject.SetActive(false));
    }
}
