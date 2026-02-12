using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherSoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource upstairsSource;
    [SerializeField] private List<AudioClip> upstairsClips;

    [SerializeField] private AudioSource outsideSource;
    [SerializeField] private List<AudioClip> outsideClips;
    private int outsideIndex = 0; // 다음에 재생할 외부 소리 인덱스

    private float minUpInterval = 5.0f;
    private float maxUpInterval = 15.0f;

    private float minOutInterval = 7.0f;
    private float maxOutInterval = 15.0f;

    void Start()
    {
        // 두 소리 로직을 각각의 코루틴으로 실행
        StartCoroutine(PlayUpstairsSoundRoutine());
        StartCoroutine(PlayOutsideSoundRoutine());
    }

    // 1. 위층 소리: 완전 랜덤 재생
    IEnumerator PlayUpstairsSoundRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(minUpInterval, maxUpInterval);
            yield return new WaitForSeconds(waitTime);
            PlayRandomUpstairsClip();
        }
    }

    // 2. 외부 소리: 랜덤 시간 대기 + 순차적 재생
    IEnumerator PlayOutsideSoundRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(minOutInterval, maxOutInterval);
            yield return new WaitForSeconds(waitTime);
            PlayNextOutsideClip();
        }
    }

    void PlayRandomUpstairsClip()
    {
        if (upstairsClips.Count == 0 || upstairsSource == null) return;

        int randomIndex = Random.Range(0, upstairsClips.Count);
        upstairsSource.volume = 0.5f;
        upstairsSource.pitch = Random.Range(0.9f, 1.1f);
        upstairsSource.PlayOneShot(upstairsClips[randomIndex]);
    }

    void PlayNextOutsideClip()
    {
        if (outsideClips.Count == 0 || outsideSource == null) return;

        // 현재 순서의 클립 재생
        outsideSource.volume = 0.5f;
        outsideSource.pitch = Random.Range(0.9f, 1.1f);
        outsideSource.PlayOneShot(outsideClips[outsideIndex]);

        // 3. 인덱스 1씩 증가 (리스트 끝에 도달하면 다시 0으로 초기화)
        outsideIndex = (outsideIndex + 1) % outsideClips.Count;
    }
}