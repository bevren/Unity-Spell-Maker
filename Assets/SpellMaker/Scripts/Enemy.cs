using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	// Use this for initialization
	public int health = 100;
	public TextMesh tm;

	void Start () {

		tm = GetComponentInChildren<TextMesh> ();
	
	}
	
	// Update is called once per frame
	void Update () {

		tm.text = "Health : " + health.ToString();
	
	}

	public void GetHit(int damage)
	{
		if(health <= 0)
		{
			transform.GetComponent<Animation>().CrossFade("die");
			health = 0;
			Destroy(this.gameObject,5);
		}

		health -= damage;

	}
}
