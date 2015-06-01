using UnityEngine;
using System.Collections;

abstract public class ActorInputManager : MonoBehaviour {

	public ActorInputManager () {
	
	}

	abstract public void CheckForInput();
}
