using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] TimingManager theTimingManager0;
    [SerializeField] TimingManager theTimingManager1;
    [SerializeField] TimingManager theTimingManager2;
    [SerializeField] TimingManager theTimingManager3;

    public static bool isRhythmGamePaused = false;
    public CenterFrame centerFrameScript;

    // Start is called before the first frame update
    void Start()
    {
        isRhythmGamePaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            // 판정 체크
            theTimingManager0.CheckTiming();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            // 판정 체크
            theTimingManager1.CheckTiming();
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            // 판정 체크
            theTimingManager2.CheckTiming();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            // 판정 체크
            theTimingManager3.CheckTiming();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    void TogglePause()
    {
        // 상태를 반대로 뒤집음 (true <-> false)
        isRhythmGamePaused = !isRhythmGamePaused;

        if (isRhythmGamePaused)
        {
            // [멈춤] 음악만 일시 정지 (시간은 건드리지 않음!)
            if (centerFrameScript.myAudio != null) centerFrameScript.myAudio.Pause();
        }
        else
        {
            // [재개] 음악 다시 재생
            if (centerFrameScript.myAudio != null) centerFrameScript.myAudio.UnPause();
        }
    }
}
