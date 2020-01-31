using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public AudioClip[] pushes;
    public AudioClip[] pulls;

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
            case "pushes":
                sfx.PlayOneShot(pushes[Random.Range(0, pushes.Length)]);
                break;
            case "pulls":
                sfx.PlayOneShot(pulls[Random.Range(0, pulls.Length)]);
                break;
        }
    }
}
