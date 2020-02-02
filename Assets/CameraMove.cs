using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public bool CanFollow, zoomedIn;

    // Update is called once per frame
    void Update()
    {
        if (!CanFollow || !zoomedIn)
            return;
        transform.position = new Vector3 (CharacterMotor.instance.transform.position.x, CharacterMotor.instance.transform.position.y, transform.position.z);
    }
}
