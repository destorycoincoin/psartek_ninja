using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dog : MonoBehaviour {
	public Animator animator;
	private Rigidbody2D rb;
	private GameObject ninjaObject;
	private const int UP = 1;
	private const int RIGHT = 2;
	private const int LEFT = 3;
	private const int DOWN = 4;
	public int startDirection;
	private Vector2 startPosition;
	private float cooldown;
	private float cyrilIneptitude;

	// Use this for initialization
	void InitVar()
	{
		this.rb = GetComponent<Rigidbody2D>();
		this.animator = GetComponent<Animator>();
		this.cooldown = 0f;
		this.cyrilIneptitude = 4.0f;
		this.startPosition = this.transform.position;
		this.ninjaObject = GameObject.Find("Ninja");
	}
	void Start () {
		this.InitVar();
		this.animator.SetFloat("direction", this.startDirection);
	}
	
	void UpdateDirection(Vector2 moveDirection)
	{
		if (Mathf.Abs(moveDirection.y) > Mathf.Abs(moveDirection.x))
		{
			if (moveDirection.y > 0)
				this.animator.SetFloat("direction", dog.UP);
			else if (moveDirection.y < 0)
				this.animator.SetFloat("direction", dog.DOWN);
		}
		else if (Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.y))
		{
			if (moveDirection.x > 0)
				this.animator.SetFloat("direction", dog.RIGHT);
			else if (moveDirection.x < 0)
				this.animator.SetFloat("direction", dog.LEFT);
		}
	}

	void Move(Vector2 moveDirection)
	{
		moveDirection.Normalize();
		this.UpdateDirection(moveDirection);
		if (!this.animator.GetCurrentAnimatorStateInfo(0).IsName("walk"))
			this.animator.SetTrigger("walk");
		this.rb.AddForce(0.5f * moveDirection * this.cyrilIneptitude, ForceMode2D.Impulse);
	}

	void Attack(Vector2 attackDirection)
	{
		attackDirection.Normalize();
		this.UpdateDirection(attackDirection);
		if (!this.animator.GetCurrentAnimatorStateInfo(0).IsName("attack"))
			this.animator.SetTrigger("attack");
		this.cooldown = 0.5f;
	}

	void ResetStart()
	{
		this.animator.SetFloat("direction", this.startDirection);
		this.animator.SetTrigger("stay");
	}

	// Update is called once per frame
	void Update() {
		if (this.cooldown >= 0f)
		{
			this.cooldown -= Time.deltaTime;
			return;
		}
		this.rb.velocity = Vector2.zero;
		if (Vector2.Distance(this.transform.position, this.ninjaObject.transform.position) < 1.25f)
			this.Attack(new Vector2(this.ninjaObject.transform.position.x - this.transform.position.x, this.ninjaObject.transform.position.y - this.transform.position.y));
		else if (Vector2.Distance(this.transform.position, this.ninjaObject.transform.position) < 10)
			this.Move(new Vector2(this.ninjaObject.transform.position.x - this.transform.position.x, this.ninjaObject.transform.position.y - this.transform.position.y));
		else if (Vector2.Distance(this.startPosition, this.transform.position) >= 0.3f)
			this.Move(new Vector2(this.startPosition.x - this.transform.position.x, this.startPosition.y - this.transform.position.y));
		else
			this.ResetStart();
	}
}
