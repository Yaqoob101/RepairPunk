using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : InteractableObject
{
    public const float DOOR_MAX_PRESSURE = 10.0f;
    public const float DOOR_BREAK_BUMP_SCALAR = 1;
    public const float PRESSURE_FUZZY_LIMIT = 0.1f;
    public const float ROOM_MIN_PRESSURE = 0.2f;


    [SerializeField]
    Sprite[] open, shut, broken;
    [SerializeField]
    PressurizedRoom[] rooms = new PressurizedRoom[2];
    [SerializeField]
    AudioClip opening, closing, breaking, smash;
    [SerializeField]
    Slider healthSlider;
    [SerializeField]
    Image healthBar;
    [SerializeField]
    Color fullHealth, noHealth;
    [SerializeField]
    bool isVertical;

    AudioSource _source;
    [SerializeField]
    SpriteRenderer[] _art;
    bool isOpen = true;
    bool isBroken;

    private void Start()
    {
        _source = GetComponent<AudioSource>();
        healthSlider.gameObject.SetActive(false);
        //_art = GetComponent<SpriteRenderer>();
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
                for(int i = 0; i < _art.Length; i++)
                    _art[i].sprite = open[i];
                gameObject.layer = 9;
                _source.clip = opening;
                _art[0].sortingOrder = 2;

            }
            else
            {
                for (int i = 0; i < _art.Length; i++)
                    _art[i].sprite = shut[i];
                gameObject.layer = 8;
                _source.clip = closing;
               _art[0].sortingOrder = isVertical ? 0 : 2;
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
                if (!isBreaking)
                {
                    isBreaking = true;
                    StartCoroutine(StartBreaking());
                }
            }
        }
    }

    float timer = 0;
    float audioTime = 0;
    bool isBreaking;
    IEnumerator StartBreaking()
    {
        healthSlider.gameObject.SetActive(true);
        float endTime = breaking.length;
        _source.clip = breaking;
        _source.time = audioTime;
        _source.Play();

        while (timer < endTime)
        {
            timer += Time.deltaTime;
            audioTime = _source.time;
            healthSlider.value = 1 - (timer / endTime);
            healthBar.color = Color.Lerp(fullHealth, noHealth, timer / endTime);
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
        for (int i = 0; i < _art.Length; i++)
            _art[i].sprite = broken[i];
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
        float force = Mathf.Lerp(1, 0, Vector3.Distance(CharacterMotor.instance.transform.position, transform.position) / 2);
        //print(Vector3.Distance(CharacterMotor.instance.transform.position, transform.position) + " " + force);

        //float maxPulldistance = 1.5f;
        //float distancePercentage = 1 / (Vector3.Distance(CharacterMotor.instance.transform.position,transform.position) * 2);
        //float maxForce = 100;
        //float pullAmount = maxForce * distancePercentage;
        //Vector2 moveVector = direction * force;
        //float differenceX = characterPos.x - transform.position.x;
        //float differenceY = characterPos.y - transform.position.y;

        //Vector3 impulse = new Vector3(differenceX * DOOR_BREAK_BUMP_SCALAR, differenceY * DOOR_BREAK_BUMP_SCALAR, 0);
        Vector3 impulse = direction * force;
        if(impulse != Vector3.zero)
            CharacterMotor.instance.BumpCharacter(impulse,greaterPressureRoom);
    }
}