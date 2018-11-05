using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScript : MonoBehaviour
{
    public void SceneSelect()
    {
        SceneManager.LoadScene("Level1");
    }
	
}
