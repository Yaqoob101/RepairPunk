using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public virtual void Interact()
    {
        print("Just a regular " + gameObject.name);
    }
}
