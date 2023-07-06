using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimatorFrame 
{
    const float frameRate = 0.08f;
    public static IEnumerator FrameGame(SpriteRenderer renderer, Sprite[] sprites, bool loopAnim = false, Action animTrigger = null, float _frameRate = frameRate)
    {
        int currentFrame = 0;
        renderer.sprite = sprites[currentFrame];
        if(sprites.Length > 1)
        {
            WaitForSeconds waitFrame = new WaitForSeconds(_frameRate);
            while (true)
            {
                yield return waitFrame;
                renderer.sprite = sprites[currentFrame];
                currentFrame++;
                if (currentFrame >= sprites.Length)
                {
                    if (loopAnim) break;
                    currentFrame = 0;
                }
            }
            animTrigger?.Invoke();
        }
    }
}
