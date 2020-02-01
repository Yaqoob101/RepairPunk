using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractableObject
{
    public const float DOOR_MAX_PRESSURE = 800.0f;


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
            if (rooms[0].GetDifferenceInAirPressure(rooms[1]) > DOOR_MAX_PRESSURE)
            {
                doorBreak();
                pullPlayer();
            }
        }
        //print("Air Pressure room 1: " + rooms[1].GetAirPressure());
    }

    private void doorBreak()
    {
        isBroken = true;
        isOpen = true;
    }

    private void pullPlayer()
    {
    
    }
}