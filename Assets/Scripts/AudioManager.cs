using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public AudioClip[] fixes;
    public AudioClip[] hits;
    public AudioClip[] screams;
    public AudioClip[] wooshes;
    public AudioClip[] timers;
    public AudioClip[] beeps;
    public AudioClip[] steps;
    public AudioSource[] sources;
    AudioSource sfx;
    AudioSource soundtrack;

	void Start () {
        sources = GetComponents<AudioSource>();
        sfx = sources[0];
	}

    public void PlaySound(string code)
    {
        switch (code)
        {
            case "fixes":
                sfx.PlayOneShot(fixes[Random.Range(0, fixes.Length)]);
                break;
            case "hits":
                sfx.PlayOneShot(hits[Random.Range(0, hits.Length)]);
                break;
            case "steps":
                sfx.PlayOneShot(steps[Random.Range(0, steps.Length)]);
                break;
        }
    }
}
