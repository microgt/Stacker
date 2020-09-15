using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreScript : MonoBehaviour
{
    private int score = 0;
    int originalMultiplierSize = 75;
    float animationSpeed = 4;
    float multiplier = -1;
    float defaultMultiplierTimer = 3;
    float multiplierTimer = 3;
    Text text;
    [SerializeField]
    Image scoreMultiplier;
    [SerializeField]
    Text smulti;
    [SerializeField]
    Color circleStartColor;
    [SerializeField]
    Color circleEndColor;
    [SerializeField]
    Color multiplierFontStartColor;
    [SerializeField]
    Text timerText;
    [SerializeField]
    float timer = 15;
    float timeBonusProbe = 0;
    bool hasGameStarted = false;
    bool isGameOver = false;
    AudioSource asource;
    [SerializeField]
    AudioClip stackSFX;
    [SerializeField]
    AudioClip loseComboSFX;
    [SerializeField]
    AudioClip bonusSFX;
    [SerializeField]
    AudioClip startSFX;
    [SerializeField]
    AudioClip timePlusSFX;
    [SerializeField]
    AudioClip gameOverSFX;
    [SerializeField]
    AudioClip gameWonSFX;
    int bonus;
    [SerializeField]
    bool resetScore = false;

    // Start is called before the first frame update
    void Start()
    {
        if(resetScore) PlayerPrefs.SetInt("score", 0);
        text = GetComponent<Text>();
        asource = GetComponent<AudioSource>();
    }
    public void onCubeSpawnned(){
        if(!hasGameStarted && !isGameOver) hasGameStarted = true;
        if(hasGameStarted && !isGameOver){
        transform.GetChild(0).GetComponent<Text>().text = "";
        multiplier++;
        float tempPitch = asource.pitch;
        if(GameObject.FindObjectOfType<ParentFollowScript>() != null){
            bonus = GameObject.FindObjectOfType<ParentFollowScript>().GetBonus();
            Destroy(GameObject.FindObjectOfType<ParentFollowScript>().gameObject);
            //asource.PlayOneShot(stackSFX);
            tempPitch = asource.pitch;
            asource.pitch = 1;
            asource.clip = bonusSFX;
            asource.Play();
            asource.pitch = tempPitch;
        }else if((score + (int)multiplier + bonus) > 0){
            bonus = 0;
            asource.clip = stackSFX;
            asource.Play();
        }else{
            asource.clip = startSFX;
            asource.Play();
        }
        score += (int)multiplier + bonus;
        timeBonusProbe += (int)multiplier + bonus;
        text.text = score.ToString();
        text.GetComponent<Animator>().Play("anim", 0);
        asource.pitch = asource.pitch + 0.01f;
        
        if(multiplier > 1){   
            multiplierTimer = defaultMultiplierTimer;
            smulti.fontSize = originalMultiplierSize;
            scoreMultiplier.fillAmount = 1;
            scoreMultiplier.color = circleStartColor;
            smulti.color = new Color(smulti.color.r + (multiplier)/100, smulti.color.g - (multiplier)/100, smulti.color.b - (multiplier)/100, smulti.color.a);
        }
        if(timeBonusProbe >= 25){
            timer += 5;
            timerText.GetComponent<Animator>().Play("anim", 0);
            tempPitch = asource.pitch;
            asource.pitch = 1;
            asource.PlayOneShot(timePlusSFX);
            asource.pitch = tempPitch;
            timeBonusProbe = 0;
        }
        }
    }

    void Update(){

        if(hasGameStarted && isGameOver && Input.GetButtonDown("Fire1") && GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Default State")) ResetGame();

        if(hasGameStarted && !isGameOver){
        if(multiplier > 1){     
                showMultiplier();

                multiplierTimer -= Time.deltaTime;
                scoreMultiplier.fillAmount = ((multiplierTimer * 100)/ defaultMultiplierTimer) /100;
                smulti.fontSize = (int)Mathf.Lerp(smulti.fontSize, 45, Time.deltaTime / defaultMultiplierTimer);
                scoreMultiplier.color = Color.Lerp(scoreMultiplier.color, circleEndColor, Time.deltaTime / (defaultMultiplierTimer/2));
            }else
            {
                hideMultiplier();
            }

            if(multiplierTimer <= 0 && multiplier > 1){
                multiplier = 0;
                asource.pitch = 1;
                asource.PlayOneShot(loseComboSFX);
                smulti.color = multiplierFontStartColor;
            }

            timer -= Time.deltaTime;
                if(timer > 60){
                    float minutes = timer / 60;
                    float seconds = timer % 60;
                    string mins = (minutes >= 10)? Mathf.Floor(minutes).ToString() : "0" + Mathf.Floor(minutes);
                    string secs = (seconds >= 10)? Mathf.Floor(seconds).ToString() : "0" + Mathf.Floor(seconds);
                    timerText.text = mins + ":" + secs;
                }else{
                    string secs = (timer >= 10)? Mathf.Floor(timer).ToString() : "0" + Mathf.Floor(timer);
                    timerText.text = "00:" + secs;
                }
                if(timer <= 0){
                     if(MovingCubeScript.currentCube != null) {
                        MovingCubeScript.currentCube.Stop();
                    }
                }
        }
    }
    public int GetTimer(){
        return (int)timer;
    }
    void showMultiplier(){
        smulti.text = (multiplier+1) + "x";
    }
    void hideMultiplier(){
        smulti.text = "";
    }
    public void GameOver(){
        isGameOver = true;
        hideMultiplier();
        scoreMultiplier.fillAmount = 0;
        timerText.text = "";

        if(GameObject.FindObjectOfType<ParentFollowScript>() != null){
            Destroy(GameObject.FindObjectOfType<ParentFollowScript>().gameObject);
        }

        int highestScore = PlayerPrefs.GetInt("score");
        if(score > highestScore){
            PlayerPrefs.SetInt("score", score);
        }
        string line2 = (score > highestScore)? "\nHighestScore: " + score : "\nHighestScore: " + highestScore;
        string line3 = (score > highestScore)? "\nNew High Score!" : "";

        if(score > highestScore) transform.GetChild(0).GetComponent<Animator>().Play("win", 0);

        text.text = "Game Over";
        transform.GetChild(0).GetComponent<Text>().text = "Score: " + score + line2 + line3;

        GetComponent<Animator>().Play("end", 0);

        AudioSource musicSource = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();
        musicSource.loop = false;
        musicSource.clip = (score > highestScore)? gameWonSFX : gameOverSFX;
        musicSource.Play();

    }
    void ResetGame(){
        SceneManager.LoadScene(0);
    }

    public bool isTheGameOver(){
        return isGameOver;
    }
}
