using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawner : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Despawn(GameObject go, float inSeconds)
    {
        go.SetActive(false);

        GameManager.Instance.Timer.Add(() => {
            go.SetActive(true);
        }, inSeconds);
    }
}
