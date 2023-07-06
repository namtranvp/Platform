using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public FrameAnimation framePlayer;

    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float jumpForce;
    public bool onGround;

    Rigidbody2D rb;
    float h;

    bool facingRight;
    [SerializeField] SpriteRenderer[] sprites;

    Coroutine bodyFrame;
    Coroutine footFrame;
    Status statusPlayer = Status.IDLE;
    [SerializeField] Sprite[] jumpSprite;
    [SerializeField] Sprite deathSprite;

    [SerializeField] SkillAnimation skillAnimation;
    [SerializeField] FrameSkill frameSkill;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        PlayAnimation(statusPlayer);
    }

    void Update()
    {
        h = Input.GetAxisRaw(TagScript.Horizontal);
        onGround = Physics2D.Linecast(transform.position, groundCheck.position, groundLayer);

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            
            if (onGround)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                AnimationFullBody(true);
                statusPlayer = Status.JUMP;
                StartCoroutine(AnimatorFrame.FrameGame(sprites[4], jumpSprite, true, () =>
                {
                    AnimationFullBody(false);
                    PlayAnimation(Status.JUMP);
                }, 0.1f
                ));
            }
        }

        if (Input.GetKeyDown(KeyCode.I)) PlayAnimation(Status.IDLE);
        if (Input.GetKeyDown(KeyCode.R)) PlayAnimation(Status.RUN);
        if (Input.GetKeyDown(KeyCode.J)) PlayAnimation(Status.JUMP);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (statusPlayer == Status.ATTACK) return;
            PlayAnimation(Status.ATTACK);
            skillAnimation.AnimationSkill(frameSkill);
        }

    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(h * speed, rb.velocity.y);

        if (h > 0 && facingRight || h < 0 && !facingRight) Flip();
        
        //if (!onGround) PlayAnimation(Status.JUMP);

        if (statusPlayer != Status.RUN && statusPlayer != Status.JUMP)
            if (h > 0 || h < 0) PlayAnimation(Status.RUN);

        if (h == 0)
        {
            if (statusPlayer == Status.IDLE || statusPlayer == Status.ATTACK || statusPlayer == Status.JUMP) return;
            PlayAnimation(Status.IDLE);
        }
    }
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    void PlayAnimation(Status status)
    {
        if (bodyFrame != null) StopCoroutine(bodyFrame);
        if (footFrame != null) StopCoroutine(footFrame);
        switch (status)
        {
            case Status.IDLE:
                statusPlayer = Status.IDLE;
                bodyFrame = StartCoroutine(AnimatorFrame.FrameGame(sprites[1], framePlayer.bodyFramesIdle));
                footFrame = StartCoroutine(AnimatorFrame.FrameGame(sprites[2], framePlayer.footFramesIdle));
                break;
            case Status.RUN:
                statusPlayer = Status.RUN;
                bodyFrame = StartCoroutine(AnimatorFrame.FrameGame(sprites[1], framePlayer.bodyFramesRun));
                footFrame = StartCoroutine(AnimatorFrame.FrameGame(sprites[2], framePlayer.footFramesRun));
                break;
            case Status.JUMP:
                statusPlayer = Status.JUMP;
                bodyFrame = StartCoroutine(AnimatorFrame.FrameGame(sprites[1], framePlayer.bodyFramesJump, true, SetStatusIdle, 0.2f));
                footFrame = StartCoroutine(AnimatorFrame.FrameGame(sprites[2], framePlayer.footFramesJump, false, null, 0.5f));
                break;
            case Status.ATTACK:
                statusPlayer = Status.ATTACK;
                bodyFrame = StartCoroutine(AnimatorFrame.FrameGame(sprites[1], framePlayer.bodyFramesAttack, true, SetStatusIdle));
                footFrame = StartCoroutine(AnimatorFrame.FrameGame(sprites[2], framePlayer.footFramesAttack));
                break;
        }
    }
    void SetStatusIdle()
    {
        statusPlayer = Status.IDLE;
        PlayAnimation(statusPlayer);
    }
    void AnimationFullBody(bool isFull)
    {
        for (int i = 0; i < sprites.Length - 1; i++)
        {
            sprites[i].gameObject.SetActive(!isFull);
        }
        sprites[4].gameObject.SetActive(isFull);
    }
}
public enum Status { IDLE, RUN, JUMP, ATTACK, HIT, DEATH }
