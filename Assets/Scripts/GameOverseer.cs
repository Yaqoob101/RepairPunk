using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverseer : MonoBehaviour
{

    public static GameOverseer instance = null;

    [SerializeField]
    float endTime = 240;

    [SerializeField]
    GameObject shipIcon, startObject, endObject;

    [SerializeField]
    RectTransform bar;

    [SerializeField]
    Furniture[] allFurniture;

    bool isPaused;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        bar.sizeDelta = new Vector2( Vector2.Distance(startObject.transform.position, endObject.transform.position), bar.sizeDelta.y);
        StartCoroutine(GameTimer());
        InvokeRepeating("CreatePuncture", 1f, 1f);
    }

    void CreatePuncture()
    {
        if(Random.Range(1, 5) == 4)
        {
            allFurniture[Random.Range(0, allFurniture.Length)].AddPuncture();
        }
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
        Time.timeScale = isPaused ? 0 : 1;
    }
    void Pause()
    {
        isPaused = true;
        Time.timeScale = 0;
    }
    void Unpause()
    {
        isPaused = false;
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
        //if (Input.GetKeyDown(KeyCode.T))
        //    CharacterMotor.instance.TakeDamage();
    }

    public void Failure()
    {
        Pause();
        print("You Lose!");
    }
}
