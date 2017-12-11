using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnDialogue : MonoBehaviour {

	//TODO ADD AUDIO
	public GameObject DialogueToSpawn;
	public float timeToDespawn = 2.0f;

	private bool activated = false;
	private GameObject dialogue;

	private void Awake()
	{
		Debug.Assert (DialogueToSpawn);
	}

	public void OnTriggerEnter2D(Collider2D other) 
	{
		if(other.gameObject.CompareTag("Player") && !activated)
		{
			activated = true;
			dialogue = GameObject.Instantiate (DialogueToSpawn, transform.position, Quaternion.identity);
			StartCoroutine (WaitInScreen (timeToDespawn));	
		}
	}

	IEnumerator WaitInScreen(float sToWait)
	{
		yield return new WaitForSeconds (sToWait);
		dialogue.SetActive (false);
	}
}
