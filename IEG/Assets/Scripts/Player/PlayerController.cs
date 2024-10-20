using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float mSpeed;
    [SerializeField] private float mJumpForce;

    public Rigidbody rb; 
        
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
    }

    private void Move()
    {
        float direction = Input.GetAxis("Horizontal");
        Vector2 position = transform.position + new Vector3(direction * mSpeed, 0 ,0) * Time.deltaTime;
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(mJumpForce * Vector3.up , ForceMode.Impulse);
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            //遇到npc可以触发对话
        }
    }
}