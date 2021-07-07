using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour {
    

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.GetType() == typeof(BoxCollider2D))
        {
            CharacterScript player = collision.GetComponent<CharacterScript>();

            player.Injury();     
      
            Debug.Log("Player entered the trigger.");
           
        }
    }

}
