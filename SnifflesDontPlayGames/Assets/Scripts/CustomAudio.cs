using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomAudio : MonoBehaviour {

	public static void PlaySound(AudioClip clip, Vector3 location){
		AudioSource.PlayClipAtPoint (clip, location);
	}

}
