using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public AudioClip[] fixes;
    public AudioClip[] goodScreams;
    public AudioClip[] badScreams;
    public AudioClip[] reboots;
    public AudioClip[] startRepair;
    public AudioClip[] dies;
    public AudioClip[] wooshes;
    public AudioClip[] instructionCardComplete;
    public AudioClip[] beeps;
    public AudioClip[] steps;
    public AudioClip[] start;
    public AudioSource[] sources;
    AudioSource sfx;
    AudioSource soundtrack;

    public AudioSource loudAudioSource;

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
            case "reboots":
                sfx.PlayOneShot(reboots[Random.Range(0, reboots.Length)]);
                break;
            case "startRepair":
                sfx.PlayOneShot(startRepair[Random.Range(0, startRepair.Length)]);
                break;
            case "instructionCardComplete":
                sfx.PlayOneShot(instructionCardComplete[Random.Range(0, instructionCardComplete.Length)]);
                break;
            case "dies":
                sfx.PlayOneShot(dies[Random.Range(0, dies.Length)]);
                break;
            case "goodScreams":
                loudAudioSource.PlayOneShot(goodScreams[Random.Range(0, goodScreams.Length)]);
                break;
            case "badScreams":
                loudAudioSource.PlayOneShot(badScreams[Random.Range(0, badScreams.Length)]);
                break;
            case "wooshes":
                sfx.PlayOneShot(wooshes[Random.Range(0, wooshes.Length)]);
                break;
            case "steps":
                sfx.PlayOneShot(steps[Random.Range(0, steps.Length)]);
                break;
            case "beeps":
                sfx.PlayOneShot(beeps[Random.Range(0, beeps.Length)]);
                break;
            case "start":
                sfx.PlayOneShot(start[Random.Range(0, start.Length)]);
                break;
        }
    }
}
