using Godot;
using System;

public class Target : Area2D
{

	private AnimationPlayer _animPlayer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_animPlayer = GetNode("AnimationPlayer") as AnimationPlayer;
		Visible = false;
	}

	public override void _PhysicsProcess(float delta)
	{
		if (Input.IsActionPressed("click"))
		{
			GlobalPosition = GetGlobalMousePosition();
		}
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed("click"))
		{
			_animPlayer.Play("fade_in");
		}
	}

	private void _on_Target_body_entered(object body)
	{
		_animPlayer.Play("fade_out");
	}
}
