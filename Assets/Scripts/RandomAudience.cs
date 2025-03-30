using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomAudience : MonoBehaviour
{
    [SerializeField] Sprite[] sp;
    
    void Start()
    {
        GetChildren(this.gameObject);
    }

    void GetChildren(GameObject obj)
    {
        foreach (Transform child in obj.transform) {
            if(child.name == "Square") child.gameObject.GetComponent<SpriteRenderer>().sprite = sp[Random.Range(0, sp.Length)];
            else GetChildren(child.gameObject);
        }
    }
}
