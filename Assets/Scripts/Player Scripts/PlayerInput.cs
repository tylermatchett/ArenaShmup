using UnityEngine;
using System.Collections;
using InControl;

public class PlayerInput : MonoBehaviour {

	InputDevice device;
	PlayerManager playerManager;

	// TODO Remove the sendmessages and target specific player scripts
	// or send all the data and just have all scripts reference the playermanager for the data they need

	void Start() {
		playerManager = GetComponent<PlayerManager>();
	}

	void Update () {
		playerManager.player.profile.inputActions.Device = playerManager.player.device;

		switch (playerManager.playerState) {
		case PlayerManager.PlayerState.Alive:
			// Handle Alive input based on profile data
			CheckForAliveInput ();
			break;
		case PlayerManager.PlayerState.Dead:
			// Handle input while dead
			break;
		case PlayerManager.PlayerState.Paused:
			// Handle paused input
			break;
		}
	}

	void CheckForAliveInput() {
		// Update movement direction
		gameObject.BroadcastMessage("MoveDirectionUpdate", playerManager.player.profile.inputActions.Move.Value, SendMessageOptions.DontRequireReceiver);
		
		// Update aim direction
		gameObject.BroadcastMessage("AimDirectionUpdate", playerManager.player.profile.inputActions.Aim.Value, SendMessageOptions.DontRequireReceiver);
		
		// send commands that were pressed
		if (playerManager.player.profile.inputActions.Boost.WasPressed) {
			gameObject.SendMessage("Boost", SendMessageOptions.DontRequireReceiver);
		}
		if (playerManager.player.profile.inputActions.Ability.WasPressed) {
			gameObject.SendMessage("AbilityStart", SendMessageOptions.DontRequireReceiver);
		}
		if (playerManager.player.profile.inputActions.Ability.WasReleased) {
			gameObject.SendMessage("AbilityEnd", SendMessageOptions.DontRequireReceiver);
		}
		if (playerManager.player.profile.inputActions.Reload.WasPressed) {
			gameObject.SendMessage("Reload", SendMessageOptions.DontRequireReceiver);
		}
		if (playerManager.player.profile.inputActions.Melee.WasPressed) {
			gameObject.SendMessage("Melee", SendMessageOptions.DontRequireReceiver);
		}
		if (playerManager.player.profile.inputActions.Shoot.WasPressed) {
			gameObject.BroadcastMessage("StartFire", SendMessageOptions.DontRequireReceiver);
		}
		if (playerManager.player.profile.inputActions.Shoot.WasReleased) {
			gameObject.BroadcastMessage("EndFire", SendMessageOptions.DontRequireReceiver);
		}
		if (playerManager.player.profile.inputActions.Taunt.WasPressed) {
			gameObject.SendMessage("Taunt", SendMessageOptions.DontRequireReceiver);
		}
	}
}
