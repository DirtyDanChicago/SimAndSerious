using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BeginScript : MonoBehaviour
{
    public void Begin()
    {
        SceneManager.LoadScene("Level1");
    }
	
}
