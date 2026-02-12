using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingManager : MonoBehaviour
{
    public List<GameObject> boxNoteList = new List<GameObject>();

    [SerializeField] Transform Center = null;
    [SerializeField] Transform[] timingRect = null;
    Vector2[] timingBoxs = null;

    public Action<String> onTimingCheck;

    // Start is called before the first frame update
    void Start()
    {
        timingBoxs = new Vector2[timingRect.Length];
        for (int i = 0; i < timingBoxs.Length; i++)
        {
            timingBoxs[i].Set(Center.localPosition.y - timingRect[i].localScale.y / 2,
                              Center.localPosition.y + timingRect[i].localScale.y / 2);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CheckTiming()
    {
        if (PlayerController.isRhythmGamePaused) return;

        for (int i = 0; i < boxNoteList.Count; i++)
        {
            float t_notePosY = boxNoteList[i].transform.localPosition.y;

            for (int y = 0; y < timingRect.Length; y++)
            {
                if (timingBoxs[y].x <= t_notePosY && t_notePosY <= timingBoxs[y].y)
                {
                    Destroy(boxNoteList[i]);
                    boxNoteList.RemoveAt(i);
                    String timing;
                    if (y == 0) timing = "Perfect";
                    else if (y == 1) timing = "Cool";
                    else if (y == 2) timing = "Good";
                    else timing = "Bad";

                    Debug.Log(timing);
                    onTimingCheck?.Invoke(timing);
                    if (ScoreManager.instance != null)
                    {
                        ScoreManager.instance.ProcessJudgement(timing);
                    }
                    return;
                }
            }
        }
        Debug.Log("Miss");
        onTimingCheck?.Invoke("Miss");
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.ProcessJudgement("Miss");
        }
    }
}
