using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public float currentHealth;
    [HideInInspector] public Character character; 
    private Animator anim;
    
    [Header("检测参数")]
    public Vector2 leftOffset;//左方检测
    public Vector2 rightOffset;//右方检测
    public float checkRaduis;//检测的基础范围
    public bool touchLeftWall;//角色是否触碰左墙
    public bool touchRightWall; //角色是否触碰右墙
    public LayerMask groundLayer;
    public float CheckRate = 1f;

    public GameObject Player;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        character = GetComponent<Character>();
    }

    private void Update()
    {   
        currentHealth = character.currentHealth;
        
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
        
        Dead();
    }

    private void Patrol()
    {
        transform.position += new Vector3(-moveSpeed * Time.deltaTime, 0f, 0f);
        anim.SetBool("isRunning",true);
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
        Vector2 attackPosition = (Vector2)transform.position + (-Vector2.right * transform.localScale.x * 1.5f);
        Vector2 attackSize = new Vector2(1.5f, 1.5f);
        LayerMask enemyLayer = LayerMask.GetMask("Player");
        
        Collider2D[] hitPlayer = Physics2D.OverlapBoxAll(attackPosition, attackSize, 0, enemyLayer);
        
        foreach (Collider2D player in hitPlayer)
        {
            Character character = player.GetComponent<Character>();
            if (character != null)
            {
                character.TakeDamage(1f);
            }
        }
        
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

    private void Dead()
    {
        if (currentHealth == 0f)
        {
            moveSpeed = 0f;
            Player.transform.position = new Vector3(0f, -0.2f, 0f);
            Destroy(gameObject);
        }
    }
}
