using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
	public int spd;
	public int dmg;
	public int range;
	public Vector3 vel;
	public Vector3 shootLoc;
	public GameObject shooter;

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
		rigidbody.velocity = vel;
		if (Vector3.Distance(shootLoc, transform.position) > range)
			Destroy(gameObject);
		if (transform.position.y < -10)
			Destroy(gameObject);
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject == shooter)
			return;
		if (other.gameObject.name == "Player")
		{
			other.gameObject.GetComponent<Player>().hp -= dmg;
			if (other.gameObject.GetComponent<Player>().hp <= 0)
				Application.LoadLevel(0);
		}
		Destroy (gameObject);
	}
}
