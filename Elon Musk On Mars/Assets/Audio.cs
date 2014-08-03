using UnityEngine;
using System.Collections;

public class Audio : MonoBehaviour
{
	public float bpm = 140.0F;
	public int numBeatsPerSegment = 16;
	public AudioClip[] clips;
	private double nextEventTime;
	private int flip = 0;
	private AudioSource[] audioSources;
	private bool running = false;
	void Awake() {
		int i = 0;
		while (i < clips.Length) {
			GameObject child = new GameObject("Player");
			child.transform.parent = gameObject.transform;
			audioSources[i] = (AudioSource) child.AddComponent("AudioSource") as AudioSource;
			i++;
		}
		nextEventTime = AudioSettings.dspTime + 2.0F;
		running = true;
		DontDestroyOnLoad(gameObject);
	}
	void Update() {
		transform.position = Camera.main.transform.position;
		if (!running)
			return;
		
		double time = AudioSettings.dspTime;
		if (time + 1.0F > nextEventTime) {
			audioSources[flip].clip = clips[flip];
			audioSources[flip].PlayScheduled(nextEventTime);
			Debug.Log("Scheduled source " + flip + " to start at time " + nextEventTime);
			nextEventTime += 60.0F / bpm * numBeatsPerSegment;
			flip = 1 - flip;
		}
	}

	void OnGUI ()
	{
		if (Camera.main.GetComponent<AudioListener>().enabled && GUI.Button(new Rect(0, Screen.height - 100, 200, 50), "Mute"))
			Camera.main.GetComponent<AudioListener>().enabled = false;
		else if (!Camera.main.GetComponent<AudioListener>().enabled && GUI.Button(new Rect(0, Screen.height - 100, 200, 50), "Unmute"))
			Camera.main.GetComponent<AudioListener>().enabled = true;
	}
}
