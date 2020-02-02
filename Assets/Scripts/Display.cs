using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Display : MonoBehaviour
{
    public static Display instance = null;

    [SerializeField]
    Sprite found, notFound;

    Image _image;
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        _image = GetComponent<Image>();
    }

    public void ShowImage(bool hasFoundHole)
    {
        StopAllCoroutines();
        StartCoroutine(ShowInfo(hasFoundHole ? found : notFound));
    }

    IEnumerator ShowInfo(Sprite newText)
    {

        _image.sprite = newText;
        _image.gameObject.SetActive(true);
        _image.color = Color.white;
        yield return new WaitForSeconds(2f);

        float timer = 0;
        float endTime = 1;

        while (timer < endTime)
        {
            timer += Time.deltaTime;
            _image.color = Color.Lerp(Color.white, Color.clear, timer / endTime);
            yield return null;
        }
    }
}
