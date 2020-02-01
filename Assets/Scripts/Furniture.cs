using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furniture : InteractableObject
{
    public bool HasAPuncture;

    [SerializeField]
    string displayInfo;
    [SerializeField]
    PressurizedRoom room;

    float drainSpeed;

    private void Start()
    {
        drainSpeed = Random.Range(10, 100);
    }

    // Start is called before the first frame update
    public override void Interact()
    {
        if (!HasAPuncture)
            print(displayInfo);
        else
        {
            print("You have found and sealed a breach!");
            HasAPuncture = false;
        }
    }

    private void Update()
    {
        if (HasAPuncture)
        {
            room.AddAirPressure(-1 * Time.deltaTime); //This is possibly wrong, might need to be AddAir
        }

    }
}
