using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BoardText : MonoBehaviour
{
    public static BoardText instance;

    public static BoardText Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindAnyObjectByType<BoardText>();
            }
            return instance;
        }
    }
    
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Button nextButton;
    
    
    [Header("对话显示")]
    private Queue<string> dialogueQueue; //存储对话
    private bool isTyping = false;  //判断是否正在显示
    private Coroutine typingCoroutine;
    
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
        nextButton.onClick.AddListener(DisplayNextSentence); //显示下一句
        nextButton.gameObject.SetActive(false);
    }
    
    
    public void LoadDialogue()
    {
        switch (0)
        {
            case 0:
                StartDialogue(new List<string>
                {
                   "正如你所做的一样,你所拥有的能力可以被用于探索未知.",
                   "欣赏沿路的风景,在适时的地方停驻下来.",
                   "但并不是所有能力拥有了就一定要去使用它,",
                   "如果必须使用的话,我想还是不曾拥有的好.",
                   "你问我怎么避免使用这个能力?",
                   "我想...总是会有其他的路可以走的吧."
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
    }
}
