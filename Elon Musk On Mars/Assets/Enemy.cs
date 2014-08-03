using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour 
{
	float atkTimer;
	public float atkRate;
	public float dmg;
	public GameObject bullet;
	GameObject go;
	public float hp;
	public int moveSpd;
	bool dead;
	bool canHurtPlayer;
	RaycastHit hit;
	public string moveAnim;
	public string damageAnim;
	public string deathAnim;
	public string attackAnim;
	public int atkRangeMax;
	public float atkRangeMin;
	public Transform bulletSpawn;
	public bool retreatIfClose;
	bool toClose;
	bool toFar;
	public LayerMask notEnemy;
	bool dead2;

	// Use this for initialization
	void Start ()
	{
		atkTimer = Time.timeSinceLevelLoad + atkRate;
		if (bullet != null)
		{
			atkRangeMax = bullet.GetComponent<Bullet>().range - 1;
			atkRangeMin = atkRangeMax / 2;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (dead2)
			return;
		Vector3 toPlayer = GameObject.Find("Player").transform.position - transform.position;
		toClose = Vector3.Distance(GameObject.Find("Player").transform.position, transform.position) < atkRangeMin;
		Ray ray = new Ray(transform.position + (Vector3.up * transform.lossyScale.y * .5f), toPlayer);
		toFar = Vector3.Distance(GameObject.Find("Player").transform.position, transform.position) > atkRangeMax || !(Physics.Raycast(ray, out hit, 1000, notEnemy) && hit.collider != null && hit.collider.name == "Player");
		if (dead)
		{
			if (!animation.IsPlaying(deathAnim))
			{
				GameObject.Find("Player").GetComponent<Player>().score += 10;
				dead2 = true;
				//Destroy(gameObject);
			}
			return;
		}
		if (!animation.IsPlaying(attackAnim))
			transform.rotation = Quaternion.LookRotation(new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z), Vector3.up);
		else
			transform.rotation = Quaternion.LookRotation(new Vector3(toPlayer.x, 0, toPlayer.z), Vector3.up);
		if (Time.timeSinceLevelLoad > atkTimer && !toFar && !(toClose && retreatIfClose))
		{
			if (!animation.IsPlaying(attackAnim) && !animation.IsPlaying(damageAnim))
			{
				animation.Play(attackAnim);
				atkTimer += atkRate;
				canHurtPlayer = true;
				if (bullet != null)
					StartCoroutine("Shoot", .5);
			}
		}
		else if ((toFar || toClose) && !animation.IsPlaying(attackAnim) && !animation.IsPlaying(damageAnim))
			animation.Play(moveAnim);
		Vector3 vel = new Vector3(toPlayer.x, 0, toPlayer.z);
		vel *= 9999999999;
		if (retreatIfClose)
		{
			if (toFar)
				vel = Vector3.ClampMagnitude(vel, moveSpd);
			else if (toClose)
				vel = -Vector3.ClampMagnitude(vel, moveSpd);
			else
				vel = Vector3.zero;
		}
		else
			vel = Vector3.ClampMagnitude(vel, moveSpd);
		//transform.rotation = Quaternion.LookRotation(vel, Vector3.up);
		vel += Vector3.up * rigidbody.velocity.y;
		rigidbody.velocity = vel;
		if (transform.position.y < -10)
			Destroy(gameObject);
	}

	void OnCollisionEnter (Collision collision)
	{
		if (dead)
			return;
		if (collision.relativeVelocity.magnitude > 5 && collision.gameObject.name == "Bullet(Clone)")
		{
			rigidbody.drag = 9999999999;
			rigidbody.angularDrag = 9999999999;
			Destroy(collision.gameObject);
			hp -= 1;
			if (hp <= 0)
			{
				rigidbody.drag = 9999999999;
				rigidbody.angularDrag = 9999999999;
				animation.Play(deathAnim);
				transform.Find("MapIcon").renderer.enabled = false;
				dead = true;
				if (GameObject.Find("Player").GetComponent<Player>().flying)
				{
					GameObject.Find("Player").GetComponent<Player>().StartCoroutine("ChangeBool", 2);
					GameObject.Find("Player").GetComponent<Player>().score += 25;
				}
				if (Vector3.Distance(transform.position, GameObject.Find("Player").transform.position) > 100)
				{
					GameObject.Find("Player").GetComponent<Player>().StartCoroutine("ChangeBool2", 2);
					GameObject.Find("Player").GetComponent<Player>().score += 25;
				}
				return;
			}
			animation.Play(damageAnim);
			rigidbody.drag = 0;
			rigidbody.angularDrag = .05f;
		}
	}

	void HandleTriggerStay (Collider[] selfAndOther)
	{
		Collider other = selfAndOther[1];
		if (other.name == "Player" && animation.IsPlaying(attackAnim) && canHurtPlayer)
		{
			canHurtPlayer = false;
			other.GetComponent<Player>().hp -= dmg;
			if (other.GetComponent<Player>().hp <= 0)
				Application.LoadLevel(0);
		}
	}

	IEnumerator Shoot (float delay)
	{
		yield return new WaitForSeconds(delay);
		go = (GameObject) GameObject.Instantiate(bullet, bulletSpawn.position, Quaternion.identity);
		go.GetComponent<Bullet>().shooter = gameObject;
		go.GetComponent<Bullet>().vel = GameObject.Find("Player").transform.position - bulletSpawn.position;
		go.GetComponent<Bullet>().vel *= 9999999999;
		go.GetComponent<Bullet>().vel = Vector3.ClampMagnitude(go.GetComponent<Bullet>().vel, go.GetComponent<Bullet>().spd);
		go.GetComponent<Bullet>().shootLoc = bulletSpawn.position;
	}
}
