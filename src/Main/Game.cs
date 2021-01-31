using Godot;
using System;

public class Game : Node
{

	private Player _player;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_player = GetNode("Player") as Player;
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed("debug_restart"))
		{
			GetTree().ReloadCurrentScene();
		}

		if (@event.IsActionPressed("debug_die"))
		{
			_player._stateMachine.TransitionTo("Die");
		}
	}
}
