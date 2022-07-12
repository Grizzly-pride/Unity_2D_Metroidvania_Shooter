using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D theRB;
    public float moveSpeed;
    public float jumpForce;

    public Transform groundPoint;
    private bool isOnGround;
    public LayerMask whatIsGround;

    public Animator anim;

    public BulletController shotToFier;
    public Transform shotPoint;

    private bool isDubleJump;

    public float dashSpeed, dashTime;
    private float dashCounter;
    private bool isDashing;

    public GameObject standing, ball;
    public float waitToBall;
    private float ballCounter;
    public Animator ballAnim;

    public Transform bombPoint;
    public GameObject bomb;

    private PlayerAbilityTracker abilities;

    public bool canMove;


    void Start()
    {
        abilities = GetComponent<PlayerAbilityTracker>();

        canMove = true;
       
    }

    void Update()
    {
        if (canMove && Time.timeScale != 0)
        {

            if (Input.GetButtonDown("Fire2") && standing.activeSelf && abilities.canDash)
            {
                dashCounter = dashTime;
            }

            if (dashCounter > 0)
            {
                dashCounter -= Time.deltaTime;
                isDashing = true;
                theRB.velocity = new Vector2(dashSpeed * transform.localScale.x, transform.localScale.y);

                AudioManager.instance.PlaySFXAdjusted(7);
            }
            else
            {
                isDashing = false;

                //movement axis x
                theRB.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, theRB.velocity.y);

                //direction change
                if (theRB.velocity.x < 0)
                {
                    transform.localScale = new Vector2(-1f, 1f);
                }
                else if (theRB.velocity.x > 0)
                {
                    transform.localScale = Vector2.one;
                }

            }


            //check ground
            isOnGround = Physics2D.OverlapCircle(groundPoint.position, 0.2f, whatIsGround);

            //jumping
            if (Input.GetButtonDown("Jump") && (isOnGround || (isDubleJump && abilities.canDoubleJump)))
            {
                if (isOnGround)
                {
                    isDubleJump = true;
                    AudioManager.instance.PlaySFXAdjusted(12);
                }
                else
                {
                    isDubleJump = false;
                    anim.SetTrigger("doubleJump");

                    AudioManager.instance.PlaySFXAdjusted(9);
                }

                theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);
            }

            //FireShot
            if (Input.GetButtonDown("Fire1"))
            {
                if (standing.activeSelf && isDashing == false)
                {
                    Instantiate(shotToFier, shotPoint.position, shotPoint.rotation).moveDir = new Vector2(transform.localScale.x, 0f);
                    anim.SetTrigger("shotFired");
                    AudioManager.instance.PlaySFXAdjusted(14);
                }
                else if (ball.activeSelf && abilities.canDropBomb)
                {
                    Instantiate(bomb, bombPoint.position, bombPoint.rotation);
                    AudioManager.instance.PlaySFX(13);
                }



            }

            //Ball mode
            if (!ball.activeSelf)
            {
                if (Input.GetAxisRaw("Vertical") < -0.5f && abilities.canBecomeBall)
                {
                    ballCounter -= Time.deltaTime;
                    if (ballCounter <= 0)
                    {
                        ball.SetActive(true);
                        standing.SetActive(false);

                        AudioManager.instance.PlaySFX(6);
                    }
                }
                else
                {
                    ballCounter = waitToBall;
                }
            }
            else
            {
                if (Input.GetAxisRaw("Vertical") > -0.5f)
                {
                    ballCounter -= Time.deltaTime;
                    if (ballCounter <= 0)
                    {
                        ball.SetActive(false);
                        standing.SetActive(true);

                        AudioManager.instance.PlaySFX(10);
                    }
                }
                else
                {
                    ballCounter = waitToBall;
                }

            }

        }
        else
        {
            theRB.velocity = Vector3.zero;  
        }

        //animation transition
        if (standing.activeSelf)
        {
            anim.SetBool("isOnGround", isOnGround);
            anim.SetFloat("speed", Mathf.Abs(theRB.velocity.x));
            anim.SetBool("isDashing", isDashing);

        }
        else if (ball.activeSelf)
        {
            ballAnim.SetFloat("speed", Mathf.Abs(theRB.velocity.x));
        }


    }
}
