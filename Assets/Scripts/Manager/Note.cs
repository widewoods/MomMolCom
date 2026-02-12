using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public float noteSpeed = 3f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.isRhythmGamePaused) return;
        transform.localPosition += Vector3.down * noteSpeed * Time.deltaTime;
    }
}
