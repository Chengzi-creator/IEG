using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Board : MonoBehaviour
{
    [SerializeField] private GameObject boardText;
    private bool talk = false; 
    
    private void Awake()
    {
        boardText.SetActive(false);
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
            //Debug.Log("Start");
            boardText.SetActive(true);
            BoardText.Instance.LoadDialogue();
            talk = false;
        }
    }
}
