using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverseer : MonoBehaviour
{
    [SerializeField]
    float endTime = 240;

    [SerializeField]
    GameObject shipIcon, startObject, endObject;

    [SerializeField]
    RectTransform bar;

    bool isPaused;

    // Start is called before the first frame update
    void Start()
    {
        bar.sizeDelta = new Vector2( Vector2.Distance(startObject.transform.position, endObject.transform.position), bar.sizeDelta.y);
        StartCoroutine(GameTimer());  
    }

    IEnumerator GameTimer()
    {
        //Set Values
        float timer = 0;

        //Loop
        while (timer < endTime)
        {
            timer += Time.deltaTime;

            Vector2 startPoint = startObject.transform.position;
            Vector2 endPoint = endObject.transform.position;
            bar.sizeDelta = new Vector2(Vector2.Distance(startPoint,endPoint), bar.sizeDelta.y);

            shipIcon.transform.position = Vector2.Lerp(startPoint, endPoint, timer / endTime);
            yield return null;
        }
        // End
        print("You win");
    }

    private void TogglePause()
    {
        isPaused = !isPaused;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
