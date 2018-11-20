using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishScript : MonoBehaviour
{
    public void SceneSelect()
    {
        SceneManager.LoadScene("SampleScene");
    }

}
