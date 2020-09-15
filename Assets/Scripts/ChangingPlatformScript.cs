using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangingPlatformScript : MonoBehaviour
{
    [SerializeField]
    Material[] mats;
    // Start is called before the first frame update
    void Awake()
    {
        int rand = Random.Range(0, mats.Length-1);
        GetComponent<Renderer>().material = mats[rand];
    }
}
