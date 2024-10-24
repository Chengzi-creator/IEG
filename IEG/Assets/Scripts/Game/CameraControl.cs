using System;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CameraControl : MonoBehaviour
{
        public Transform target;
        private Vector3 targetPos;
        private float smooothSpeed = 1f;

        private void Awake()
        {
                
        }

        private void LateUpdate()
        {
                targetPos = transform.position + new Vector3(0f, 0f, 0f);
                if (target != null)
                {
                        if (transform.position != targetPos)
                        {       
                                Vector3 targetPosition = new Vector3(targetPos.x,targetPos.y,transform.position.z);
                                transform.position = Vector3.Lerp(transform.position, targetPosition, smooothSpeed);
                        }
                }
        }
}