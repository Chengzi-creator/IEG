using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClimbText : MonoBehaviour
{
    public static ClimbText instance;

    public static ClimbText Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindAnyObjectByType<ClimbText>();
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
        nextButton.onClick.AddListener(DisplayNextSentence); //显示下一句
        nextButton.gameObject.SetActive(false);
    }
    
    
    public void LoadDialogue()
    {
        switch (talkConut)
        {
            case 0:
                StartDialogue(new List<string>
                {
                    "哦...你还是来了.你还记得吗?我们曾无数次地想要攀登更高的地方.",
                    "高处并没有什么特别的,只是站在那里,你以为能看得更清楚.",
                    "这次,我想让你帮我完成这个执念.对面的峭壁,我想让你替我登顶.",
                    "爬得高并不意味着得到更多,也许...一些高度,永远不值得去征服.",
                    "希望还能再看到你!"
                });
                break;

            case 1:
                StartDialogue(new List<string>
                {
                    "你成功了.可是...站在这里,你看到了什么?",
                    "你觉得这就是结束吗?不,那只是另一个新的起点.",
                    "我们这些执念,未曾真正带来什么,反而让人更加迷失.",
                    "希望你能在后面的路上,找到真正的意义...",
                    "(现在你可以在贴墙的时候按Q进入爬墙姿态了)"
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
        
        if (talkConut == 0)
        {
            isMissionActive = true;
        }
        
        if (talkConut == 1)
        {
            PlayerControl.Instance.powerCount++;
            gameObject.SetActive(false);
        }
    }
}
