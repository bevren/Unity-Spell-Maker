using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CastSpell : MonoBehaviour {

	private Transform myTransform;
	public Transform myTarget;
	public bool castingSpell = false;

	public List<Spell> spellList = new List<Spell> ();

	public int mana = 100;

	void Start()
	{
		myTransform = transform;

		//Adding spells to player or npc's spell list or you can do it manually in the inspector...
		List<Spell> spellDatabase = GameObject.Find ("SpellManager").GetComponent<SpellManager>().spellList;

		for(int i = 0; i < spellDatabase.Count; i++)
		{
			spellList.Add(spellDatabase[i]);
		}

	}

	void Update()
	{

		if (myTarget == null){
			Debug.LogWarning(myTransform.name + " " + "target is a null.");
			return;
		}

		if(Input.GetMouseButtonDown(0) && !castingSpell)
		{
			StartCoroutine(RandomSpellCast());
		}

		if(Input.GetMouseButtonDown(1) && !castingSpell)
		{
			StartCoroutine(SpellCast(spellList[0]));
		}

	}

	void SpellSetUp(Spell spell)
	{
		
		if(spell.spellPrefab == null)
		{
			Debug.LogWarning("Spell prefab is null.Assign a spell prefab.");
			return;
			
		}
		
		GameObject spellObject = null;

		//We will find what type of spell we are using
		//
		//********************************SINGLE*********************************************
		if(spell.spellType == Spell.SpellType.Single)
		{
			//Instantiated object will move straight forward.
			if(spell.spellDirection == Spell.SpellDirection.Directional)
			{
				spellObject = (GameObject)Instantiate(spell.spellPrefab,myTransform.position + myTransform.up,myTransform.rotation);
				spellObject.name = spell.spellName;
				
			}
			
			//Instantiated object will follow target.
			if(spell.spellDirection == Spell.SpellDirection.Follow)
			{
				spellObject = (GameObject)Instantiate(spell.spellPrefab,myTransform.position + myTransform.up,Quaternion.identity);
				spellObject.name = spell.spellName;
				spellObject.GetComponent<SpellObjectConfigurator>().myTarget = myTarget;
			}
			
			//Instantiating to target's position.
			if(spell.spellDirection == Spell.SpellDirection.Point)
			{
				spellObject = (GameObject)Instantiate(spell.spellPrefab,myTarget.position,Quaternion.identity);
				spellObject.name = spell.spellName;
				spellObject.GetComponent<SpellObjectConfigurator>().myTarget = myTarget;
			}
			
			
		}

		//********************************AOE*********************************************
		else if(spell.spellType == Spell.SpellType.Aoe)
		{
			if(spell.spellPosition == Spell.SpellPosition.TargetTransform)
				spellObject = (GameObject)Instantiate(spell.spellPrefab,myTarget.position,Quaternion.identity);
			else
				spellObject = (GameObject)Instantiate(spell.spellPrefab,myTransform.position,Quaternion.identity);
			
			spellObject.name = spell.spellName;

			
		}

		//********************************BUFF*********************************************
		else
		{
			//Spell type is a buff.And we are checking what type of buff spell is used.
			if(spell.buffType == Spell.BuffType.Heal)
			{
				spellObject = (GameObject)Instantiate(spell.spellPrefab,myTransform.position,Quaternion.identity);
				spellObject.name = spell.spellName;
				//currentHealth += (Random.Range(spell.minBuffAmount,spell.maxBuffAmount));	
				
			}
			else if(spell.buffType == Spell.BuffType.MagicalDefense)
			{
				spellObject = (GameObject)Instantiate(spell.spellPrefab,myTransform.position,Quaternion.identity);
				spellObject.name = spell.spellName;
				//magicalDefense += (Random.Range(spell.minBuffAmount,spell.maxBuffAmount));	
				
			}
			else
			{
				//Physical Defense
				spellObject = (GameObject)Instantiate(spell.spellPrefab,myTransform.position,Quaternion.identity);
				spellObject.name = spell.spellName;
				//physicalDefense += (Random.Range(spell.minBuffAmount,spell.maxBuffAmount));	
			}
		}
	}

	public IEnumerator RandomSpellCast()
	{
		//will choose random spell in own spell list.
		Spell randomSpell = spellList[Random.Range(0,spellList.Count)];
		
		//If not out of mana.
		if(randomSpell.spellManaCost <= mana){
			//casting spell.
			castingSpell = true;
			//Decrease mana
			mana -= randomSpell.spellManaCost;
			//Wait for choosen spell cast time.
			yield return new WaitForSeconds(randomSpell.spellCastTime);
			//Play the spell cast animation
			myTransform.GetComponent<Animation>().CrossFade("attack");
			//Set up a spell and cast it.
			SpellSetUp(randomSpell);
		}
		
		castingSpell = false;
		//Wait 1 second.
		yield break;
		//end.
	}

	public IEnumerator SpellCast(Spell spell)
	{
		//If not out of mana.
		if(spell.spellManaCost <= mana){
			//casting spell.
			castingSpell = true;
			//Decrease mana
			mana -= spell.spellManaCost;
			//Wait for choosen spell cast time.
			yield return new WaitForSeconds(spell.spellCastTime);
			//Play the spell cast animation
			myTransform.GetComponent<Animation>().CrossFade("attack");
			//Set up a spell and cast it.
			SpellSetUp(spell);
		}
		
		castingSpell = false;
		//Wait 1 second.
		yield break;
		//end.
	}

}
