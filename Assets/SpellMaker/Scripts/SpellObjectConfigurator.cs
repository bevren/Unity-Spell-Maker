using UnityEngine;
using System.Collections;

public class SpellObjectConfigurator : MonoBehaviour {
	
	private Transform myTransform = null;
	public Spell spell = null;
	public Transform myTarget = null;
	
	void Start()
	{
		myTransform = transform;
		spell = (Spell)Resources.Load("Spells/"+myTransform.gameObject.name,typeof(Spell));

		if(spell != null)
		{
			if(spell.spellType == Spell.SpellType.Single){

				if(spell.spellDirection == Spell.SpellDirection.Point)
				{
					DamageByFlag damageByFlag = myTarget.gameObject.GetComponent<DamageByFlag>();

					if(spell.spellFlag == Spell.SpellFlag.DamagePerSecond){

						//You can implement a your own damage script.This is an example.(col) means a player.
						//PlayerDamageScript pds = col.gameObject.GetComponent<PlayerDamageScript>();
						//pds.TakeDamage(); or pds.health -= damage;
						myTarget.gameObject.GetComponent<Enemy>().GetHit(Random.Range(spell.spellMinDamage,spell.spellMaxDamage));

						if(damageByFlag && damageByFlag.check == false)
							damageByFlag.StartCoroutine(damageByFlag.TakeDamageByFlagType(spell,myTarget));
						else{
							damageByFlag.resetDps = true;
							damageByFlag.StartCoroutine(damageByFlag.TakeDamageByFlagType(spell,myTarget));
						}

					}
					else
					{
						damageByFlag.StartCoroutine(damageByFlag.TakeDamageByFlagType(spell,myTarget));
					}
				}
			}

			if(spell.spellType == Spell.SpellType.Aoe)
			{

				Collider[] hitColliders = Physics.OverlapSphere(myTransform.position,5.0f);
				
				//for each collider in that radius will take damage
				for(int i = 0; i < hitColliders.Length;i++)
				{
					if(hitColliders[i].tag == "Enemy"){
						Instantiate(spell.spellCollisionParticle,hitColliders[i].transform.position,Quaternion.identity);

						//You can implement a your own damage script.This is an example.(col) means a player.
						//PlayerDamageScript pds = col.gameObject.GetComponent<PlayerDamageScript>();
						//pds.TakeDamage(); or pds.health -= damage;
						hitColliders[i].gameObject.GetComponent<Enemy>().GetHit(Random.Range(spell.spellMinDamage,spell.spellMaxDamage));


					}
				}
				//if spell type is aoe and spell flag is a damage over time
				if(spell.spellFlag == Spell.SpellFlag.DamagePerSecond){

					//for each collider in that radius will take damage
					for(int i = 0; i < hitColliders.Length;i++)
					{
						if(hitColliders[i].tag == "Enemy"){
							DamageByFlag damageByFlag = hitColliders[i].gameObject.GetComponent<DamageByFlag>();
							
							if(damageByFlag && damageByFlag.check == false)
								damageByFlag.StartCoroutine(damageByFlag.TakeDamageByFlagType(spell,hitColliders[i].transform));
							else{
								damageByFlag.resetDps = true;
								damageByFlag.StartCoroutine(damageByFlag.TakeDamageByFlagType(spell,hitColliders[i].transform));
							}
						}
					}

				}

				else if(spell.spellFlag == Spell.SpellFlag.Slow)
				{
					for(int i = 0; i < hitColliders.Length;i++)
					{
						if(hitColliders[i].tag == "Enemy"){
							DamageByFlag damageByFlag = hitColliders[i].gameObject.GetComponent<DamageByFlag>();

							damageByFlag.StartCoroutine(damageByFlag.TakeDamageByFlagType(spell,hitColliders[i].transform));
						}

					}

				}
			}

		}

	}
	
	void Update()
	{
		if(spell != null){
			if(spell.spellType == Spell.SpellType.Single)
			{
				//Instantiated object will move straight forward.
				if(spell.spellDirection == Spell.SpellDirection.Directional)
				{
					MoveStraightForward();
				}
				
				//Instantiated object will follow target.
				if(spell.spellDirection == Spell.SpellDirection.Follow)
				{
					FollowTarget();
				}


			}
		}

		
	}
	
	
	public void MoveStraightForward()
	{
		myTransform.Translate(new Vector3(0,0,spell.projectileSpeed * Time.deltaTime));
	}
	
	public void FollowTarget()
	{
		myTransform.TransformDirection(Vector3.forward);
		myTransform.Translate(new Vector3(0,0,spell.projectileSpeed * Time.deltaTime));
		myTransform.rotation = Quaternion.Slerp(myTransform.rotation,
											    Quaternion.LookRotation(myTarget.position - myTransform.position),
											    5 * Time.deltaTime);
		
	}
	
	void OnCollisionEnter(Collision col)
	{
		if(col.gameObject.tag == "Enemy")
		{

			ContactPoint cp = col.contacts[0];

			Instantiate(spell.spellCollisionParticle,cp.point,Quaternion.identity);
			
			DamageByFlag damageByFlag = col.gameObject.GetComponent<DamageByFlag>();

			if(spell.spellFlag == Spell.SpellFlag.DamagePerSecond){

				//You can implement a your own damage script.This is an example.(col) means a enemy.
				//DamageScript ds = col.gameObject.GetComponent<DamageScript>();
				//ds.TakeDamage(damage); or ds.health -= damage;
				myTarget.gameObject.GetComponent<Enemy>().GetHit(Random.Range(spell.spellMinDamage,spell.spellMaxDamage));

				//This is for dot only.
				if(damageByFlag && damageByFlag.check == false)
					damageByFlag.StartCoroutine(damageByFlag.TakeDamageByFlagType(spell,myTarget));
				else{
					damageByFlag.resetDps = true;
					damageByFlag.StartCoroutine(damageByFlag.TakeDamageByFlagType(spell,myTarget));
				}
			}
			else
			{
				if(damageByFlag)
					damageByFlag.StartCoroutine(damageByFlag.TakeDamageByFlagType(spell,myTarget));
				else
					Debug.LogWarning("The DamageByFlag script not found in" + " " + col.gameObject.name + ".Please assign the DamageByFlag script.");
			}

			//You can implement a your own damage script.This is an example.(col) means a enemy in this sitiuation.
			//PlayerDamageScript pds = col.gameObject.GetComponent<PlayerDamageScript>();
			//pds.TakeDamage(); or pds.health -= damage;
			col.gameObject.GetComponent<Enemy>().GetHit(Random.Range(spell.spellMinDamage,spell.spellMaxDamage));

			Destroy(this.gameObject);
			
		}
	}
	
	
	
}
