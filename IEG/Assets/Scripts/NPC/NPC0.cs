using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class NPC0 : MonoBehaviour
{
    [SerializeField] private GameObject talkText;
    private bool talk = false; 
    private PlayerControl playerControl;
    
    private void Awake()
    {
        talkText.SetActive(false);
        playerControl = GetComponent<PlayerControl>();
    }

    private void Update()
    {
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            talk = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {   
        //Debug.Log("Enter");
        if (talk && other.CompareTag("Player"))
        {     
            Debug.Log("Start");
            talkText.SetActive(true);
            TalkText.Instance.LoadDialogue();
            talk = false;
        }
    }
    
}
