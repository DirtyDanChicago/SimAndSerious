using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            CharacterScript player = collision.GetComponent<CharacterScript>();

            Debug.Log("Player entered the trigger.");

           
        }
    }
}
