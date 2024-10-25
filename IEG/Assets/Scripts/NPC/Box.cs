using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public ClimbText climbTextText;
    
    private void Update()
    {
        climbTextText = FindObjectOfType<ClimbText>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {   
        if (other.CompareTag("Player"))
        {   
            Debug.Log("get");
            climbTextText.talkConut += 1;
            Destroy(gameObject);
        }
    }
}
