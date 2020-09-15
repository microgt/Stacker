using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCubeScript : MonoBehaviour
{
    public static MovingCubeScript currentCube {get; private set;}
    public static MovingCubeScript lastCube {get; private set;}
    public MoveDirection MoveDirection { get; internal set; }

    [SerializeField]
    float speed = 1;
    float startingAxis;
    float currentAxis;
    [SerializeField]
    GameObject bonusFloat;
    [SerializeField]
    int[] bonuses;
    int bonusAmount;
    // Start is called before the first frame update
    void OnEnable()
    {
        if(lastCube == null) {
            lastCube = GameObject.FindWithTag("Platform").GetComponent<MovingCubeScript>();
        }

        currentCube = this; 
    
        if(this.gameObject != GameObject.FindWithTag("Platform")) GetComponent<Renderer>().material.color = new Color(Random.Range(0,1f), Random.Range(0,1f), Random.Range(0,1f));
        transform.localScale = new Vector3(lastCube.transform.localScale.x, transform.localScale.y, lastCube.transform.localScale.z);

         if(Random.value > 0.8){
            StartCoroutine("spawnBonus");
         }

        //startingAxis = MoveDirection == MoveDirection.z? transform.position.z : transform.position.x;
        startingAxis = 2.58f;
    }
    IEnumerator spawnBonus(){
            yield return 0;
            bonusAmount = bonuses[Random.Range(0, bonuses.Length-1)];
            GameObject pfs = Instantiate(bonusFloat, transform.position, Quaternion.identity);
            pfs.GetComponent<ParentFollowScript>().Initialize(transform, bonusAmount);
    }
    public int GetBonus(){
        if(bonusAmount > 0){
        int temp = bonusAmount;
        if(GameObject.FindObjectOfType<ParentFollowScript>() != null) Destroy (GameObject.FindObjectOfType<ParentFollowScript>().gameObject);
        bonusAmount = 0;
        return temp;}else{return 0;}
    }
    void DestroyBonus(){

    }

    void SplitOnX(float amount, float dir){
        if(lastCube == null) return;

        float newXSize = lastCube.transform.localScale.x - Mathf.Abs(amount);
        float fallingBlockSize = transform.localScale.x - newXSize;
        float newXPosition = lastCube.transform.position.x + amount/2;

        transform.localScale = new Vector3(newXSize, transform.localScale.y, transform.localScale.z);
        transform.position = new Vector3(newXPosition, transform.position.y, transform.localPosition.z);

        float cubeEdge = transform.position.x + (newXSize/2 * dir);
        float fallingXPosition = cubeEdge + (fallingBlockSize/2 * dir);

        SpawnPiece(fallingXPosition, fallingBlockSize);
    }
    void SplitOnZ(float amount, float dir){
        if(lastCube == null) return;

        float newZSize = lastCube.transform.localScale.z - Mathf.Abs(amount);
        float fallingBlockSize = transform.localScale.z - newZSize;
        float newZPosition = lastCube.transform.position.z + amount/2;

        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, newZSize);
        transform.position = new Vector3(transform.position.x, transform.position.y, newZPosition);

        float cubeEdge = transform.position.z + (newZSize/2 * dir);
        float fallingZPosition = cubeEdge + (fallingBlockSize/2 * dir);
        SpawnPiece(fallingZPosition, fallingBlockSize);
    }
    void SpawnPiece(float pos, float size){
        if(currentCube.gameObject == GameObject.FindWithTag("Platform")) return;

        GameObject newCube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        if(MoveDirection == MoveDirection.z){
            newCube.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, size);
            newCube.transform.position = new Vector3(transform.position.x, transform.position.y, pos);
        }else{
            newCube.transform.localScale = new Vector3(size, transform.localScale.y, transform.localScale.z);
            newCube.transform.position = new Vector3(pos, transform.position.y, transform.position.z);
        }
 
        newCube.AddComponent<Rigidbody>();
        newCube.GetComponent<Renderer>().material.color = GetComponent<Renderer>().material.color;
        Destroy(newCube.gameObject, 2);
    }

    // Update is called once per frame
    void Update()
    {
        if(MoveDirection == MoveDirection.z){
            transform.position += (transform.forward * -1) * Time.deltaTime * speed;
        }else{
            transform.position += (transform.right * -1) * Time.deltaTime * speed;
        }

        currentAxis = MoveDirection == MoveDirection.z? transform.position.z : transform.position.x;
        if(Mathf.Abs(currentAxis) > Mathf.Abs(startingAxis)){
            speed = speed * -1;
            GetBonus();
        }
    }

    internal void Stop()
    {
        speed = 0;
        float hangover = getHangover();
        float max = MoveDirection == MoveDirection.z? lastCube.transform.localScale.z : lastCube.transform.localScale.x;

        if (Mathf.Abs(hangover) >= max)
        {
            lastCube = null;
            currentCube = null;
            GameObject.FindObjectOfType<ScoreScript>().GameOver();
        }
        float direction = hangover > 0 ? 1 : -1f;
        if (MoveDirection == MoveDirection.z)
        {
            SplitOnZ(hangover, direction);
        }
        else
        {
            SplitOnX(hangover, direction);
        }
        lastCube = this;
    }

    private float getHangover()
    {
        if(MoveDirection == MoveDirection.z){
            return transform.position.z - lastCube.transform.position.z;
        }else{
            return transform.position.x - lastCube.transform.position.x;
        }
    }
}
