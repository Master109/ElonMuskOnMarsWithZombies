using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour
{
	public Vector3 pos;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.position = pos;
	}
}
