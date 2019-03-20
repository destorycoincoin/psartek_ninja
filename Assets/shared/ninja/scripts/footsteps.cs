using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class footsteps : MonoBehaviour {

    public AudioClip[] audio_step_array;
    public float audio_step_length;
    private float time;
    private AudioSource audio_source;
    private System.Random rand;
    private bool is_walking;

    // Use this for initialization
    void Start () {
        time = 0.0f;
        audio_source = this.GetComponent<AudioSource>();
        rand = new System.Random();
        is_walking = false;
    }

    public void start_walking ()
    {
        if (!is_walking)
        {
            is_walking = true;
            time = audio_step_length;
        }
    }

    public void stop_walking()
    {
        is_walking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (is_walking)
        {
            time += Time.deltaTime;

            if (time >= audio_step_length)
            {
                time = 0.0f;
                audio_source.clip = audio_step_array[rand.Next(0, audio_step_array.Length - 1)];
                audio_source.Play();
            }
        }
    }
}
