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
    public int talkConut = 0;//用来存储对话次数？
    private PlayerControl playerControl;
    
    [Header("对话显示")]
    private Queue<string> dialogueQueue; //存储对话
    private bool isTyping = false;  //判断是否正在显示
    private Coroutine typingCoroutine;

    public int Count = 0;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        dialogueQueue = new Queue<string>();
        nextButton.onClick.AddListener(DisplayNextSentence);//显示下一句
        nextButton.gameObject.SetActive(false);
    }
    
    public void LoadDialogue()
    {
        switch (talkConut)
        {
            case 0:
                StartDialogue(new List<string>
                {
                    "你来了?看来你已经不记得了——这原本是你的一部分.",
                    "跳跃,不是为了逃离,而是为了超越.",
                    "你需要再学会这项能力.不过,在此之前,你得帮我完成一件事.",
                    "去那边的断路.那里有一些蓝光,它们像是我们曾经的希望.收集它们.或许这次你能跳得更高.",
                    "记得收集完了再来找我,我还有话想跟你说.",
                    "(沉默片刻)但是...跳的再高又有什么意义呢."
                });
                break;

            case 1:
                StartDialogue(new List<string>
                {
                    "很好,你做到了.曾经的希望又回到了你的身上.",
                    "但还是希望你不要忘了,飞得更高,也意味着跌得更重.",
                    "希望你能做出正确的选择...",
                    "再见.愿你在后面的路上,找到自己真正想要的.",
                    "(现在你可以进行二段跳了)"
                });
                break;

            default:
                //Debug.Log("没有更多的对话。");
                break;
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

        if (Count == 2)
        {
            talkConut = 1;
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
        //gameObject.SetActive(false);
        
        if (talkConut == 0)
        {
            isMissionActive = true;//暂时不知道这个的意义是
        }
        
        if (talkConut == 1)
        {
            PlayerControl.Instance.powerCount++;
            gameObject.SetActive(false);
        }
    }
}
