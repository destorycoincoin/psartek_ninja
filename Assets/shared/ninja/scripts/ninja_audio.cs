using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ninja_audio : MonoBehaviour {

    public AudioClip[] audio_step_array;
    public AudioClip[] audio_dash_array;
    public float audio_step_length;
    private float time;
    private AudioSource[] audio_source;
    private System.Random rand;
    private bool is_walking;

    // Use this for initialization
    void Start () {
        time = 0.0f;
        audio_source = this.GetComponents<AudioSource>();
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

    public void dash()
    {
        is_walking = false;
        audio_source[1].clip = audio_dash_array[rand.Next(0, audio_dash_array.Length)];
        audio_source[1].Play();
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
                audio_source[0].clip = audio_step_array[rand.Next(0, audio_step_array.Length)];
                audio_source[0].Play();
            }
        }
    }
}
