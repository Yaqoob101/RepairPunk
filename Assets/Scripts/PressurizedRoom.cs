using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurizedRoom : MonoBehaviour
{
    public const float DEFAULT_VOLUME = 1.0f; //The total volume of space in the room by default
    public const float DEFAULT_AIR_AMOUNT = 1000.0f; //The defualt amount of air in a room


    //float airPressure = DEFAULT_AIRPRESSURE; //The air pressure in this room during this tick
    float airAmount = DEFAULT_AIR_AMOUNT;
    [SerializeField]
    float roomVolume = 100;

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
    */

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

    public float GetDifferenceInAirPressure(PressurizedRoom inRoom)
    {
        float difference = this.GetAirPressure() - inRoom.GetAirAmount();
        return difference;
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

        //if (inAir < 0)
        //{
        //    print("[Error]: Tried to SetAir to a negative amount, inAir is: " + inAir + " + airAmount was: " + airAmount + " Setting airAmount to " + airAmount);
        //}
        //else
        //{
            SetAir(airAmount + inAir);
        //}
    }

    private void SetAir(float inAir)
    {
        if (airAmount < 0)
        {
            //print("[Error]: Tried to SetAir while airAmount was negative, inAir is: " + inAir + " + airAmount was: " + airAmount + " Setting airAmount to 0");
            airAmount = 0;

        }
        else
        {
            airAmount = inAir;
        }

    }

}
