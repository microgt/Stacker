using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerSFXScript : MonoBehaviour
{
    ScoreScript timerText;
    AudioSource asource;
    int timer;
    // Start is called before the first frame update
    void Awake()
    {
        timerText = GameObject.FindObjectOfType<ScoreScript>();
        asource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        timer = timerText.GetTimer();
        if(timer <= 5 && timer > 0){
            if(!asource.isPlaying)
            asource.Play();
        }else
        {
            asource.Stop();
        }
    }
}
