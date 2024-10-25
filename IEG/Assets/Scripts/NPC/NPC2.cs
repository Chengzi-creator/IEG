using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
    
public class NPC2 : MonoBehaviour
{
    [SerializeField] private GameObject attackText;
    private bool talk = false; 
    private PlayerControl playerControl;
    
    private void Awake()
    {
        attackText.SetActive(false);
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
            attackText.SetActive(true);
            AttackText.Instance.LoadDialogue();
            talk = false;
        }
    }
}
