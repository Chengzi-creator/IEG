using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class BaseNPC : MonoBehaviour
{
    //存储不同对话内容
    //public string[] initialDialogue = { "你好，勇士！", "我有个任务给你。" };
    //public string[] missionInProgressDialogue = { "任务还没完成哦。" };
    //public string[] missionCompletedDialogue = { "干得漂亮！任务完成了。" };

    public bool isMissionActive = false;
    public bool isMissionCompleted = false;
    
    //public TextMeshProUGUI dialogueText; 
    [SerializeField] private GameObject talkText;
    private bool isInRange = false; 
    private PlayerControl playerControl;
    
    private void Awake()
    {
        talkText.SetActive(false);
        playerControl = GetComponent<PlayerControl>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {   
        //Debug.Log("Enter");
        if (Input.GetKeyDown(KeyCode.E))
        {     
            Debug.Log("Start");
            talkText.SetActive(true);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        talkText.SetActive(false);
    }


    //获取当前对话
    // public string[] GetCurrentDialogue()
    // {
    //     if (isMissionCompleted)
    //         return missionCompletedDialogue;
    //     else if (isMissionActive)
    //         return missionInProgressDialogue;
    //     else
    //         return initialDialogue;
    // }
}
