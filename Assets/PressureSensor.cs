using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PressureSensor : MonoBehaviour
{
    [SerializeField]
    Text pressureOutput;

    PressurizedRoom currentRoom;
    float hurtTimer = 1.2f;
    float timer = 0;

    private void OnTriggerEnter2D (Collider2D c)
    {
        if(c.tag == "room")
        {
            currentRoom = c.GetComponent<PressurizedRoom>();
        }
    }

    private void Update()
    {
        if(currentRoom != null)
        {
            int pressure = (int)currentRoom.GetAirPressure();
            pressureOutput.text = pressure.ToString();
            if (pressure <= 0)
            {
                hurtTimer += Time.deltaTime;
                if (timer >= hurtTimer)
                {
                    CharacterMotor.instance.TakeDamage(10f);
                }
            } else
            {
                timer = 0;
            }
        }
    }
}
