using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
		//Play Sound
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.anyKeyDown)
        {
            //Load Next Scene
            SceneManager.LoadScene("Main");
        }
    }
}
