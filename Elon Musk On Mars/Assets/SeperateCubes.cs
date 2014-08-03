using UnityEngine;
using System.Collections;

public class SeperateCubes : MonoBehaviour
{
	public float cubeSize;
	GameObject go;
	
	void Awake ()
	{
		//Camera.main.renderingPath = RenderingPath.Forward;
		foreach (GameObject go2 in GameObject.FindGameObjectsWithTag("Seperate"))
		{
			Vector3 pos = go2.transform.position;
			Vector3 size2 = go2.transform.localScale;
			Vector3 size = size2;
			Vector3 rota = go2.transform.rotation.eulerAngles;
			if (Mathf.Round(rota.x) == 90)
				size = new Vector3(size2.x, size2.z, size2.y);
			if (Mathf.Round(rota.y) == 90)
				size = new Vector3(size2.z, size2.y, size2.x);
			if (Mathf.Round(rota.z) == 90)
				size = new Vector3(size2.y, size2.x, size2.z);
			for (float x = pos.x - size.x / 2 + cubeSize / 2; x <= pos.x + size.x / 2 - cubeSize / 2; x += cubeSize)
				for (float y = pos.y - size.y / 2 + cubeSize / 2; y <= pos.y + size.y / 2 - cubeSize / 2; y += cubeSize)
					for (float z = pos.z - size.z / 2 + cubeSize / 2; z <= pos.z + size.z / 2 - cubeSize / 2; z += cubeSize)
					{
						go = (GameObject) GameObject.Instantiate(go2);
						go.transform.position = new Vector3(x, y, z);
						go.transform.rotation = Quaternion.identity;
						go.transform.localScale = Vector3.one * cubeSize;
						go.tag = "Untagged";
					}
			Destroy(go2);
		}
	}
	
	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{

	}
}
