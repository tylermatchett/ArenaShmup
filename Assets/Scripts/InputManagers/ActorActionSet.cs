using UnityEngine;
using System.Collections;
using InControl;

public class ActorActionSet : PlayerActionSet {
	public PlayerAction Boost;
	public PlayerAction Reload;
	public PlayerAction Taunt;

	public PlayerAction Shoot;
	public PlayerAction Ability;
	public PlayerAction Melee;

	public PlayerAction Move_Up;
	public PlayerAction Move_Down;
	public PlayerAction Move_Left;
	public PlayerAction Move_Right;
	public PlayerTwoAxisAction Move;

	public PlayerAction Aim_Up;
	public PlayerAction Aim_Down;
	public PlayerAction Aim_Left;
	public PlayerAction Aim_Right;
	public PlayerTwoAxisAction Aim;

	public ActorActionSet() {
		Boost = CreatePlayerAction("Boost");
		Reload = CreatePlayerAction("Reload");
		Taunt = CreatePlayerAction("Taunt");

		Shoot = CreatePlayerAction("Shoot");
		Ability = CreatePlayerAction("Ability");
		Melee = CreatePlayerAction("Melee");

		Move_Up = CreatePlayerAction("Move Up");
		Move_Down = CreatePlayerAction("Move Down");
		Move_Left = CreatePlayerAction("Move Left");
		Move_Right = CreatePlayerAction("Move Right");

		Move = CreateTwoAxisPlayerAction(Move_Left, Move_Right, Move_Down, Move_Up);
		
		Aim_Up = CreatePlayerAction("Aim Up");
		Aim_Down = CreatePlayerAction("Aim Down");
		Aim_Left = CreatePlayerAction("Aim Left");
		Aim_Right = CreatePlayerAction("Aim Right");

		Aim = CreateTwoAxisPlayerAction(Aim_Left, Aim_Right, Aim_Down, Aim_Up);
	}
}
