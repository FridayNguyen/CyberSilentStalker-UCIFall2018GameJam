using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dontDestoryAudio : MonoBehaviour {

	public AudioClip MusicClip;

	public AudioSource MusicSource;
	// Use this for initialization
	void Start () {
		MusicSource.clip = MusicClip;
		MusicSource.Play();
	}
	
	// Update is called once per frame
	private static dontDestoryAudio instance = null;

	public static dontDestoryAudio Instance
	{ get {return instance; }}
	void Awake () {
		if(instance != null && instance != this)
		{
			Destroy(this.gameObject);
			return;
		}
		else
		{
			instance = this;
		}
		DontDestroyOnLoad(this.gameObject);
	}
}
