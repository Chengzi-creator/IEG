using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Collections : MonoBehaviour
{   
    
    private void Awake()
    {
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        TalkText.Instance.Count++;
        gameObject.SetActive(false);
    }
}