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

    private float leavingDuration = 2.0f;  // ���� �� ���ڱ� �Ҹ� �鸮�� �ð�

    public AudioSource audioSource;      // ���ڱ� �Ҹ��� �� ����� �ҽ� ������Ʈ
    public List<AudioClip> footstepsClips;      // ���ڱ� MP3 ����
    [SerializeField] private SoundContainer soundContainer;

    //public GameObject momObject;         // ���� ���� ������Ʈ (�湮 ��)

    // ��Ʈ�� ���� (�ܺ� ��ũ��Ʈ���� �� ������ �����ϰų�, ������Ƽ�� �����ؾ� ��)
    public PlayerContorller3D laptop;

    public bool isGameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        //if (momObject != null) momObject.SetActive(false);

        // ������ ���� ���� ����
        StartCoroutine(MomPatternRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ���� �ൿ ���� �ڷ�ƾ
    IEnumerator MomPatternRoutine()
    {
        while (!isGameOver)
        {
            // 1. ������ �ð���ŭ ��� (��ȭ�ο� �ð�)
            float randomWait = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(randomWait);

            // 2. ���ڱ� �Ҹ� ��� (��� �ܰ�)
            Debug.Log("���ڱ� �Ҹ��� �鸲");

            AudioClip footstepsClip = footstepsClips[Random.Range(0, footstepsClips.Count)];
            PlayFootstepSound(footstepsClip); // �Ҹ� ��� �Լ� ȣ��

            // 3. N��(warningDuration) ��ŭ ��� (�÷��̾ ��Ʈ���� ���� �ð�)
            float warningDuration = Random.Range(minWarningDuration, maxWarningDuration);
            yield return new WaitForSeconds(warningDuration);

            // 4. ���� ���� �� ��Ʈ�� Ȯ��
            StopFootstepSound();

            Debug.Log("���� ����");

            float timer = 0f;
            bool caught = false;

            float momStayDuration = Random.Range(minMomStayDuration, maxMomStayDuration);
            while (timer < momStayDuration)
            {
                // �ǽð����� ��Ʈ���� �����ִ��� üũ
                if (laptop.openLaptop)
                {
                    Debug.Log("�������� �ɸ�");
                    GameOver();
                    caught = true;
                    break; // ���� ���� Ż��
                }

                // ���� �����ӱ��� ����ϸ� �ð��� ���
                timer += Time.deltaTime;
                yield return null;
            }

            // ���� �ɷȴٸ� �� �̻� ������ �������� �ʰ� ����
            if (caught) yield break;

            Debug.Log("���� ����");

            PlayFootstepSound(footstepsClip); // �ٽ� ���ڱ� �Ҹ� ���

            // ������ ���ڱ� �Ҹ��� �󸶳� ������� (��: 2��)
            yield return new WaitForSeconds(leavingDuration);

            StopFootstepSound();
        }
    }

    // �Ҹ� ����� ���ϰ� �ϱ� ���� ���� �Լ�
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

    // �Ҹ� ������ ���ϰ� �ϱ� ���� ���� �Լ�
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
        
        // ���� ���� �� �Ҹ��� ���� �ִٸ� ���ϴ�
        if (audioSource != null) audioSource.Stop();

        // ���⿡ ���� ���� UI ȣ���̳� �� ����� ���� �߰�
        Debug.Log("<color=red>Game Over! �������� ���׽��ϴ�.</color>");
    }
}
