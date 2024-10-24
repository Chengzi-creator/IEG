using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class BossControl : MonoBehaviour
{
     [Header("移动参数")]
    public float moveSpeed = 2f; //移动速度
    private Rigidbody2D rb;
    private bool faceRight = false;

    [Header("攻击参数")]
    public int attackForm = 0;
    public float attackRate = 0.2f;
    public float attackRateCounter = 0f;
    
    private Animator anim;
    
    [Header("检测参数")]
    public Vector2 leftOffset;//左方检测
    public Vector2 rightOffset;//右方检测
    public float checkRaduis;//检测的基础范围
    public bool touchLeftWall;//角色是否触碰左墙
    public bool touchRightWall; //角色是否触碰右墙
    public LayerMask groundLayer;
    public float CheckRate = 1f;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        //移动逻辑
        Patrol();
        
        
        Check();

        if (touchLeftWall)
        {
            Rotate();
            touchLeftWall = false;
        }
        if (touchRightWall)
        {
            Rotate();
            touchRightWall = false;
        }
            
        //攻击逻辑
        attackRateCounter += Time.deltaTime;
        if (CanAttack())
        {
            Slash();
        }
    }

    private void Patrol()
    {
        transform.position += new Vector3(-moveSpeed * Time.deltaTime, 0f, 0f);
            
        if (touchLeftWall)
        {
            Flip(false);
            moveSpeed = -moveSpeed;
        }

        if (touchRightWall)
        {
            Flip(true);
            moveSpeed = -moveSpeed;
        }
    }

    private void Rotate()
    {
        if (touchLeftWall)
        {
            Flip(false);
            moveSpeed = -moveSpeed;
        }

        if (touchRightWall)
        {
            Flip(true);
            moveSpeed = -moveSpeed;
        }
    }
    
    private void Flip(bool faceRight)
    {
        Vector3 scale = transform.localScale;

        if (faceRight)
        {
            scale.x = Mathf.Abs(scale.x);
        }
        else
        {
            scale.x = -Mathf.Abs(scale.x);
        }

        transform.localScale = scale;
    }
    

    private bool CanAttack()
    {
        if (attackRateCounter >= attackRate)
        {
            attackRateCounter = 0f;
            return true;
        }
        else
        {
            return false;
        }
    }

    
    private void Slash()
    {
        anim.SetTrigger("Attack");
        //Debug.Log("Attack");
        //对前方方形区域造成伤害
        StartCoroutine(DisableSlashCollider());
    }

    IEnumerator DisableSlashCollider()
    {
        yield return new WaitForSeconds(0.2f);
    }
   
    public void Check()
    {
        //Debug.Log("Check");
        
        touchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, checkRaduis, groundLayer);
        touchRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, checkRaduis, groundLayer);
    }

    
    private void OnDrawGizmos()
    {

        
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, checkRaduis);

    }
}
