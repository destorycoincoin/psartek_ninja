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
	private Animator			animator;
	private Rigidbody2D			rb;
	private const int			UP		= 1;
	private const int			RIGHT	= 2;
	private const int			LEFT	= 3;
	private const int			DOWN	= 4;
	private float				cooldown;
	private float				jumpCooldown;
	private float				dashCooldown;
	private bool				isTeleportReady;
	private GameObject			TeleportObject;
	private teleport			TeleportScript;
	private int					direction;
	private Vector2				directionVec;
	private Vector2				dashDirection;
	private int					onWood;
	private float				cyrilIneptitude;
    public float                camera_x_min;
    public float                camera_x_max;
    public float                camera_y_min;
    public float                camera_y_max;
    private bool                is_interrupt;
    private Collider2D          interrupt;
    private ninja_audio         ninja_audio_script;
	private int					hp;
	private GameObject[]		hitboxArray;

	// Use this for initialization
	void  initVar ()
	{
		this.rb					= GetComponent<Rigidbody2D>();
		this.animator			= GetComponent<Animator>();
		this.cooldown			= 0f;
		this.jumpCooldown		= 0f;
		this.dashCooldown		= 0f;
		this.isTeleportReady	= false;
		this.TeleportObject		= GameObject.Find("Teleport");
		this.TeleportScript		= this.TeleportObject.GetComponent<teleport>();
		this.directionVec		= Vector2.zero;
		this.dashDirection		= Vector2.zero;
		this.onWood				= 0;
		this.cyrilIneptitude	= 4.0f;
        this.is_interrupt       = false;
        this.ninja_audio_script = GetComponent<ninja_audio>();
		this.hp					= 100;
		this.hitboxArray		= new GameObject[5];
		this.hitboxArray[ninja.UP]		= this.transform.Find("hitbox_up").gameObject;
		this.hitboxArray[ninja.RIGHT]	= this.transform.Find("hitbox_right").gameObject;
		this.hitboxArray[ninja.LEFT]	= this.transform.Find("hitbox_left").gameObject;
		this.hitboxArray[ninja.DOWN]	= this.transform.Find("hitbox_down").gameObject;
	}
	void Start ()
	{
		this.initVar();
        this.transform.position = new Vector2(shared_data.Ninja_x, shared_data.Ninja_y);
        this.direction = shared_data.Start_direction;
		this.animator.SetFloat("direction", this.direction);
	}

	public void Hit (int damages)
	{
		this.hp -= damages;
		Debug.Log("hp = " + this.hp);
		if (this.hp <= 0)
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
		else if (other.CompareTag("interrupt"))
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
		this.directionVec = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		this.directionVec.Normalize();
		if (this.cooldown <= 0f && this.dashCooldown < 0.2f)
		{
			if (Mathf.Abs(this.directionVec.y) > Mathf.Abs(this.directionVec.x))
			{
				if (this.directionVec.y > 0)
					this.direction = ninja.UP;
				else if (this.directionVec.y < 0)
					this.direction = ninja.DOWN;
			}
			else if (Mathf.Abs(this.directionVec.x) > Mathf.Abs(this.directionVec.y))
			{
				if (this.directionVec.x > 0)
					this.direction = ninja.RIGHT;
				else if (this.directionVec.x < 0)
					this.direction = ninja.LEFT;
			}
			this.animator.SetFloat("direction", this.direction);
		}

		if (this.cooldown > 0f)
		{
			this.cooldown -= Time.deltaTime;
			if (this.cooldown <= 0f)
				this.hitboxArray[(int)this.animator.GetFloat("direction")].SetActive(false);
			return;
		}

		if (this.dashCooldown > 0f)
		{
			this.dashCooldown -= Time.deltaTime;

			if (this.dashCooldown >= 0.2f)
			{
				this.rb.AddForce(5f * this.dashDirection * this.cyrilIneptitude, ForceMode2D.Impulse);
				return;
			}
		}

		if (this.jumpCooldown > 0f)
		{
			this.jumpCooldown -= Time.deltaTime;
		}
        else if (this.is_interrupt && (Input.GetButtonDown("Xbox360_X") || Input.GetKeyDown(KeyCode.L)))
            this.interrupt.gameObject.GetComponent<fence_door>().Change();
        else if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Xbox360_A"))
		{
			this.animator.SetTrigger("jump");
			this.jumpCooldown = 0.8f;
            this.ninja_audio_script.jump();
            return;
		}
		else if ((Input.GetKeyDown(KeyCode.J) || Input.GetButtonDown("Xbox360_L")) && this.dashCooldown <= 0)
		{
			this.animator.SetTrigger("dash");
			this.dashCooldown = 0.4f;
			if (this.directionVec != Vector2.zero)
				this.dashDirection = this.directionVec;
			else if (this.direction == ninja.UP)
				this.dashDirection = Vector2.up;
			else if (this.direction == ninja.RIGHT)
				this.dashDirection = Vector2.right;
			else if (this.direction == ninja.LEFT)
				this.dashDirection = Vector2.left;
			else
				this.dashDirection = Vector2.down;
			this.ninja_audio_script.dash();
            return;
		}
		else if (Input.GetKeyDown(KeyCode.K) || Input.GetButtonDown("Xbox360_B"))
		{
			if (this.isTeleportReady)
			{
				this.transform.position = this.TeleportObject.transform.position;
				this.animator.SetFloat("teleport_start", 0);
				this.cooldown = 0.4f;
                this.ninja_audio_script.tp_reappear();
            }
			else
			{
				this.TeleportObject.transform.position = this.transform.position;
				this.animator.SetFloat("teleport_start", 1);
				this.cooldown = 0.8f;
                this.ninja_audio_script.tp_cast();
            }

			this.animator.SetTrigger("teleport");
			this.TeleportScript.ChangeState();
			this.isTeleportReady = !this.isTeleportReady;
            return;
		}
		else if (Input.GetKeyDown(KeyCode.I) || Input.GetButtonDown("Xbox360_X"))
		{
			this.animator.SetTrigger("attack_weak");
			this.cooldown = 0.3f;
            this.ninja_audio_script.attack_weak();
			this.hitboxArray[(int)this.animator.GetFloat("direction")].SetActive(true);

			return;
		}

		if (this.directionVec != Vector2.zero)
		{
			if (!this.animator.GetCurrentAnimatorStateInfo(0).IsName("walk") && this.jumpCooldown <= 0)
			{
				this.animator.SetTrigger("walk");
				this.ninja_audio_script.start_walking();
			}
			this.rb.AddForce(1.5f * this.directionVec * this.cyrilIneptitude, ForceMode2D.Impulse);
		}
		else if (!this.animator.GetCurrentAnimatorStateInfo(0).IsName("stay") && this.jumpCooldown <= 0)
		{
			this.animator.SetTrigger("stay");
			this.ninja_audio_script.stop_walking();
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

