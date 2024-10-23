using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class BaseNPC : MonoBehaviour
{
    //存储不同对话内容
    public string[] initialDialogue = { "你好，勇士！", "我有个任务给你。" };
    public string[] missionInProgressDialogue = { "任务还没完成哦。" };
    public string[] missionCompletedDialogue = { "干得漂亮！任务完成了。" };

    public bool isMissionActive = false;//任务激活
    public bool isMissionCompleted = false;  //任务完成

    private void Start()
    {
        
    }


    //获取当前对话
    public string[] GetCurrentDialogue()
    {
        if (isMissionCompleted)
            return missionCompletedDialogue;
        else if (isMissionActive)
            return missionInProgressDialogue;
        else
            return initialDialogue;
    }
}
