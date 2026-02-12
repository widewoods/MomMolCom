using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MomController : MonoBehaviour
{
    private float minSpawnTime = 3f;
    private float maxSpawnTime = 5f;
 
    private float minWarningDuration = 2f;
    private float maxWarningDuration = 4f;
 
    private float minMomStayDuration = 1f;
    private float maxMomStayDuration = 4f;

    private float leavingDuration = 2.0f;  // 나갈 때 발자국 소리 들리는 시간

    public AudioSource audioSource;      // 발자국 소리를 낼 오디오 소스 컴포넌트
    public AudioClip footstepsClip;      // 발자국 MP3 파일

    //public GameObject momObject;         // 엄마 게임 오브젝트 (방문 등)

    // 노트북 상태 (외부 스크립트에서 이 변수를 제어하거나, 프로퍼티로 연결해야 함)
    public PlayerContorller3D laptop;

    private bool isGameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        //if (momObject != null) momObject.SetActive(false);

        // 엄마가 오는 패턴 시작
        StartCoroutine(MomPatternRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 엄마 행동 패턴 코루틴
    IEnumerator MomPatternRoutine()
    {
        while (!isGameOver)
        {
            // 1. 랜덤한 시간만큼 대기 (평화로운 시간)
            float randomWait = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(randomWait);

            // 2. 발자국 소리 재생 (경고 단계)
            Debug.Log("발자국 소리가 들림");
            PlayFootstepSound(); // 소리 재생 함수 호출

            // 3. N초(warningDuration) 만큼 대기 (플레이어가 노트북을 덮을 시간)
            float warningDuration = Random.Range(minWarningDuration, maxWarningDuration);
            yield return new WaitForSeconds(warningDuration);

            // 4. 엄마 등장 및 노트북 확인
            StopFootstepSound();

            Debug.Log("엄마 들어옴");

            float timer = 0f;
            bool caught = false;

            float momStayDuration = Random.Range(minMomStayDuration, maxMomStayDuration);
            while (timer < momStayDuration)
            {
                // 실시간으로 노트북이 열려있는지 체크
                if (laptop.openLaptop)
                {
                    Debug.Log("엄마한테 걸림");
                    GameOver();
                    caught = true;
                    break; // 감시 루프 탈출
                }

                // 다음 프레임까지 대기하며 시간을 잰다
                timer += Time.deltaTime;
                yield return null;
            }

            // 만약 걸렸다면 더 이상 패턴을 진행하지 않고 종료
            if (caught) yield break;

            Debug.Log("엄마 나감");

            PlayFootstepSound(); // 다시 발자국 소리 재생

            // 나가는 발자국 소리를 얼마나 들려줄지 (예: 2초)
            yield return new WaitForSeconds(leavingDuration);

            StopFootstepSound();
        }
    }

    // 소리 재생을 편하게 하기 위해 만든 함수
    void PlayFootstepSound()
    {
        if (audioSource != null && footstepsClip != null)
        {
            audioSource.clip = footstepsClip;
            audioSource.Play();
        }
    }

    // 소리 정지를 편하게 하기 위해 만든 함수
    void StopFootstepSound()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }

    void GameOver()
    {
        isGameOver = true;
        
        // 게임 오버 시 소리가 나고 있다면 끕니다
        if (audioSource != null) audioSource.Stop();

        // 여기에 게임 오버 UI 호출이나 씬 재시작 로직 추가
        Debug.Log("<color=red>Game Over! 엄마한테 들켰습니다.</color>");
    }
}
