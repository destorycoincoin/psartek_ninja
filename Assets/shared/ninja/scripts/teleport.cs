using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleport : MonoBehaviour
{
	public Animator		animator;
	private bool		isStarted;
	private AudioSource audio_source;

	// Use this for initialization
	void Start ()
	{
		this.isStarted = false;
		this.audio_source = this.GetComponent<AudioSource>();
	}

	public void ChangeState()
	{
		if (this.isStarted)
		{
			this.isStarted = false;
			this.animator.SetTrigger("teleport_end");
			this.audio_source.Stop();
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
		this.audio_source.Play();
	}

	// Update is called once per frame
	void Update ()
	{
	}
}
