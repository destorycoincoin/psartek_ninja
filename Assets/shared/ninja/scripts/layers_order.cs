using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class layers_order : MonoBehaviour {

	public GameObject		ninja;  //TEMP !
	private SpriteRenderer	sprite;
	private float adapt = 0.3f;

	// Use this for initialization
	void Start()
	{
		this.sprite = GetComponent<SpriteRenderer>();
	}

	// Update is called once per frame
	void Update()
	{
		if (this.ninja.transform.position.y > this.transform.position.y - this.sprite.sprite.bounds.size.y / 2 + this.adapt) //TEMP change that
		{
			if (this.sprite.sortingOrder == 2)
				this.sprite.sortingOrder = 6;
		}
		else if (this.sprite.sortingOrder == 6)
			this.sprite.sortingOrder = 2;
	}
}