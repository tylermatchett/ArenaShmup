using UnityEngine;
using System.Collections;
using InControl;

public class Player {
	public int PlayerNumber;
	public InputDevice device;
	public Character character;
	public int TeamID;
	public bool Ready;

	public Profile profile;

	public Player(int pn, InputDevice d) {
		PlayerNumber = pn;
		device = d;
		TeamID = pn;
		Ready = false;
		character = GameManager.Instance.characterList[0];

		profile = new Profile("Player" + pn.ToString());
	}
}
