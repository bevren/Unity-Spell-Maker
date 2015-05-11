using UnityEngine;
using System.Collections;

public class DamageByFlag : MonoBehaviour {

	public bool check = false;
	public bool resetDps = false;
	public bool stunned = false;

	public IEnumerator TakeDamageByFlagType(Spell spell,Transform target)
	{
		if(spell.spellFlag == Spell.SpellFlag.Slow)
		{
			//This is for testing only.You can implement your own characters movement logic script very easily.
			//Yourscript script = this.gameObject.GetComponent<Yourscript>();
			//yourscript.movespeed = .... /etc.;
			/*
			ThirdPersonController cont = this.gameObject.GetComponent<ThirdPersonController>();
			cont.runSpeed = 1.5f;
			cont.walkSpeed = 0.5f;
			yield return new WaitForSeconds(spell.slowDuration);
			cont.runSpeed = 6.0f;
			cont.walkSpeed = 2.0f;
			yield break;
			*/
			Debug.Log("Slowed");

		}

		else if(spell.spellFlag == Spell.SpellFlag.DamagePerSecond)
		{
			if(resetDps && check){
				check = false;
				resetDps = false;
				StopAllCoroutines();
			}

			if(!check)
				StartCoroutine(DOT(spell.dotDamage,spell.dotTick,spell.dotSeconds,spell.dotEffect,target));

		}

		else
		{
			Debug.Log("don't have spell flag.");
			yield break;
		}
		
		
	}


	public IEnumerator DOT(int damage,int over,int time,GameObject dotEffect,Transform target)
	{

		int count = 0;

		check = true;


		while (count < over){
			yield return new WaitForSeconds(time);
			//Do (damage over time)damage
			//target.GetComponent<HealthScript>().health -= damage;
			target.gameObject.GetComponent<Enemy>().GetHit(damage);
			Instantiate(dotEffect,target.position,Quaternion.identity);
			count ++;

		}

		check = false;
	}


}
