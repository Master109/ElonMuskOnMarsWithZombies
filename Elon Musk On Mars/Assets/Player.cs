using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	public int speed;
	public int jumpForce;
	public float hp;
	float atkTimer;
	public float atkRate;
	public float damage;
	float chamberRotateAmount;
	public float chamberRotateAmountMax;
	public float chamberAngularDrag;
	public GameObject bullet;
	GameObject go;
	RaycastHit hit;
	public GameObject enemy;
	public GameObject enemy2;
	public float[] enemyCreateRates;
	public float[] enemyCreateTimers;
	public int mapSize;
	public float enemyCreateRatesMultiplier;
	public float score;
	public GUISkin guiSkin;
	public bool flying;
	bool showAieralKill;
	bool showSniped;
	float snipedEndTime;
	float aieralKillEndTime;
	float snipedStartTime;
	float aieralKillStartTime;
	float surviveStartTime;
	float surviveEndTime;
	float surviveTimer;
	string surviveMessage;
	bool showSurvive;
	public GameObject wall;
	public float money;
	public GameObject light;
	NavMeshAgent agent;

	// Use this for initialization
	void Start ()
	{
		//agent = GetComponent<NavMeshAgent>();
		atkTimer = Time.timeSinceLevelLoad + atkRate;
		for (int i = 0; i < enemyCreateTimers.Length; i ++)
		{
			float f = (float) enemyCreateTimers[i];
			f = Time.timeSinceLevelLoad + (float) enemyCreateRates[i];
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		score += Time.deltaTime;
		money += Time.deltaTime;
		GameObject.Find ("Chamber").transform.RotateAround(GameObject.Find ("Chamber").transform.position, GameObject.Find ("Chamber").transform.right, chamberRotateAmount);
		chamberRotateAmount *= 1 / chamberAngularDrag;
		if (Physics.CheckSphere(transform.position + (Vector3.down * (transform.lossyScale.y / 2)), 1f , 1))
		{
			flying = false;
			rigidbody.velocity = (transform.right * Input.GetAxis("Horizontal") * speed) + (transform.forward * Input.GetAxis("Vertical") * speed) + (Vector3.up * rigidbody.velocity.y);
			if (Input.GetKeyDown(KeyCode.Space))
				rigidbody.AddForce(Vector3.up * jumpForce);
				//rigidbody.velocity += Vector3.up * jumpForce;
		}
		else
		{
			Ray ray2 = new Ray(transform.position, Vector3.down);
			if (Physics.Raycast(ray2, out hit) && hit.collider != null && Vector3.Distance(transform.position, hit.point) > 20)
				flying = true;
			else
				flying = false;
		}
		if (Input.GetKey (KeyCode.LeftShift))
			speed = 20;
		else
			speed = 10;
		if (Time.timeSinceLevelLoad > atkTimer && Input.GetKey(KeyCode.Mouse0))
		{
			atkTimer = Time.timeSinceLevelLoad + atkRate;
			chamberRotateAmount = chamberRotateAmountMax;
			go = (GameObject) GameObject.Instantiate(bullet, GameObject.Find("ShootPoint").transform.position, Quaternion.Euler(GameObject.Find("ShootPoint").transform.rotation.eulerAngles.x, GameObject.Find("ShootPoint").transform.rotation.eulerAngles.y, GameObject.Find("ShootPoint").transform.rotation.eulerAngles.z - 90));
			go.rigidbody.AddForce(go.transform.up * 7500);
			go = (GameObject) GameObject.Instantiate(light, GameObject.Find("ShootPoint").transform.position, Quaternion.identity);
			go = (GameObject) GameObject.Instantiate(bullet, GameObject.Find("ShootPoint2").transform.position, Quaternion.Euler(GameObject.Find("ShootPoint").transform.rotation.eulerAngles.x, GameObject.Find("ShootPoint").transform.rotation.eulerAngles.y, GameObject.Find("ShootPoint").transform.rotation.eulerAngles.z - 90));
			float maxMagnitude = 500;
			float maxMagnitude2 = 7500;
			go.rigidbody.AddForce(go.transform.up * 0 + new Vector3(Random.Range(-maxMagnitude, maxMagnitude), Random.Range(-maxMagnitude, maxMagnitude), Random.Range(-maxMagnitude, maxMagnitude)));
			go.rigidbody.AddTorque(new Vector3(Random.Range(-maxMagnitude2, maxMagnitude2), Random.Range(-maxMagnitude2, maxMagnitude2), Random.Range(-maxMagnitude2, maxMagnitude2)));
		}
		/*
		if (Input.GetKeyDown(KeyCode.Mouse1) && money > wall.GetComponent<Barricade>().cost)
		{
			go = (GameObject) GameObject.Instantiate(wall, transform.position + (transform.forward * 5), Quaternion.LookRotation(transform.forward));
			money -= wall.GetComponent<Barricade>().cost;
		}
		*/
		Ray ray = new Ray(GameObject.Find("ShootPoint").transform.position, GameObject.Find("ShootPoint").transform.right);
		if (Physics.Raycast(ray, out hit) && hit.collider != null && hit.collider.name != "Bullet(Clone)")
			GameObject.Find("Crosshair").GetComponent<Crosshair>().pos = Camera.main.WorldToViewportPoint(ray.GetPoint(Vector3.Distance(GameObject.Find("ShootPoint").transform.position, hit.point)));
		if (Time.timeSinceLevelLoad > enemyCreateTimers[0])
		{
			enemyCreateTimers[0] = Time.timeSinceLevelLoad + enemyCreateRates[0];
			enemyCreateRates[0] *= enemyCreateRatesMultiplier;
			Vector3 createLoc = new Vector3(Random.Range(-mapSize, mapSize), -100, Random.Range(-mapSize, mapSize));
			while (Vector3.Distance(transform.position, createLoc) < 100)
				createLoc = new Vector3(Random.Range(-mapSize, mapSize), -100, Random.Range(-mapSize, mapSize));
			ray = new Ray(createLoc, Vector3.up);
			if (Physics.Raycast(ray, out hit))
				createLoc = hit.point + (Vector3.up * 2);
			go = (GameObject) GameObject.Instantiate(enemy, createLoc, Quaternion.identity);
			if (Random.Range(0, 10) < 1)
			{
				go.transform.localScale *= 2;
				go.GetComponent<Enemy>().hp *= 2;
			}
		}
		if (Time.timeSinceLevelLoad > enemyCreateTimers[1])
		{
			enemyCreateTimers[1] = Time.timeSinceLevelLoad + enemyCreateRates[1];
			enemyCreateRates[1] *= enemyCreateRatesMultiplier;
			Vector3 createLoc = new Vector3(Random.Range(-mapSize, mapSize), -100, Random.Range(-mapSize, mapSize));
			while (Vector3.Distance(transform.position, createLoc) < 100)
				createLoc = new Vector3(Random.Range(-mapSize, mapSize), -100, Random.Range(-mapSize, mapSize));
			ray = new Ray(createLoc, Vector3.up);
			if (Physics.Raycast(ray, out hit))
				createLoc = hit.point + (Vector3.up * 2);
			go = (GameObject) GameObject.Instantiate(enemy2, createLoc, Quaternion.identity);
			if (Random.Range(0, 10) < 1)
			{
				go.transform.localScale *= 2;
				go.GetComponent<Enemy>().hp *= 2;
			}
		}
		if (transform.position.y < -10)
			Application.LoadLevel(0);
		surviveTimer += Time.deltaTime;
		if (surviveTimer > 10)
		{
			float size = 150 * (1 / (snipedEndTime - Time.timeSinceLevelLoad)) + 250;
			int r = Mathf.RoundToInt(Random.Range(1, 4));
			if (r == 1)
				surviveMessage = "Keep it up!";
			else if (r == 2)
				surviveMessage = "You rock!";
			else if (r == 3)
				surviveMessage = "You're doing well!";
			StartCoroutine("ChangeBool3", 2);
			surviveTimer = 0;
		}
	}
	
	void OnCollisionEnter (Collision collision)
	{
		if (collision.gameObject.name == "Fireball(Clone)")
		{
			hp -= collision.gameObject.GetComponent<Bullet>().dmg;
			if (hp <= 0)
				Application.LoadLevel(0);
		}
	}
	
	void OnGUI ()
	{
		GUI.skin = guiSkin;
		GUI.Box(new Rect(0, 0, 500, 105), "");
		if (Mathf.RoundToInt(score) > PlayerPrefs.GetInt("Score", 0))
			PlayerPrefs.SetInt("Score", Mathf.RoundToInt(score));
		GUI.Label(new Rect(0, 0, 9999999999, 100), "Best score: " + PlayerPrefs.GetInt("Score", 0));
		GUI.Label(new Rect(0, 30, 9999999999, 100), "Score: " + Mathf.RoundToInt(score));
		//GUI.Label(new Rect(0, 60, 9999999999, 100), "$" + Mathf.RoundToInt(money));
		GUI.Label(new Rect(0, 60, 9999999999, 100), "Health: " + Mathf.RoundToInt(hp));
		if (showAieralKill)
		{
			float size = 150 * (1 / (aieralKillEndTime - Time.timeSinceLevelLoad)) + 250;
			GUI.Box(new Rect(Screen.width / 2 - size / 2, Screen.height / 2 - 25, size, 50), "AERIAL KILL!");
		}
		if (showSniped)
		{
			float size = 150 * (1 / (snipedEndTime - Time.timeSinceLevelLoad)) + 250;
			GUI.Box(new Rect(Screen.width / 2 - size / 2, Screen.height / 2 + 25, size, 50), "SNIPED!");
		}
		if (showSurvive)
		{
			float size = 150 * (1 / (surviveEndTime - Time.timeSinceLevelLoad)) + 250;
			GUI.Box(new Rect(Screen.width / 2 - size / 2, Screen.height / 2 + 75, size, 50), surviveMessage);
		}
	}
	
	public IEnumerator ChangeBool (float duration)
	{
		showAieralKill = true;
		aieralKillStartTime = Time.timeSinceLevelLoad;
		aieralKillEndTime = aieralKillStartTime + duration;
		yield return new WaitForSeconds(duration);
		showAieralKill = false;
	}
	
	public IEnumerator ChangeBool2 (float duration)
	{
		showSniped = true;
		snipedStartTime = Time.timeSinceLevelLoad;
		snipedEndTime = snipedStartTime + duration;
		yield return new WaitForSeconds(duration);
		showSniped = false;
	}
	
	public IEnumerator ChangeBool3 (float duration)
	{
		showSurvive = true;
		surviveStartTime = Time.timeSinceLevelLoad;
		surviveEndTime = surviveStartTime + duration;
		yield return new WaitForSeconds(duration);
		showSurvive = false;
	}
}
