using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleport : MonoBehaviour
{
	public Animator		animator;
	private bool		isStarted;

	// Use this for initialization
	void Start ()
	{
		this.isStarted = false;
	}

	public void ChangeState()
	{
		if (this.isStarted)
		{
			this.isStarted = false;
			this.animator.SetTrigger("teleport_end");
		}
		else
		{
			StartCoroutine(Wait());
		}
	}

	IEnumerator Wait()
	{
		yield return new WaitForSeconds(0.6f);
		this.isStarted = true;
		this.animator.SetTrigger("teleport_start");
	}

	// Update is called once per frame
	void Update ()
	{
	}
}
