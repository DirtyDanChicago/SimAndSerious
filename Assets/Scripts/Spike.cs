using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour {

<<<<<<< HEAD

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.GetType() == typeof(BoxCollider2D))
        {
            CharacterScript player = collision.GetComponent<CharacterScript>();

            player.Injury();     
      
            Debug.Log("Player entered the trigger.");
           
        }
    }
=======
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
>>>>>>> parent of e12d0d4... Fixed sorting layer, and added spike script detection
}
