using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TalkText : MonoBehaviour
{
    public static TalkText instance;

    public static TalkText Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindAnyObjectByType<TalkText>();
            }
            return instance;
        }
    }
    
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Button nextButton;
    
    [Header("对话更新")]
    public bool isMissionActive = false;
    public bool isMissionCompleted = false;
    public int talkConut = 0;//用来存储对话次数？
    private PlayerControl playerControl;
    
    [Header("对话显示")]
    private Queue<string> dialogueQueue; //存储对话
    private bool isTyping = false;  //判断是否正在显示
    private Coroutine typingCoroutine; 
    

    private void Awake()
    {
        //playerControl = GetComponent<PlayerControl>();
    }

    void Start()
    {
        dialogueQueue = new Queue<string>();
        nextButton.onClick.AddListener(DisplayNextSentence); //显示下一句
        nextButton.gameObject.SetActive(false);

        if (talkConut == 0)
        {
            StartDialogue(new List<string>
            {
                "你好,勇士!欢迎来到这个世界.",
                "你需要完成任务才能继续冒险.",
                "加油,我相信你能做到!"
            });
        }

        if (talkConut == 1)
        {
            StartDialogue(new List<string>
            {
                "","",""
            });
        }

        if (talkConut == 2)
        {
            
        }
    }
    

    //初始化,存储对话内容
    public void StartDialogue(List<string> dialogue)
    {
        dialogueQueue.Clear();

        foreach (string sentence in dialogue)
        {
            dialogueQueue.Enqueue(sentence);
        }

        DisplayNextSentence();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isTyping)
        {
            DisplayNextSentence();
        }

        if (isMissionCompleted)
        {
            
        }
    }


    private void DisplayNextSentence()
    {
        if (dialogueQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = dialogueQueue.Dequeue();
        
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeSentence(sentence));
    }
    
    private IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter; 
            yield return new WaitForSeconds(0.05f);
        }

        isTyping = false;
    }
    
    private void EndDialogue()
    {
        dialogueText.text = "";
        nextButton.gameObject.SetActive(false);
        gameObject.SetActive(false);
        
        if (talkConut == 0)
        {
            isMissionActive = true;
        }
        
        if (talkConut == 1)
        {
            PlayerControl.Instance.powerCount++;
            
        }
    }
}
