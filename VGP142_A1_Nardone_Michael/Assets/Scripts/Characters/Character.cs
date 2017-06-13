using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]

public class Character : MonoBehaviour
{
    CharacterController cc;

    float speed;
    float backSpeed;
    float jumpSpeed;
    float rotateSpeed;
    int jump;                       // Double jump
    float gravity;
    Vector3 moveDir;
    bool canMove;

    public GameObject iceballPrefab;
    Transform projectileSpawn;
    float fireRate;
    float timeSinceFire;

    Animator anim;

    AnimatorStateInfo currentBaseState;
    AnimatorStateInfo currentAttackState;

    static int stunState = Animator.StringToHash("Base Layer.Stunned");
    static int gettingUpState = Animator.StringToHash("Base Layer.Getting_Up");
    static int fireState = Animator.StringToHash("HandAttacks.Iceball");
    static int punchState = Animator.StringToHash("Base Layer.Quad_Punch");

    public AudioClip[] footsteps;
    public AudioClip attack1;
    public AudioClip attack2;
    public AudioClip damageHigh;
    public AudioClip damageLow;

    // Use this for initialization
    void Start()
    {
        cc = GetComponent<CharacterController>();
        if (!cc)
            Debug.Log("No Character Controller found");

        speed = 12;
        backSpeed = 7;
        jumpSpeed = 7;
        rotateSpeed = 3;
        jump = 0;
        gravity = 9.81f;
        moveDir = Vector3.zero;
        canMove = true;
        fireRate = 4;
        timeSinceFire = Time.time;

        projectileSpawn = this.transform.FindChild("ProjectileSpawn").gameObject.transform;

        anim = GetComponentInChildren<Animator>();
        if (!anim)
            Debug.Log("No Animator found");
        
    }

    // Update is called once per frame
    void Update()
    {
        checkHealth();

        currentBaseState = anim.GetCurrentAnimatorStateInfo(0);
        currentAttackState = anim.GetCurrentAnimatorStateInfo(1);

        if (currentBaseState.fullPathHash == stunState 
            || currentBaseState.fullPathHash == gettingUpState 
            || currentAttackState.fullPathHash == fireState
            || currentBaseState.fullPathHash == punchState)
        {
            canMove = false;
            moveDir = Vector3.zero;
        }
        else
        {
            canMove = true;
        }

        anim.SetBool("Grounded", cc.isGrounded);

        if (canMove)
        {
            transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed, 0);

            if (cc.isGrounded)
            {
                // Double jump reset when touching ground
                if (jump > 0)
                    jump = 0;
                // Basic movement
                float moveForward = Input.GetAxis("Vertical");
                moveDir = new Vector3(0, 0, moveForward);
                moveDir = transform.TransformDirection(moveDir);
                // Foward / Backward movement
                if (moveForward < 0)
                    moveDir *= backSpeed;
                else
                    moveDir *= speed;
                // Forward / Backward movement animation
                anim.SetFloat("Jog", moveForward);
            }
            // Use Iceball attack
            if (Input.GetButtonDown("Fire3"))
            {
                if (Time.time > fireRate + timeSinceFire)
                {
                    iceAttack();
                    timeSinceFire = Time.time;
                }
            }
            // Use QuadPunch attack
            if (Input.GetButtonDown("Fire1"))
            {
                anim.SetTrigger("QuadPunch");
            }
            // Jump and Double Jump
            if (Input.GetButtonDown("Jump"))
            {
                if (jump < 1)
                {
                    //Debug.Log("Jump: " + jump);
                    moveDir.y = jumpSpeed;
                    anim.SetTrigger("Jump");
                    if (!cc.isGrounded)
                    {
                        moveDir.y = jumpSpeed * 1.25f;
                        jump += 1;
                        //Debug.Log("Jump: " + jump);
                        anim.SetTrigger("Jump");
                    }
                }
            }
        }
        // Gravity application
        moveDir.y -= gravity * Time.deltaTime;
        // Apply motion
        cc.Move(moveDir * Time.deltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //Debug.Log("Character collided with " + hit.gameObject.name);
        if (hit.gameObject.tag == "Flamestrike")
        {
            GameManager.instance.health -= 20;
            anim.SetTrigger("Stun");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Character entered trigger: " + other.gameObject.tag + ": " + other.gameObject.name);
        if (other.gameObject.name == "KillBox")
        {
            //GameObject.FindGameObjectWithTag("Player Model").SetActive(false);

            // Works - Destroys character model
            //Destroy(GameObject.FindGameObjectWithTag("Player Model"));

            //Debug.Log("You died");
            Destroy(gameObject);
            GameManager.instance.LoadGameOver();
        }
        else if(other.gameObject.name.Contains("key"))
        {
            //Debug.Log("Key collected");
            Destroy(other.gameObject);
            GameManager.instance.keys++;
        }
        else if (other.gameObject.tag == "Flamestrike")
        {
            SoundManager.instance.playSFX(damageHigh);
            GameManager.instance.health -= 20;
            anim.SetTrigger("Stun");
        }
        else if (other.gameObject.name == "Victory Cube")
        {
            //Debug.Log("You win");
            GameManager.instance.LoadVictory();
        }
        else if (other.gameObject.name == "LeftFoot" || other.gameObject.name == "LeftHand" || other.gameObject.tag == "EnemyPatrolAttack")
        {
            //Debug.Log(other.GetComponentInParent<EnemyPatrol>().attack);
            if (other.GetComponentInParent<EnemyPatrol>().attack)    // If the enemy is attacking, otherwise running into player causes damage
            {
                SoundManager.instance.playSFX(damageLow);
                GameManager.instance.health -= 10;
            }
        }
    }

    private void checkHealth()
    {
        if (GameManager.instance.health <= 0)
        {
            Destroy(gameObject);
            GameManager.instance.LoadGameOver();
        }
    }

    private void iceAttack()
    {
        anim.SetTrigger("Iceball");
        StartCoroutine(iceInstantiate());
    }

    private IEnumerator iceInstantiate()
    {
        //Debug.Log("Ice Coroutine started");
        yield return new WaitForSeconds(2);
        //Debug.Log("Ice Coroutine ended");
        GameManager.instance.mana -= 10;
        SoundManager.instance.playSFX(attack1);
        GameObject projectile = Instantiate(iceballPrefab,
            projectileSpawn.position,
            projectileSpawn.rotation) as GameObject;
    }
}

