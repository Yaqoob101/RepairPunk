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
    [SerializeField]
    float drainSpeed = 1;

    AudioSource _source;

    private void Start()
    {
        _source = GetComponent<AudioSource>();
        AddPuncture();
        //drainSpeed = Random.Range(10, 100);
        //drainSpeed = 1;
    }

    public void AddPuncture()
    {
        HasAPuncture = true;
        drainSpeed = Random.Range(1, 5);
        _source.Play();
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
            _source.Stop();
        }
    }

    private void Update()
    {
        if (HasAPuncture)
        {
            room.AddAirPressure(-drainSpeed * Time.deltaTime); //This is possibly wrong, might need to be AddAir
        }

    }
}
