using System;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class TalkText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueText;  //显示对话
    [SerializeField] private Button nextButton;  //右下角的继续按钮
    private Queue<string> dialogueQueue;  //存对话内容
    
    public string[] initialDialogue = { "你好，勇士！", "我有个任务给你。" };
    public string[] missionInProgressDialogue = { "任务还没完成哦。" };
    public string[] missionCompletedDialogue = { "干得漂亮！任务完成了。" };

    public static TalkText instance;
    public static TalkText Instance => instance;
    public bool isMissionActive = false;//任务激活
    public bool isMissionCompleted = false;  //任务完成

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
    
    private void Awake()
    {
       
    }


    void Start()
    {
        dialogueQueue = new Queue<string>();
        nextButton.onClick.AddListener(DisplayNextSentence);
        
        StartDialogue(new List<string>
        {
            "你好，勇士！欢迎来到这个世界。",
            "你需要完成任务才能继续冒险。",
            "加油，我相信你能做到！"//文本待定
        });
    }
    
    public void StartDialogue(List<string> dialogue)
    {
        dialogueQueue.Clear();

        foreach (string sentence in dialogue)
        {
            dialogueQueue.Enqueue(sentence);
        }

        DisplayNextSentence();
    }
    
    private void DisplayNextSentence()
    {
        if (dialogueQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = dialogueQueue.Dequeue();
        dialogueText.text = sentence;
    }
    
    private void EndDialogue()
    {
        dialogueText.text = "对话结束";
        nextButton.gameObject.SetActive(false);
    }    
}