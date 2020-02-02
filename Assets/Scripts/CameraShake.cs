using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake: MonoBehaviour
{
    public static CameraShake instance = null;
    CameraMove mover;

    private void Start()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(this);

        Camera.main.GetComponent<CameraMove>();
    }

    public void Shake(float amount)
    {
        mover.CanFollow = false;
        StartCoroutine(Vibration(amount));
    }

    IEnumerator Vibration(float amount)
    {
        Vector3 origin = transform.position;
        float timer = 0;
        float endTime = 0.05f;

        for (int i = 0; i < 5; i++) {
            Vector3 startPos = transform.position;
            Vector3 endPos = new Vector3(Random.Range(origin.x - amount, origin.x + amount),
                                         Random.Range(origin.y - amount, origin.y + amount),
                                         Camera.main.transform.position.z);

            while (timer < endTime)
            {
                timer += Time.deltaTime;
                transform.position = Vector3.Lerp(startPos, endPos, timer / endTime);
                yield return null;
            }
            timer = 0;
        }

        transform.position = origin;
        mover.CanFollow = false;
    }
}
