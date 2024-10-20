using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("角色属性")]
    public float speed = 200f;
    public float jumpForce = 70f;
    private Rigidbody2D rb;
    SpriteRenderer spriteRenderer;

    private PlayerInput playerInput;
    Vector2 moveInput;

    [Header("角色状态")]
    public bool isGrounded = false;
    public bool isJumping = false;
    public bool landed = false;
    public float coyoteTime = 0.1f;
    public float coyoteTimeCounter = 0f;
    public bool touchLeftWall;//角色是否触碰左墙
    public bool touchRightWall; //角色是否触碰右墙
    public bool isSticking = false;
    private Animator anim;
    private Vector3 originalPosition;
    private bool isInvincible = false;
    public float currentHealth;

    [Header("检测参数")]
    public Vector2 leftOffset;//左方检测
    public Vector2 rightOffset;//右方检测
    public float checkRaduis;//检测的基础范围
    public LayerMask groundLayer;
    //[HideInInspector]public Character character;
    //private string currentSceneName;

    [Header("角色攻击")]
    public GameObject meleePrefab;
    private PolygonCollider2D meleeCollider;
    public GameObject lungePrefab;
    private PolygonCollider2D lungeCollider;
    public GameObject slashPrefab;
    private PolygonCollider2D slashCollider;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public bool canShoot = false;
    public int attackForm = 0;
    private float attackRate = 0.2f;
    private float attackRateCounter = 0f;
    private Vector2 fireDirection = Vector2.zero;
    private Vector2 mousePos;
    private Camera camera;
    //private static Player instance;//试图保留角色数据
    
    private void Awake()
    {
        //playerInput = new PlayerInput();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        //character = GetComponent<Character>();
        //currentSceneName = SceneManager.GetActiveScene().name;
            
        camera = Camera.main;
        
        // if (instance == null)
        // {
        //     instance = this;
        //     DontDestroyOnLoad(gameObject);//切换关卡血量经验不变
        // }
        // else
        // {
        //     Destroy(gameObject);
        // }
        
        // 获取射击点
        bulletSpawnPoint = transform.Find("Shoot");

        // 获取近战碰撞体
        meleeCollider = meleePrefab.GetComponent<PolygonCollider2D>();

        // 获取冲刺碰撞体
        lungeCollider = lungePrefab.GetComponent<PolygonCollider2D>();

        // 获取斩击碰撞体
        slashCollider = slashPrefab.GetComponent<PolygonCollider2D>();

        // 触发跳跃
        //playerInput.Player.Jump.started += Jump;

        // 触发射击
        //playerInput.Player.Attack.started += Attack;
    }

    private void OnEnable()
    {
        //playerInput.Enable();
    }

    private void Start()
    {

    }

    private void OnDisable()
    {
        //playerInput.Disable();
    }

    private void Update()
    {
        //moveInput = playerInput.Player.Move.ReadValue<Vector2>();
        mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
        fireDirection = mousePos - (Vector2)transform.position;
        
        //Debug.Log("Check");
        Check();
        attackRateCounter += Time.deltaTime;
        //currentHealth = character.currentHealth;
        Dead();
        Stick();
        if (isInvincible)
        {
            transform.position = originalPosition;
        }
    }

    private void FixedUpdate()
    {
        Move();
        CheckGround();
        Fall();
        
        //Stick();
        if (isSticking)
        {
            StickMovement(); // 在贴墙状态下进行特殊移动
        }
    }

    #region 角色攻击
    public void AbsorbEssence(string form)
    {
        switch (form)
        {
            case "Shoot":
                canShoot = true;
                break;
            case "Melee":
                canShoot = false;
                break;
            default:
                break;
        }
    }
    public void AbsorbAnim()
    {
        anim.SetTrigger("absorb");
        originalPosition = transform.position;
        StartCoroutine(Invincible());
    }

    public void LevelUp()
    {
        attackForm++;
        if (attackForm > 2)
        {
            attackForm = 2;
        }
    }

    IEnumerator Invincible()
    {
        isInvincible = true;
        yield return new WaitForSeconds(1f);
        isInvincible = false;
    }

    private void Attack(InputAction.CallbackContext ctx)
    {
        if (CanAttack())
        {
            if (canShoot)
            {
                Shoot();
            }
            else
            {
                switch (attackForm)
                {
                    case 0:
                        Melee();
                        break;
                    case 1:
                        Lunge();
                        break;
                    case 2:
                        Slash();
                        break;
                    default:
                        break;
                }
            }
        }
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

    private void Shoot()
    {
        //GameObject bullet = ObjectPool.Instance.GetObject(bulletPrefab);
        //bullet.transform.position = bulletSpawnPoint.position;
        //anim.SetTrigger("isShoot");
        //bullet.GetComponent<Bullet>().SetSpeed(fireDirection);
    }

    private void Melee()
    {
        meleeCollider.enabled = true;
        anim.SetBool("isMelee", true);
        StartCoroutine(DisableMeleeCollider());
    }

    IEnumerator DisableMeleeCollider()
    {
        yield return new WaitForSeconds(0.2f);
        meleeCollider.enabled = false;
        anim.SetBool("isMelee", false);
    }

    private void Lunge()
    {
        lungeCollider.enabled = true;
        anim.SetBool("isLunge", true);
        StartCoroutine(DisableLungeCollider());
    }

    IEnumerator DisableLungeCollider()
    {
        yield return new WaitForSeconds(0.2f);
        lungeCollider.enabled = false;
        anim.SetBool("isLunge", false);
    }

    private void Slash()
    {
        slashCollider.enabled = true;
        anim.SetBool("isSlash", true);
        StartCoroutine(DisableSlashCollider());
    }

    IEnumerator DisableSlashCollider()
    {
        yield return new WaitForSeconds(0.2f);
        slashCollider.enabled = false;
        anim.SetBool("isSlash", false);
    }
    #endregion

    #region 角色移动
    private void Move()
    {
        rb.velocity = new Vector2(moveInput.x * speed * Time.fixedDeltaTime, rb.velocity.y);

        // 翻转角色
        if (moveInput.x > 0 && !isSticking)
        {
            Flip(true);
            anim.SetBool("isRunning", true);
        }
        else if (moveInput.x < 0 && !isSticking)
        {
            Flip(false);
            anim.SetBool("isRunning", true);
        }
        else if (moveInput.x == 0)
        {
            anim.SetBool("isRunning", false);
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
    #endregion

    #region 角色跳跃
    // private void Jump(InputAction.CallbackContext ctx)
    // {
    //     if (landed && (isGrounded || coyoteTimeCounter > 0))
    //     {
    //         rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    //         coyoteTimeCounter = 0;
    //         isJumping = true;
    //         landed = false;
    //     }
    // }

    private void Fall()
    {
        if (!isInvincible)
        {
            if (rb.velocity.y < 0)
            {
                rb.AddForce(Vector2.down * jumpForce, ForceMode2D.Force);
            }
        }
    }

    public void CheckGround()
    {
        bool wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(transform.position - new Vector3(0, 0.4f, 0), 0.05f, LayerMask.GetMask("Ground")) ||
                     Physics2D.OverlapCircle(transform.position - new Vector3(0, 0.4f, 0), 0.05f, LayerMask.GetMask("Obstacle"));

        if (!wasGrounded && isGrounded)
        {
            landed = true;
        }
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            isJumping = false;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }
    #endregion

    #region 角色爬墙机制

    public void Check()
    {
        Debug.Log("Check");
        
        touchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, checkRaduis, groundLayer);

        touchRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, checkRaduis, groundLayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, checkRaduis);

        Gizmos.DrawWireSphere((Vector2)transform.position - new Vector2(0, 0.4f), 0.1f);
    }

    public void Stick()
    {
        if (touchLeftWall)
        {
            Debug.Log("贴墙了");
            if (Input.GetKeyDown(KeyCode.E))
            {
                isSticking = true;//进入贴墙状态，可以添加动画？
                rb.velocity = Vector2.zero;//停止角色的所有移动
                Debug.Log("进入了");
            }
        }
        if (touchRightWall)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("右墙");
                isSticking = true;//进入贴墙状态，可以添加动画？
                rb.velocity = Vector2.zero;//停止角色的所有移动，可能需要加上localscale的变化
            }
        }
        // if (isSticking && Input.GetKeyDown(KeyCode.Space))
        // {
        //     isSticking = false;//通过跳跃键退出贴墙状态，可添加动画
        // }
    }

    private void StickMovement()
    {
        anim.SetBool("isSticking", true);
        if (touchLeftWall)
        {
            Flip(false);
        }
        else if (touchRightWall)
        {
            Flip(true);
        }    
        // 允许上下移动
        float verticalInput = Input.GetAxis("Vertical");
        rb.velocity = new Vector2(0, verticalInput * speed / 50);

        // 在贴墙状态下进行跳跃
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isSticking = false; // 退出贴墙状态
            anim.SetBool("isSticking", false);
        }

        if (!touchLeftWall && !touchRightWall)
        {
            Debug.Log("Exit");
            // rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isSticking = false; // 退出贴墙状态
            anim.SetBool("isSticking", false);
        }
    }
    #endregion

    #region 角色死亡

    private void Dead()
    {
        if(currentHealth == 0f)
        {   
            //ObjectPool.Instance.Clear(); // 初始化对象池
            //SceneManager.LoadScene(currentSceneName);
        }
    }
    #endregion
}
