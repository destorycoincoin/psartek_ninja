using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitbox : MonoBehaviour {
	private GameObject[] objectArray;
	private dog dogScript;
	private ninja ninjaScript;
	public int damages;
	private Collider2D hitboxCollider;
	private bool hit;

	// Use this for initialization
	void Start () {
	}

	void OnEnable()
	{
		hitboxCollider = this.GetComponent<Collider2D>();
		hitboxCollider.isTrigger = true;
		hit = false;
	}

	void OnDisable()
	{
		hitboxCollider.isTrigger = false;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (!hit)
		{
			if (this.transform.parent.CompareTag("enemy") && other.CompareTag("Player"))
			{
				other.GetComponent<ninja>().Hit(damages);
				this.hit = true;
			}
			else if (this.transform.parent.CompareTag("Player") && other.CompareTag("enemy"))
			{
				other.GetComponent<dog>().Hit(damages);
				this.hit = true;
			}
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
