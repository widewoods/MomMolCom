using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; // StringSplitOptions 등을 사용하기 위해 추가

public class NoteManager : MonoBehaviour
{
    public int bpm = 0;
    double currentTime = 0d;

    // 현재 체크 중인 텍스트 파일의 줄 번호 (인덱스 i)
    private int i = 0;

    // 이 스크립트가 담당하는 라인 번호 (0, 1, 2, 3 중 하나)
    public int line = 0;

    [SerializeField] Transform tfNoteAppear = null;
    [SerializeField] GameObject goNote = null;
    [SerializeField] private TextAsset mapFile;

    // 파싱된 맵 데이터를 저장할 리스트 [[0,0,0,0], [0,1,0,0]...]
    private List<int[]> mapData = new List<int[]>();

    TimingManager theTimingManager;

    void Start()
    {
        theTimingManager = GetComponent<TimingManager>();

        // 게임 시작 시 텍스트 파일을 미리 리스트화(파싱) 합니다.
        if (mapFile != null)
        {
            ParseMapData();
        }
    }

    void Update()
    {
        // 데이터가 없거나 모든 줄을 다 읽었으면 중단
        if (mapData.Count == 0 || i >= mapData.Count) return;

        if (PlayerController.isRhythmGamePaused) return;

        currentTime += Time.deltaTime;

        // 한 박자 시간이 되었을 때
        double tempo = 60d / bpm / 4;
        if (currentTime >= tempo)
        {
            // 핵심 로직: mapData[줄][라인] 값이 1인지 확인
            if (mapData[i][line] == 1)
            {
                GameObject t_note = Instantiate(goNote, tfNoteAppear.position, Quaternion.identity);
                theTimingManager.boxNoteList.Add(t_note);
            } else if (mapData[i][line] == 2) {
                // 만약 2라면 note는 생성하되, 보이지 않도록 생성하기
                GameObject t_note = Instantiate(goNote, tfNoteAppear.position, Quaternion.identity);
                t_note.GetComponent<SpriteRenderer>().enabled = false;
                //theTimingManager.boxNoteList.Add(t_note);
            }

                // 노트를 생성했든 안 했든 박자가 지났으므로 다음 줄(i)로 넘어감
                i++;
            currentTime -= tempo;
        }
    }

    // 텍스트 파일을 읽어서 List<int[]> 형태로 변환
    void ParseMapData()
    {
        // 줄바꿈 기준으로 텍스트 분할
        string[] lines = mapFile.text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

        foreach (string str in lines)
        {
            // 최소 4글자 이상일 때만 처리
            if (str.Length >= 4)
            {
                int[] row = new int[4];
                for (int j = 0; j < 4; j++)
                {
                    // 문자 '0' 또는 '1'을 정수로 변환
                    row[j] = str[j] - '0';
                }
                mapData.Add(row);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Note"))
        {
            if (theTimingManager.boxNoteList.Contains(collision.gameObject))
            {
                theTimingManager.boxNoteList.Remove(collision.gameObject);
            }
            Destroy(collision.gameObject);
            Debug.Log("Miss");
            if (ScoreManager.instance != null)
            {
                ScoreManager.instance.ProcessJudgement("Miss");
            }
        }
    }
}