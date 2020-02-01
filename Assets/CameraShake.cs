using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake: MonoBehaviour
{
    public static CameraShake instance = null;
    float difference = 1;
    private void Start()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public void Shake()
    {
        StartCoroutine(Vibration());
    }

    IEnumerator Vibration()
    {
        Vector3 origin = transform.position;
        float timer = 0;
        float endTime = 0.05f;

        for (int i = 0; i < 5; i++) {
            Vector3 startPos = transform.position;
            Vector3 endPos = new Vector3(Random.Range(origin.x - difference, origin.x + difference),
                                         Random.Range(origin.y - difference, origin.y + difference),
                                         Camera.main.transform.position.z);

            while (timer < endTime)
            {
                print(transform.position);
                timer += Time.deltaTime;
                transform.position = Vector3.Lerp(startPos, endPos, timer / endTime);
                yield return null;
            }
            timer = 0;
        }

        transform.position = origin;
    }
}
