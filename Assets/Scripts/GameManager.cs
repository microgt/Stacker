using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    ScoreScript sc;
    private SpawnnerScript[] spawnners;
    private SpawnnerScript currentSpawnner;
    private int index = 0;
    GameObject cam;
    Vector3 newCameraYPosition;

    void Awake(){
        cam = Camera.main.gameObject;
        newCameraYPosition = new Vector3(cam.transform.position.x, 2.93f, cam.transform.position.z);
        spawnners = FindObjectsOfType<SpawnnerScript>();
        sc = GameObject.FindObjectOfType<ScoreScript>();
    }
    void Update()
    {
        if(Input.GetButtonDown("Fire1")){

            if(MovingCubeScript.currentCube != null) {
                MovingCubeScript.currentCube.Stop();
                if(MovingCubeScript.currentCube != null && MovingCubeScript.currentCube.gameObject != GameObject.FindWithTag("Platform")) newCameraYPosition = new Vector3(cam.transform.position.x, cam.transform.position.y + MovingCubeScript.currentCube.transform.localScale.y /2,cam.transform.position.z);
            }
            
            index = index == 0? 1 : 0;
            currentSpawnner = spawnners[index];
            currentSpawnner.Spawn();
            sc.onCubeSpawnned();
        }
        cam.transform.position = Vector3.Lerp(cam.transform.position, newCameraYPosition, 1 * Time.deltaTime);
    }
}
