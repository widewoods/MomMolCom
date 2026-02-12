using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContorller3D : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;

    private float turnSpeed = 10.0f;
    private float turnAngle = -90.0f;

    private Quaternion originalRotation;
    private Quaternion targetRotation;

    [SerializeField] private Animator laptopAnimator;
    public bool openLaptop = true;

    public Animator handAnimator;
    [SerializeField] private Transform lhandTransform;
    [SerializeField] private Transform rhandTransform;

    private Quaternion originHandLocalRotation;

    void Start()
    {
        originalRotation = cameraTransform.rotation;
        targetRotation = originalRotation * Quaternion.Euler(0, turnAngle, 0);
        originHandLocalRotation = rhandTransform.localRotation;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, targetRotation, Time.deltaTime * turnSpeed);
        }
        else
        {
            cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, originalRotation, Time.deltaTime * turnSpeed);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleLaptop();
        }
    }

    void ToggleLaptop()
    {
        openLaptop = !openLaptop;

        if (laptopAnimator != null)
        {
            laptopAnimator.SetBool("isOpen", openLaptop);
            handAnimator.enabled = openLaptop;
        }

        // duration은 짧은 시간(0.1초)이므로 Lerp 방식이 필수적입니다.
        float duration = 0.4f;
        
        // count 파라미터는 더 이상 필요 없으므로 제거했습니다.
        if (openLaptop)
        {
            StartCoroutine(lhandMove(duration, -1));
            StartCoroutine(rhandMove(duration, -1));
        }
        else
        {
            StartCoroutine(lhandMove(duration, 1));
            StartCoroutine(rhandMove(duration, 1));
        }
    }

    // 왼손 이동: 시간을 기반으로 움직이도록 수정
    IEnumerator lhandMove(float duration, int dir)
    {
        Vector3 startPos = lhandTransform.position;
        // 최종 목표 위치 계산 (기존 로직: 0.3f 이동)
        Vector3 targetPos = startPos + (lhandTransform.up * 0.3f * dir);

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            // 0~1 사이의 진행률 계산
            float t = elapsed / duration;
            
            // 현재 진행률에 맞춰 위치 보간
            lhandTransform.position = Vector3.Lerp(startPos, targetPos, t);
            
            yield return null; // 다음 프레임까지 대기
        }

        // 루프가 끝난 후 위치를 정확히 목표점으로 설정
        lhandTransform.position = targetPos;
    }

    // 오른손 이동: 복합적인 움직임도 시간 기반으로 수정
    IEnumerator rhandMove(float duration, int dir)
    {
        rhandTransform.localRotation = originHandLocalRotation;

        float withoutRatio = 0.3f; // 전체 시간 중 40%
        float withRatio = 1.0f - withoutRatio; // 전체 시간 중 60%

        // 이동 로직을 재사용하기 위한 내부 함수
        // startPosition에서 delta만큼 time 동안 이동
        IEnumerator MoveDelta(Vector3 delta, float time)
        {
            Vector3 startPos = rhandTransform.position;
            Vector3 endPos = startPos + delta;
            float elapsed = 0f;

            while (elapsed < time)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / time;
                rhandTransform.position = Vector3.Lerp(startPos, endPos, t);
                yield return null;
            }
            rhandTransform.position = endPos;
        }

        // 기존 로직의 벡터 계산
        // moveWithoutLaptop 방향 벡터 (dir 적용 전)
        Vector3 moveWithoutVector = (Vector3.up * 0.45f + Vector3.right * 0.25f);
        Vector3 moveWithVector = (Vector3.down * 0.45f + Vector3.left * 0.4f);
        
        // dir에 따라 방향 결정
        // 기존 코드: moveWithout은 dir 그대로, moveWith는 -1 * dir
        
        if (dir == 1) // 닫을 때? (기존 로직 따름)
        {
            yield return StartCoroutine(MoveDelta(moveWithoutVector * dir, duration * withoutRatio));
            
            yield return StartCoroutine(MoveDelta(moveWithVector * dir, duration * withRatio));
        }
        else // 열 때?
        {
            yield return StartCoroutine(MoveDelta(moveWithVector * dir, duration * withRatio));

            yield return StartCoroutine(MoveDelta(moveWithoutVector * dir, duration * withoutRatio));
        }
    }
}