using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Security.Cryptography;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Security.Policy;
using UnityEngine.SceneManagement; // here ?

// TEMP think about DIRECTION
public class ninja : MonoBehaviour
{
	public int					startDirection;
	public Animator				animator;
	private SpriteRenderer		sprite;
	private Rigidbody2D			rb;
	private const int			STAY	= 0;
	private const int			UP		= 1;
	private const int			RIGHT	= 2;
	private const int			LEFT	= 3;
	private const int			DOWN	= 4;
	private int					MoveState;
	private float				cooldown;
	private float				jumpCooldown;
	private float				dashCooldown;
	private bool				isTeleportReady;
	public GameObject			TeleportObject; //FIX private
	private teleport			TeleportScript;
	private int					direction;
	private int					onWood;
	private float				cyrilIneptitude;
    public float                camera_x_min;
    public float                camera_x_max;
    public float                camera_y_min;
    public float                camera_y_max;
    private bool                is_interrupt;
    private Collider2D          interrupt;
    private footsteps           footsteps_script;

    // Use this for initialization
    void  initVar ()
	{
		this.rb					= GetComponent<Rigidbody2D>();
		this.MoveState			= ninja.STAY;
		this.cooldown			= 0f;
		this.jumpCooldown		= 0f;
		this.dashCooldown		= 0f;
		this.isTeleportReady	= false;
		this.TeleportScript		= TeleportObject.GetComponent<teleport>();
		this.direction			= this.startDirection; //TEMP no more needed ?
		this.onWood				= 0;
		this.cyrilIneptitude	= 4.0f;
        this.is_interrupt       = false;
        this.footsteps_script   = GetComponent<footsteps>();
	}
	void Start ()
	{
		this.initVar();
        this.transform.position = new Vector2(shared_data.Ninja_x, shared_data.Ninja_y);
        this.direction = shared_data.Start_direction;
    }

