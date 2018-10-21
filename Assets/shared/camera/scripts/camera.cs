using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Security.Cryptography;
using System.Collections.Specialized;

public class camera : MonoBehaviour {
	public GameObject ninja;
	private float zoom = 10;

	// Use this for initialization
	void Start () {
		GetComponent<Camera>().orthographicSize = zoom;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.R))
			SceneManager.LoadScene (SceneManager.GetActiveScene().name);
	}
}
