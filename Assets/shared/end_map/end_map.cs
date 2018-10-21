using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class end_map : MonoBehaviour {
	public string		next_map;
    public float        next_x;
    public float        next_y;
    public int          start_direction;
	
    // Use this for initialization
	void Start () {
		
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
            shared_data.Ninja_x = this.next_x;
            shared_data.Ninja_y = this.next_y;
            shared_data.Start_direction = this.start_direction;
			SceneManager.LoadScene(next_map);
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
