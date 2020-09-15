using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnnerScript : MonoBehaviour
{
    [SerializeField]
    private MovingCubeScript cubeTemplate;
    [SerializeField]
    private MoveDirection moveDirection;
    // Start is called before the first frame update
    // Update is called once per frame
    public void Spawn()
    {
        if(GameObject.FindObjectOfType<ScoreScript>().isTheGameOver()) return;
        MovingCubeScript cube = Instantiate(cubeTemplate);
        if(MovingCubeScript.lastCube != null && MovingCubeScript.lastCube.gameObject != GameObject.FindWithTag("Platform")){
            float x = moveDirection == MoveDirection.x? transform.position.x : MovingCubeScript.lastCube.transform.position.x;
            float z = moveDirection == MoveDirection.z? transform.position.z : MovingCubeScript.lastCube.transform.position.z;
            float yPos = MovingCubeScript.lastCube.transform.position.y + cubeTemplate.transform.localScale.y;

            cube.transform.position = new Vector3(x, yPos, z);
        }else{
            cube.transform.position = transform.position;
        }
        cube.MoveDirection = moveDirection;
    }
}
