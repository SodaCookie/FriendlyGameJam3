using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour {

	public AudioClip jumpSound;
	public AudioClip bendSound;
	public AudioClip footSound;
	public AudioSource playerAudioSource;
	public AudioSource playerFootSource;

	public void Footstep() {
		playerFootSource.PlayOneShot(footSound);
	}

	public void Bend()
    {
		playerAudioSource.PlayOneShot(bendSound);
    }

	public void Jump()
    {
		playerAudioSource.PlayOneShot(jumpSound);
    }
}
