using System;
using UnityEngine;

public class Collections : MonoBehaviour
{   
    
    private void Awake()
    {
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                TalkText.Instance.talkConut++;
                gameObject.SetActive(false);
            }
        }
    }
}