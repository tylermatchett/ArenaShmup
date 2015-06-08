public class Profile {

	public string Name;
	//public PlayerStats stats;
	public ActorActionSet inputActions;

	public Profile(string profileName) {
		Name = profileName;
		// stats = new PlayerStats();
		inputActions = new ActorActionSet();
		LoadDefaultControlBindings();
	}

	void LoadDefaultControlBindings() {
		inputActions.Shoot.AddDefaultBinding(InControl.InputControlType.RightTrigger);

		inputActions.Boost.AddDefaultBinding(InControl.InputControlType.Action1);

		inputActions.Taunt.AddDefaultBinding(InControl.InputControlType.DPadUp);

		//inputActions.Melee.AddDefaultBinding(InControl.InputControlType.Action2);
		//inputActions.Melee.AddDefaultBinding(InControl.InputControlType.RightBumper);

		inputActions.Ability.AddDefaultBinding(InControl.InputControlType.Action3);
		inputActions.Ability.AddDefaultBinding(InControl.InputControlType.LeftTrigger);

		inputActions.Reload.AddDefaultBinding(InControl.InputControlType.Action4);
        inputActions.Reload.AddDefaultBinding(InControl.InputControlType.RightBumper);
		
		inputActions.Move_Up.AddDefaultBinding(InControl.InputControlType.LeftStickUp);
		inputActions.Move_Down.AddDefaultBinding(InControl.InputControlType.LeftStickDown);
		inputActions.Move_Left.AddDefaultBinding(InControl.InputControlType.LeftStickLeft);
		inputActions.Move_Right.AddDefaultBinding(InControl.InputControlType.LeftStickRight);

		inputActions.Aim_Up.AddDefaultBinding(InControl.InputControlType.RightStickUp);
		inputActions.Aim_Down.AddDefaultBinding(InControl.InputControlType.RightStickDown);
		inputActions.Aim_Left.AddDefaultBinding(InControl.InputControlType.RightStickLeft);
		inputActions.Aim_Right.AddDefaultBinding(InControl.InputControlType.RightStickRight);

	}

	public bool LoadProfile(string profileName) {
		// Load a profile from a name stored in a list in the gamemanager
		// Load control bindings
		return false;
	}

	bool SaveProfile() {
		// Save or overright a profile with this name
		return false;
	}

	/*
	 * Saving and Loading the control bindings example and link
	 *  http://www.gallantgames.com/pages/incontrol-persisting-bindings
	 * 
		void SaveBindings()
		{
		    saveData = inputActions.Save();
		    PlayerPrefs.SetString( "Bindings", saveData );
		}


		void LoadBindings()
		{
		    if (PlayerPrefs.HasKey( "Bindings" ))
		    {
		        saveData = PlayerPrefs.GetString( "Bindings" );
		        inputActions.Load( saveData );
		    }
		}
	 */
}