	// only on stay ?
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("wood"))
			this.onWood += 1;
		else if (other.CompareTag("transparency"))
		{
			other.gameObject.GetComponent<SpriteRenderer>().color = new Vector4(1f, 1f, 1f, 0.8f);
		}
        if (other.CompareTag("interrupt"))
        {
            this.is_interrupt = true;
            this.interrupt = other;
        }
    }

	void OnTriggerStay2D(Collider2D other)
	{
		if (other.CompareTag("water"))
		{
			if (this.cooldown < 0.1f && this.jumpCooldown <= 0.1f && this.onWood == 0) // care, cooldown is here only for tp
			{
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			}
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("wood"))
		{
			this.onWood -= 1;
		}
		else if (other.CompareTag("transparency"))
		{
			other.gameObject.GetComponent<SpriteRenderer>().color = new Vector4(1f, 1f, 1f, 1f);
		}
        if (other.CompareTag("interrupt"))
            this.is_interrupt = false;
    }

	// Update is called once per frame
	void Update ()
	{
		this.rb.velocity = Vector2.zero;

		if (this.cooldown >= 0f)
		{
			this.cooldown -= Time.deltaTime;
			return;
		}

		if (this.dashCooldown > 0f)
		{
			this.dashCooldown -= Time.deltaTime;

			if (this.dashCooldown >= 0.2f)
			{
				if (this.direction == ninja.UP)
					this.rb.AddForce(5f * Vector2.up * this.cyrilIneptitude, ForceMode2D.Impulse);
				else if (this.direction == ninja.RIGHT)
					this.rb.AddForce(5f * Vector2.right * this.cyrilIneptitude, ForceMode2D.Impulse);
				else if (this.direction == ninja.LEFT)
					this.rb.AddForce(5f * Vector2.left * this.cyrilIneptitude, ForceMode2D.Impulse);
				else if (this.direction == ninja.DOWN)
					this.rb.AddForce(5f * Vector2.down * this.cyrilIneptitude, ForceMode2D.Impulse);

				return;
			}
		}

		if (this.jumpCooldown > 0f)
		{
			this.jumpCooldown -= Time.deltaTime;
		}
        else if (this.is_interrupt && Input.GetButtonDown("Xbox360_X"))
            this.interrupt.gameObject.GetComponent<fence_door>().Change();
        else if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Xbox360_A"))
		{
			this.animator.SetTrigger("jump");
			this.jumpCooldown = 0.8f;
            this.footsteps_script.stop_walking();
            return;
		}
		else if ((Input.GetKeyDown(KeyCode.A) || Input.GetButtonDown("Xbox360_L")) && this.dashCooldown <= 0)
		{
			this.animator.SetTrigger("dash");
			this.dashCooldown	= 0.4f;
            this.footsteps_script.stop_walking();
            return;
		}
		else if (Input.GetKeyDown(KeyCode.D) || Input.GetButtonDown("Xbox360_B"))
		{
			if (this.isTeleportReady)
			{
				this.transform.position = this.TeleportObject.transform.position;
				this.animator.SetFloat("teleport_start", 0);
				this.cooldown = 0.4f;
			}
			else
			{
				this.TeleportObject.transform.position = this.transform.position;
				this.animator.SetFloat("teleport_start", 1);
				this.cooldown = 0.8f;
			}

			this.animator.SetTrigger("teleport");
			this.TeleportScript.ChangeState();
			this.isTeleportReady = !this.isTeleportReady;
            this.footsteps_script.stop_walking();
            return;
		}

		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			this.MoveState = ninja.UP;
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			this.MoveState = ninja.RIGHT;
		}
		else if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			this.MoveState = ninja.LEFT;
		}
		else if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			this.MoveState = ninja.DOWN;
		}
		else if ((this.MoveState == ninja.UP && !Input.GetKey(KeyCode.UpArrow))
				|| (this.MoveState == ninja.RIGHT && !Input.GetKey(KeyCode.RightArrow))
				|| (this.MoveState == ninja.LEFT && !Input.GetKey(KeyCode.LeftArrow))
				|| (this.MoveState == ninja.DOWN && !Input.GetKey(KeyCode.DownArrow)))
		{
			this.MoveState = ninja.STAY;
		}

		if ((this.MoveState == ninja.UP || this.MoveState == ninja.STAY) && Input.GetKey(KeyCode.UpArrow) || Input.GetAxisRaw("Xbox360_Dpad_Y") >= 0.5f || (Input.GetAxisRaw("Vertical") >= 0.5f && !Input.GetKey(KeyCode.UpArrow)))
		{
			this.MoveState = ninja.UP;
			this.direction = ninja.UP;
			this.animator.SetFloat("direction", 1.0f);
			if (!this.animator.GetCurrentAnimatorStateInfo(0).IsName("walk") && this.jumpCooldown <= 0)
			{
				this.animator.SetTrigger("walk");
                this.footsteps_script.start_walking();
            }
			this.rb.AddForce(1.5f * Vector2.up * this.cyrilIneptitude, ForceMode2D.Impulse);
		}
		else if ((this.MoveState == ninja.RIGHT || this.MoveState == ninja.STAY) && Input.GetKey(KeyCode.RightArrow) || Input.GetAxisRaw("Xbox360_Dpad_X") >= 0.5f || (Input.GetAxisRaw("Horizontal") >= 0.5f && !Input.GetKey(KeyCode.RightArrow)))
		{
			this.MoveState = ninja.RIGHT;
			this.direction = ninja.RIGHT;
			this.animator.SetFloat("direction", 2.0f);
			if (!this.animator.GetCurrentAnimatorStateInfo(0).IsName("walk") && this.jumpCooldown <= 0)
			{
				this.animator.SetTrigger("walk");
                this.footsteps_script.start_walking();
            }
			this.rb.AddForce(1.5f * Vector2.right * this.cyrilIneptitude, ForceMode2D.Impulse);
		}
		else if ((this.MoveState == ninja.LEFT || this.MoveState == ninja.STAY) && Input.GetKey(KeyCode.LeftArrow) || Input.GetAxisRaw("Xbox360_Dpad_X") <= -0.5f || (Input.GetAxisRaw("Horizontal") <= -0.5f && !Input.GetKey(KeyCode.LeftArrow)))
		{
			this.MoveState = ninja.LEFT;
			this.direction = ninja.LEFT;
			this.animator.SetFloat("direction", 3.0f);
			if (!this.animator.GetCurrentAnimatorStateInfo(0).IsName("walk") && this.jumpCooldown <= 0)
			{
				this.animator.SetTrigger("walk");
                this.footsteps_script.start_walking();
            }
			this.rb.AddForce(1.5f * Vector2.left * this.cyrilIneptitude, ForceMode2D.Impulse);
		}
		else if ((this.MoveState == ninja.DOWN || this.MoveState == ninja.STAY) && Input.GetKey(KeyCode.DownArrow) || Input.GetAxisRaw("Xbox360_Dpad_Y") <= -0.5f || (Input.GetAxisRaw("Vertical") <= -0.5f && !Input.GetKey(KeyCode.DownArrow)))
		{
			this.MoveState = ninja.DOWN;
			this.direction = ninja.DOWN;
			this.animator.SetFloat("direction", 4.0f);
			if (!this.animator.GetCurrentAnimatorStateInfo(0).IsName("walk") && this.jumpCooldown <= 0)
			{
				this.animator.SetTrigger("walk");
                this.footsteps_script.start_walking();
			}
			this.rb.AddForce(1.5f * Vector2.down * this.cyrilIneptitude, ForceMode2D.Impulse);
		}
		else
		{
			this.MoveState = ninja.STAY;
			this.animator.SetFloat("direction", this.direction);
			if (!this.animator.GetCurrentAnimatorStateInfo(0).IsName("stay") && this.jumpCooldown <= 0)
			{
				this.animator.SetTrigger("stay");
                this.footsteps_script.stop_walking();
            }
		}
	}

	void LateUpdate ()
	{
        float camera_x;
        float camera_y;

        if (transform.localPosition.x < this.camera_x_min)
            camera_x = this.camera_x_min;
        else if (transform.localPosition.x > this.camera_x_max)
            camera_x = this.camera_x_max;
        else
            camera_x = transform.localPosition.x;
        if (transform.localPosition.y < this.camera_y_min)
            camera_y = this.camera_y_min;
        else if (transform.localPosition.y > this.camera_y_max)
            camera_y = this.camera_y_max;
        else
            camera_y = transform.localPosition.y;


        Camera.main.transform.localPosition = new Vector3(camera_x, camera_y, -10.0f);
	}
}

