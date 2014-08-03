using UnityEngine;
using System.Collections;

public class SetAlpha : MonoBehaviour
{
	public Shader shader;

	// Use this for initialization
	void Start ()
	{
		renderer.material.shader = Shader.Find("Transparent/Diffuse");
		renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, .25f);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
