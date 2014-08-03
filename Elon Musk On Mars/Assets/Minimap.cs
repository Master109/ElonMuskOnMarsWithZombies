using UnityEngine;
using System.Collections;

public class Minimap : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.position = GameObject.Find("Player").transform.position + (Vector3.up * 500);
	}
}
