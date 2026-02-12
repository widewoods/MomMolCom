using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class MomController : MonoBehaviour
{
    private float minSpawnTime = 3f;
    private float maxSpawnTime = 5f;
 
    private float minWarningDuration = 2f;
    private float maxWarningDuration = 4f;
 
    private float minMomStayDuration = 1f;
    private float maxMomStayDuration = 4f; 

    private float leavingDuration = 2.0f;

    public AudioSource audioSource;
    public List<AudioClip> footstepsClips;
    [SerializeField] private SoundContainer soundContainer;

    [SerializeField] private GameObject momObject;
    [SerializeField] private Transform start;
    [SerializeField] private Transform middle;
    [SerializeField] private Transform end;

    [SerializeField] private GameObject momHead;
    [SerializeField] private Transform lookOrigin;
    [SerializeField] private Transform lookTarget;
    [SerializeField] private Animator momAnimator;

    [SerializeField] private PlayerContorller3D laptop;

    public bool isGameOver = false;
    public System.Action OnGameOver;

    // Start is called before the first frame update
    void Start()
    {
        // if (momObject != null) momObject.SetActive(false);

        StartCoroutine(MomPatternRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator MomPatternRoutine()
    {
        while (!isGameOver)
        {
            float randomWait = Random.Range(minSpawnTime, maxSpawnTime);
            float warningDuration = Random.Range(minWarningDuration, maxWarningDuration);
            float momStayDuration = Random.Range(minMomStayDuration, maxMomStayDuration);
            yield return new WaitForSeconds(randomWait);

            Debug.Log("Mom Start");
            StartCoroutine(MomComeAndArrive(warningDuration, momStayDuration));

            AudioClip footstepsClip = footstepsClips[Random.Range(0, footstepsClips.Count)];
            PlayFootstepSound(footstepsClip);

            yield return new WaitForSeconds(warningDuration);

            StopFootstepSound();

            Debug.Log("Mom Arrive");

            float timer = 0f;
            bool caught = false;

            while (timer < momStayDuration)
            {
                if (laptop.openLaptop)
                {
                    Debug.Log("You Caught by Mom!");
                    GameOver();
                    caught = true;
                    break;
                }

                timer += Time.deltaTime;
                yield return null;
            }

            if (caught) yield break;

            Debug.Log("Mom Leaving");

            PlayFootstepSound(footstepsClip); 

            yield return new WaitForSeconds(leavingDuration);

            StopFootstepSound();
        }
    }

    IEnumerator MomComeAndArrive(float comeDuration, float stayDuration)
    {
        float middleToEnd = 1f;
        float startToMiddle = comeDuration - middleToEnd;
        // Move Mom from start to middle
        yield return MomMove(startToMiddle, start, middle);

        // Move Mom from middle to end
        yield return MomMove(middleToEnd, middle, end);

        // Wait for stay duration
        // Mom 애니메이션 멈추기
        momAnimator.enabled = false;
        StartCoroutine(MomLook(lookOrigin, lookTarget, 0.5f));
        yield return new WaitForSeconds(stayDuration-0.5f);
        yield return StartCoroutine(MomLook(lookTarget, lookOrigin, 0.5f));
        momAnimator.enabled = true;

        float endToMiddle = 1f;
        // Move Mom from end to middle
        yield return MomMove(endToMiddle, end, middle);

        // Move Mom from middle to start
        yield return MomMove(leavingDuration-endToMiddle, middle, start);
    }
    
    IEnumerator MomMove(float duration, Transform from, Transform to)
    {
        float timer = 0f;
        while (timer < duration)
        {
            momObject.transform.position = Vector3.Lerp(from.position, to.position, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }
        momObject.transform.position = to.position;
    }

    IEnumerator MomLook(Transform from, Transform to, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            momHead.transform.position = Vector3.Lerp(from.position, to.position, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }
    }

    void PlayFootstepSound(AudioClip footstepsClip)
    {
        if (audioSource != null && footstepsClips.Count > 0)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.loop = true;
            audioSource.clip = footstepsClip;
            audioSource.volume = soundContainer.audioSounds[0].volume;
            audioSource.Play();
        }
    }

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
        OnGameOver?.Invoke();
        
        if (audioSource != null) audioSource.Stop();

        Debug.Log("<color=red>Game Over! �������� ���׽��ϴ�.</color>");
    }
}
