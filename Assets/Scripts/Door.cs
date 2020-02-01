using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractableObject
{
    [SerializeField]
    Sprite open, shut;
    [SerializeField]
    PressurizedRoom[] rooms = new PressurizedRoom[2];
    
    SpriteRenderer _art;
    bool isOpen;

    private void Start()
    {
        _art = GetComponent<SpriteRenderer>();
    }

    public override void Interact()
    {
        isOpen = !isOpen;
        _art.sprite = isOpen ? open : shut;
        gameObject.layer = isOpen ? 9 : 8;
    }

    private void Update()
    {
        if(Mathf.Abs(rooms[0].GetAirPressure() - rooms[1].GetAirPressure()) > 0.1f)
        {
            float changeInPressure = (rooms[0].GetAirPressure() - rooms[1].GetAirPressure()) /2;
            rooms[0].AddAirPressure(-changeInPressure);
            rooms[1].AddAirPressure(changeInPressure);
        }
    }
}
