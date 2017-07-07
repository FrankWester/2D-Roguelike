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

    IEnumerator WaitForSeconds()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Main");
    }

    // Update is called once per frame
    void Update ()
    {
		if(Input.anyKeyDown)
        {
            //Load Next Scene
            WaitForSeconds();

        }
    }
}
