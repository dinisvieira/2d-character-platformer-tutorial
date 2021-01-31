using Godot;
using System;

public class HookObj : Position2D
{

	public RayCast2D rayCast;
	public Arrow arrow;
	public SnapDetector snapDetector;
	public Timer cooldown;

	public bool isActive = true;

	[Signal]
	public delegate void hooked_onto_target(Vector2 hookPosition);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		rayCast = GetNode("RayCast2D") as RayCast2D;
		arrow = GetNode("Arrow") as Arrow;
		snapDetector = GetNode("SnapDetector") as SnapDetector;
		cooldown = GetNode("Cooldown") as Timer;
	}

	public bool CanHook()
	{
		return isActive && snapDetector.HasTarget() && cooldown.IsStopped();
	}

	public Vector2 GetAimDirection()
	{
		var direction = Vector2.Zero;
		direction = GetLocalMousePosition().Normalized();
		return direction;
	}
}
