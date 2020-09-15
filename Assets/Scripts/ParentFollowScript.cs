using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ParentFollowScript : MonoBehaviour
{
    Transform parent;
    TextMeshPro myText;
    int bonus;
    // Start is called before the first frame update
    private void Awake() {
        myText = GetComponent<TextMeshPro>();
    }
    public void Initialize(Transform p, int b)
    {
        parent = p;
        bonus = b;
        myText.text = b.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if(parent != null){
            transform.position = new Vector3(parent.transform.position.x, parent.transform.position.y + 0.6f, parent.transform.position.z);
        }
    }
    public int GetBonus(){
        return bonus;
    }
}
