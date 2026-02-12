using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimingText : MonoBehaviour
{
    [SerializeField] private TimingManager timingManager;
    private TextMeshProUGUI text;
    //private RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        //rectTransform = GetComponent<RectTransform>();
        timingManager.onTimingCheck += handleChangeText;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void handleChangeText(string timing)
    {
        text.text = timing;
        StartCoroutine(DeleteText(0.3f));   
    }

    IEnumerator DeleteText(float duration)
    {
        float totalDeltaTime = 0f;

        while (totalDeltaTime < duration)
        {
            totalDeltaTime += Time.deltaTime;
            float size = totalDeltaTime / duration;
            text.fontSize = 23 * (size+ 1);
            //rectTransform.localScale = Vector3.one * size;

            yield return null;
        }

        //yield return new WaitForSeconds(duration);
        text.text = null;
    }
}
