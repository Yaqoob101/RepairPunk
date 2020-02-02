using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMotor : MonoBehaviour
{
    public static CharacterMotor instance = null;

    enum FacingDirection { Up, Side, Down };
    public Vector2 moveDirection = Vector3.zero;
    public PressurizedRoom currentRoom;

    [SerializeField]
    LayerMask interactables;

    SpriteRenderer characterArt;
    float speed = 1.5f;
    Animator _animator;
    FacingDirection currentOrientation;
    bool stunned, dead;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        _animator = GetComponent<Animator>();
        _source = GetComponent<AudioSource>();
        characterArt = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        if (stunned || dead)
            return;

        transform.position += (Vector3)moveDirection * speed * Time.deltaTime;
        _animator.SetBool("isIdleUp", false);
        _animator.SetBool("isIdleDown", false);
        _animator.SetBool("isIdleSide", false);
        _animator.SetBool("isWalkingUp", false);
        _animator.SetBool("isWalkingDown", false);
        _animator.SetBool("isWalkingSide", false);

        if (moveDirection == Vector2.zero)
        {
            switch (currentOrientation)
            {
                case (FacingDirection.Up):
                    _animator.SetBool("isIdleUp", true);
                    break;
                case (FacingDirection.Side):
                    _animator.SetBool("isIdleSide", true);
                    break;
                case (FacingDirection.Down):
                    _animator.SetBool("isIdleDown", true);
                    break;
            }
            return;
        }

        if (moveDirection.x == 0)
        {
            if (moveDirection.y > 0)
            {
                // Character is facing up
                _animator.SetBool("isWalkingUp", true);
                currentOrientation = FacingDirection.Up;
            }
            else
            {
                // Character is facing down
                _animator.SetBool("isWalkingDown", true);
                currentOrientation = FacingDirection.Down;
            }
        }
        else
        {
            // Character is facing to the side
            _animator.SetBool("isWalkingSide", true);
            currentOrientation = FacingDirection.Side;
            if (moveDirection.x > 0)
                characterArt.flipX = false;
            else if (moveDirection.x < 0)
                characterArt.flipX = true;
        }
    }

    public void Interact()
    {
        if (stunned || dead)
            return;

        Vector2 rayDirection = Vector2.up;
        if (currentOrientation == FacingDirection.Side)
            rayDirection = characterArt.flipX ? Vector2.left : Vector2.right;
        if (currentOrientation == FacingDirection.Down)
            rayDirection = Vector2.down;

        //Debug.DrawRay(transform.position, rayDirection * 0.3f, Color.red,10f);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, 0.3f, interactables);

        if (hit)
            hit.collider.GetComponent<InteractableObject>().Interact();

    }

    static float maxHealth = 100;
    float currentHelth = maxHealth;
    public void TakeDamage(float damage)
    {
        if(!dead)
            StartCoroutine(DamageEffects(10f));
    }

    IEnumerator DamageEffects(float amount)
    {
        //Initial damage and feed back
        currentHelth -= amount;
        float speedReference = speed;
        float animationSpeed = _animator.speed;

        speed = 0;
        _animator.speed = 0;
        characterArt.color = Color.red;
        stunned = true;
        _source.clip = hit;
        _source.Play();

        // Play Animation
        yield return new WaitForSeconds(0.2f);

        stunned = false;
        speed = speedReference;
        _animator.speed = animationSpeed;
        characterArt.color = Color.white;

        if (currentHelth <= 0)
        {
            GameOverseer.instance.Failure();
            dead = true;
        }

    }

    public void BumpCharacter(Vector3 bumpVec, PressurizedRoom greaterPressureRoom)
    {
        StartCoroutine(MovePlayer(bumpVec, greaterPressureRoom));
        TakeDamage(10f);
    }

    IEnumerator MovePlayer(Vector3 bumpVec, PressurizedRoom greaterPressureRoom)
    {
        float timer = 0;
        float moveTime = 0.2f;

        bumpVec = currentRoom != greaterPressureRoom ? bumpVec : bumpVec * -1;

        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + bumpVec;


        while(timer < moveTime)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPos, timer / moveTime);
            yield return null;
        }

    }

    [SerializeField]
    AudioClip stepLeft, stepRight, hit;
    AudioSource _source;
    bool isLeftFoot;
    void Step()
    {
        isLeftFoot = !isLeftFoot;
        _source.clip = isLeftFoot ? stepLeft : stepRight;
        _source.Play();
    }
}
