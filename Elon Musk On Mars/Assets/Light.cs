using UnityEngine;
using System.Collections;

public class Light : MonoBehaviour
{
	float createTime;
	public float duration;

	// Use this for initialization
	void Start ()
	{
		createTime = Time.timeSinceLevelLoad;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Time.timeSinceLevelLoad - createTime > duration)
			Destroy(gameObject);
	}
}
