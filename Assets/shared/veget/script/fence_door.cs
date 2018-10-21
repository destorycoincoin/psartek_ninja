using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fence_door : MonoBehaviour {
    public Sprite off;
    public Sprite on;
    public GameObject first_door;
    public GameObject second_door;
    private bool state = false;

	// Use this for initialization
	void Start () {
		
	}

    public void Change () {
        if (state == false)
        {
            this.GetComponent<SpriteRenderer>().sprite = on;
            first_door.SetActive(false);
            second_door.SetActive(true);
            state = true;
        }
        else
        {
            this.GetComponent<SpriteRenderer>().sprite = off;
            first_door.SetActive(true);
            second_door.SetActive(false);
            state = false;
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
