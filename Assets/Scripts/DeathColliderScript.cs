using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathColliderScript : MonoBehaviour
{
    private BoxCollider2D boxCollider2d;

    private void Start()
    {
       boxCollider2d = GetComponent<BoxCollider2D>();
    }


    

}
