using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyText : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine("DestructText");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public IEnumerator DestructText()
    {
        yield return new WaitForSeconds(10f);
        Destroy(GameObject.FindGameObjectWithTag("text"));
    }
}
