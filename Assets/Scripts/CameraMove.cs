using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public bool CanFollow, zoomedIn;
    [SerializeField]
    float minZoom, maxZoom, sensitivity;
    Vector3 startPos;
    private void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        float size = Camera.main.orthographicSize;
        size += -Input.GetAxisRaw("Mouse ScrollWheel") * sensitivity;
        size = Mathf.Clamp(size, minZoom, maxZoom);
        Camera.main.orthographicSize = size;

        if (size != maxZoom)
            zoomedIn = true;
        else
        {
            zoomedIn = false;
            transform.position = startPos;
        }

        if (!CanFollow || !zoomedIn)
            return;

        transform.position = new Vector3 (CharacterMotor.instance.transform.position.x, CharacterMotor.instance.transform.position.y, transform.position.z);
    }
}
