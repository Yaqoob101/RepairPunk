﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractableObject
{
    public const float DOOR_MAX_PRESSURE = 500.0f;
    public const float DOOR_BREAK_BUMP_SCALAR = 1;


    [SerializeField]
    Sprite open, shut;
    [SerializeField]
    PressurizedRoom[] rooms = new PressurizedRoom[2];

    SpriteRenderer _art;
    bool isOpen;
    bool isBroken;

    private void Start()
    {
        _art = GetComponent<SpriteRenderer>();
    }

    public override void Interact()
    {
        if (!isBroken) //Check if the door is broken, if it is not broken the player may interact with it normally, otherwise they may not interact with it and it is locked open.
        {
            isOpen = !isOpen;

            _art.sprite = isOpen ? open : shut;
            gameObject.layer = isOpen ? 9 : 8;
        }

    }

    private void Update()
    {
        if (isOpen)
        {
            if (Mathf.Abs(rooms[0].GetAirPressure() - rooms[1].GetAirPressure()) > 0.1f)
            {
                float changeInPressure = (rooms[0].GetAirPressure() - rooms[1].GetAirPressure()) / 2;
                rooms[0].AddAirPressure(-changeInPressure);
                rooms[1].AddAirPressure(changeInPressure);
            }
        }
        else
        {
            float differenceInPressure = rooms[0].GetDifferenceInAirPressure(rooms[1]);
            if (Mathf.Abs(differenceInPressure) > DOOR_MAX_PRESSURE)
            {
                print("A door broke! The air pressure difference was: " + differenceInPressure);
                if (differenceInPressure > 0)
                {
                    print("Door 0 had the greater pressure");
                    PullPlayer(rooms[0]);
                }
                else
                {
                    print("Door 1 had the greater pressure");
                    BumpPlayer(rooms[1]);
                }
                DoorBreak();
                
            }
        }
        //print("Air Pressure room 1: " + rooms[1].GetAirPressure());
    }

    private void DoorBreak()
    {
        isBroken = true;
        isOpen = true;
    }

    private void BumpPlayer(PressurizedRoom greaterPressureRoom)
    {
        Vector3 characterPos = CharacterMotor.instance.transform.position;
        float differenceX = characterPos.x - transform.position.x;
        float differenceY = characterPos.y - transform.position.y;

        Vector3 impulse = new Vector3(differenceX * DOOR_BREAK_BUMP_SCALAR, differenceY * DOOR_BREAK_BUMP_SCALAR, 0);

        CharacterMotor.instance.BumpCharacter(impulse,greaterPressureRoom);
    }
}