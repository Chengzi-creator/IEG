using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float mSpeed;
    [SerializeField] private float mJumpForce;
    public TextMeshProUGUI dialogueText;  //显示对话内容的 UI
    private TalkText currentTalkText;  //当前交互的 NPC的文本
    private bool isInRange = false;  //玩家是否在交互范围内

    
    private Rigidbody rb;
    private bool CanJumpTwice;
    private bool isGround;
    private int talkCount = 0;
    
    private void Awake()
    {
        transform.position = new Vector3(0, 0, 0);
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        Move();
        Jump();

        if (isInRange && Input.GetKeyDown(KeyCode.E))
        {
            TriggerDialogue();
        }
    }

    #region 角色控制
    private void Move()
    {
        float direction = Input.GetAxis("Horizontal");
        Vector2 position = transform.position + new Vector3(direction * mSpeed, 0 ,0) * Time.deltaTime;
    }

    private void Jump()
    {
        if (isGround)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(mJumpForce * Vector3.up, ForceMode.Impulse);
            }
        }

        if (CanJumpTwice)
        {
            if (!isGround)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    rb.AddForce(mJumpForce * Vector3.up, ForceMode.Impulse);
                }
            }
        }
    }
    
    private void JumpTwice()
    {   
        if(talkCount ==1)
            CanJumpTwice = true;
    }

    private void AttckLock()
    {
        
    }
    #endregion


    #region 角色交互
    //当玩家进入交互范围时
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("NPC"))
        {
            currentTalkText = other.GetComponent<TalkText>();
            isInRange = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("NPC"))
        {
            currentTalkText = null;
            isInRange = false;
            dialogueText.text = "";  //清空对话
        }
    }


    private void TriggerDialogue()
    {
        if (currentTalkText != null)
        {
            string[] dialogue = currentTalkText.GetCurrentDialogue();
            ShowDialogue(dialogue);

            //如果任务未激活，则激活任务
            if (!currentTalkText.isMissionActive)
            {
                currentTalkText.isMissionActive = true;
                //Debug.Log("任务已激活！");
            }
        }
    }

    // 显示对话内容
    private void ShowDialogue(string[] dialogue)
    {
        dialogueText.text = string.Join("\n", dialogue);  //将数组中的对话内容拼接为一段文本
    }
    

    // 模拟任务完成，用于测试
    public void CompleteMission()
    {
        if (currentTalkText != null && currentTalkText.isMissionActive)
        {
            currentTalkText.isMissionCompleted = true;
            Debug.Log("任务完成！");
        }
    }
    #endregion
    
}