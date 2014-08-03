using UnityEngine;
using System.Collections;

public class PassUpCollisions : MonoBehaviour
{
	public ArrayList triggerColliders = new ArrayList();
	public ArrayList colliders = new ArrayList();
	public bool passToRoot;

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
		//TriggerExit();
	}

	void OnTriggerEnter (Collider other)
	{
		Collider[] tempColliders = new Collider[2];
		tempColliders[0] = collider;
		tempColliders[1] = other;
		triggerColliders.Add(other);
		if (!passToRoot)
			transform.parent.SendMessage("HandleTriggerEnter", tempColliders, SendMessageOptions.DontRequireReceiver);
		else
			transform.root.SendMessage("HandleTriggerEnter", tempColliders, SendMessageOptions.DontRequireReceiver);
	}

	void OnTriggerStay (Collider other)
	{
		Collider[] tempColliders = new Collider[2];
		tempColliders[0] = collider;
		tempColliders[1] = other;
		if (!passToRoot)
			transform.parent.SendMessage("HandleTriggerStay", tempColliders, SendMessageOptions.DontRequireReceiver);
		else
			transform.root.SendMessage("HandleTriggerStay", tempColliders, SendMessageOptions.DontRequireReceiver);
	}

	void TriggerExit ()
	{
		Collider[] tempColliders = new Collider[2];
		ArrayList removeColliders = new ArrayList();
		tempColliders[0] = collider;
		foreach (Collider c in triggerColliders)
		{
			if (c.transform.position != transform.position)
			{
				tempColliders[1] = c;
				transform.parent.SendMessage("HandleTriggerExit", tempColliders, SendMessageOptions.DontRequireReceiver);
				removeColliders.Add(c);
			}
		}
		foreach (Collider c in removeColliders)
			triggerColliders.Remove(c);
		//if (colliders.Count == 0)
		//	transform.parent.SendMessage("HandleTriggerExit", tempColliders, SendMessageOptions.DontRequireReceiver);
	}

	void OnCollisionEnter (Collision collision)
	{
		Collider[] tempColliders = new Collider[2];
		tempColliders[0] = collider;
		tempColliders[1] = collision.collider;
		colliders.Add(collision.collider);
		transform.parent.SendMessage("HandleCollisionEnter", tempColliders, SendMessageOptions.DontRequireReceiver);
	}

	void OnCollisionStay (Collision collision)
	{
		Collider[] tempColliders = new Collider[2];
		tempColliders[0] = collider;
		tempColliders[1] = collision.collider;
		transform.parent.SendMessage("HandleCollisionStay", tempColliders, SendMessageOptions.DontRequireReceiver);
	}

	void CollisionExit ()
	{
		Collider[] tempColliders = new Collider[2];
		ArrayList removeColliders = new ArrayList();
		tempColliders[0] = collider;
		foreach (Collider c in colliders)
		{
			if (c.transform.position != transform.position)
			{
				tempColliders[1] = c;
				transform.parent.SendMessage("HandleTriggerExit", tempColliders, SendMessageOptions.DontRequireReceiver);
				removeColliders.Add(c);
			}
		}
		foreach (Collider c in removeColliders)
			colliders.Remove(c);
		//if (colliders.Count == 0)
		//	transform.parent.SendMessage("HandleTriggerExit", tempColliders, SendMessageOptions.DontRequireReceiver);
	}

	void HandleTriggerEnter (Collider[] selfAndOther)
	{
		Collider[] tempColliders = new Collider[2];
		tempColliders[0] = collider;
		tempColliders[1] = null;
		transform.parent.SendMessage("HandleTriggerStay", tempColliders, SendMessageOptions.DontRequireReceiver);
	}

	void HandleTriggerStay (Collider[] selfAndOther)
	{
		Collider[] tempColliders = new Collider[2];
		tempColliders[0] = collider;
		tempColliders[1] = null;
		transform.parent.SendMessage("HandleTriggerStay", tempColliders, SendMessageOptions.DontRequireReceiver);
	}

	void HandleTriggerExit (Collider[] selfAndOther)
	{
		Collider[] tempColliders = new Collider[2];
		tempColliders[0] = collider;
		tempColliders[1] = null;
		transform.parent.SendMessage("HandleTriggerStay", tempColliders, SendMessageOptions.DontRequireReceiver);
	}
}
