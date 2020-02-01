using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurizedRoom : MonoBehaviour
{
    public const int DEFAULT_AIRPRESSURE = 1000; //The default pressure for a room.
    public const int DEFAULT_VOLUME = 10; //The total volume of space in the room by default
    public const int DEFAULT_AIR_AMOUNT = 10; //The defualt amount of air in a room


    //float airPressure = DEFAULT_AIRPRESSURE; //The air pressure in this room during this tick
    float airAmount = DEFAULT_AIR_AMOUNT;
    float roomVolume;

    /*
    public PressurizedRoom(float inVolume)
        {
        // Room volume cannot be less than or equal to zero.
        if(inVolume <= 0)
            {

            }
        else
            {
            roomVolume = inVolume;
            }
        }

    public PressurizedRoom(float initialPressure, float inVolume)
        {
        new PressurizedRoom(inVolume);
        SetAirPressure(initialPressure);
        }
        */
    public PressurizedRoom(float initialCapacity, float initialPressure, float inVolume)
    {
        if (inVolume <= 0)
        {

        }
        else
        {
            roomVolume = inVolume;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public float GetAirPressure()
    {
        return airAmount / roomVolume;
    }

    public float GetAirAmount()
    {
        return airAmount;
    }

    public float GetRoomVolume()
    {
        return roomVolume;
    }



    public void AddAirPressure(float deltaPressure)
    {
        //if Pressure = Air/Volume THEN deltaPressure = (AirOld + deltaAir)/Volume THEREFORE (deltaPressure/Volume) - AirOld = deltaAir
        float newPressure = deltaPressure + GetAirPressure();
        float deltaAir = (newPressure * roomVolume) - airAmount;
        AddAir(deltaAir);
    }
    public void AddAir(float inAir)
    {
        SetAir(airAmount + inAir);
        print(airAmount + " " + GetAirPressure());
    }

    private void SetAir(float inAir)
    {
        airAmount = inAir;
    }

}
    