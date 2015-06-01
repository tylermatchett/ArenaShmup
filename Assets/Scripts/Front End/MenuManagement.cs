using UnityEngine;
using System.Collections;

public class MenuManagement : MonoBehaviour {
	public void LoadLevel(string level) {
		GameManager.Instance.LoadState(level);
	}

	public void ExitGame() {
		Application.Quit();
	}
}
