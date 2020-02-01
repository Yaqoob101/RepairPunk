using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractableObject
{
    public const float DOOR_MAX_PRESSURE = 10.0f;
    public const float DOOR_BREAK_BUMP_SCALAR = 1;
    public const float PRESSURE_FUZZY_LIMIT = 0.1f;
    public const float ROOM_MIN_PRESSURE = 0.2f;


    [SerializeField]
    Sprite open, shut, broken;
    [SerializeField]
    PressurizedRoom[] rooms = new PressurizedRoom[2];
    [SerializeField]
    AudioClip opening, closing, breaking, smash;

    AudioSource _source;
    SpriteRenderer _art;
    bool isOpen = true;
    bool isBroken;

    private void Start()
    {
        _source = GetComponent<AudioSource>();
        _art = GetComponent<SpriteRenderer>();
    }

    public override void Interact()
    {
        if (!isBroken) //Check if the door is broken, if it is not broken the player may interact with it normally, otherwise they may not interact with it and it is locked open.
        {
            isOpen = !isOpen;

            if (isOpen)
            {
                StopAllCoroutines();
                isBreaking = false;
                _art.sprite = open;
                gameObject.layer = 9;
                _source.clip = opening;
            }
            else
            {
                _art.sprite = shut;
                gameObject.layer = 8;
                _source.clip = closing;
            }
            _source.time = 0;
            _source.Play();
        }
    }

    private void Update()
    {
        float differenceInPressure = rooms[0].GetDifferenceInAirPressure(rooms[1]);
        if (isOpen)
        {
            if (Mathf.Abs(differenceInPressure) > PRESSURE_FUZZY_LIMIT)
            {
                float changeInPressure = (differenceInPressure) / 2;
                rooms[0].AddAirPressure(-changeInPressure);
                rooms[1].AddAirPressure(changeInPressure);
            }
        }
        else
        {
            
            if (Mathf.Abs(differenceInPressure) > DOOR_MAX_PRESSURE || rooms[0].GetAirPressure() < ROOM_MIN_PRESSURE || rooms[1].GetAirPressure() < ROOM_MIN_PRESSURE)
            {
                //print("A door broke! The air pressure difference was: " + differenceInPressure);
                //if (differenceInPressure > 0)
                //{
                //    print("Room 0 had the greater pressure");
                //    BumpPlayer(rooms[0]);
                //}
                //else
                //{
                //    print("Room 1 had the greater pressure");
                //    BumpPlayer(rooms[1]);
                //}
                if (!isBreaking)
                {
                    isBreaking = true;
                    StartCoroutine(StartBreaking());
                }
            }
        }
        //print("Air Pressure room 1: " + rooms[1].GetAirPressure());
    }

    float timer = 0;
    float audioTime = 0;
    bool isBreaking;
    IEnumerator StartBreaking()
    {
        float endTime = breaking.length;
        _source.clip = breaking;
        _source.time = audioTime;
        _source.Play();

        while (timer < endTime)
        {
            timer += Time.deltaTime;
            audioTime = _source.time;
            yield return null;
        }

        if(timer >= endTime)
        {
            DoorBreak();
        }
    }

    private void DoorBreak()
    {
        _source.time = 0;
        _source.clip = smash;
        _source.Play();

        isBroken = true;
        isOpen = true;
        gameObject.layer = 9;
        //_art.sprite = broken;
        CameraShake.instance.Shake(0.5f);

        float differenceInPressure = rooms[0].GetDifferenceInAirPressure(rooms[1]);
        print("A door broke! The air pressure difference was: " + differenceInPressure);
        if (differenceInPressure > 0)
        {
            print("Room 0 had the greater pressure");
            BumpPlayer(rooms[0]);
        }
        else
        {
            print("Room 1 had the greater pressure");
            BumpPlayer(rooms[1]);
        }
    }

    private void BumpPlayer(PressurizedRoom greaterPressureRoom)
    {
        if (CharacterMotor.instance.currentRoom != rooms[0] && CharacterMotor.instance.currentRoom != rooms[1])
            return;

        //Vector3.Distance(CharacterMotor.instance.transform.position, transform.position)

        Vector3 direction = (CharacterMotor.instance.transform.position - transform.position).normalized;
        //float maxPulldistance = 1.5f;
        float distancePercentage = 1 / (Vector3.Distance(CharacterMotor.instance.transform.position,transform.position) * 2);
        float maxForce = 100;
        float pullAmount = maxForce * distancePercentage;
        Vector2 moveVector = direction * pullAmount;
        //float differenceX = characterPos.x - transform.position.x;
        //float differenceY = characterPos.y - transform.position.y;

        //Vector3 impulse = new Vector3(differenceX * DOOR_BREAK_BUMP_SCALAR, differenceY * DOOR_BREAK_BUMP_SCALAR, 0);
        Vector3 impulse = moveVector;
        CharacterMotor.instance.BumpCharacter(impulse,greaterPressureRoom);
    }
}