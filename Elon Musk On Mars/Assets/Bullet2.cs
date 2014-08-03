using UnityEngine;
using System.Collections;

public class Bullet2 : MonoBehaviour
{
	float createTime;
	public float destroyTime;

	// Use this for initialization
	void Start ()
	{
		createTime = Time.timeSinceLevelLoad;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Time.timeSinceLevelLoad > createTime + destroyTime)
			Destroy(gameObject);
	}
}
