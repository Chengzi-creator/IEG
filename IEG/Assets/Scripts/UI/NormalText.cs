using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NormalText : MonoBehaviour
{   
    [SerializeField] private TextMeshProUGUI dialogueText;
    private Queue<string> dialogueQueue;
    private bool isTyping = false;
    private Coroutine typeCoroutine;
    public Transform character;
    public Vector3 offset = new Vector3(0, 2, 0);

    
    private void Awake()
    {
        
    }

    private void Start()
    {
        dialogueQueue = new Queue<string>();
        
        StartDialogue(new List<string>
        {
            "","",""
        });
    }
    
    void Update()
    {
        if (character != null)
        {
            transform.position = character.position + offset;
        }
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

    public void DisplayNextSentence()
    {
        if (dialogueQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = dialogueQueue.Dequeue();

        if (typeCoroutine != null)
        {
            StopCoroutine(typeCoroutine);
        }

        typeCoroutine = StartCoroutine(TypeSentence(sentence));
    }

    private IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in sentence )
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
        
        isTyping = false;
    }

    private void EndDialogue()
    {
        dialogueText.text = "";
    }
}