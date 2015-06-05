using UnityEngine;
using System.Collections;

public class DontDestroyOnLoad : MonoBehaviour {

    public bool DestroyOnLoad = false;

    void Start() {
        if (!DestroyOnLoad)
            DontDestroyOnLoad(gameObject);
	}
}
