using Godot;
using System;

public class Aim : State
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetStateMachine();
	}

	public override void UnhandledInput(InputEvent @event)
	{
		var owner = Owner as HookObj;
		if (@event.IsActionPressed("hook") && owner.CanHook())
		{
			_stateMachine.TransitionTo("Aim/Fire");
		}
	}

	public override void PhysicsProcess(float delta)
	{
		var owner = Owner as HookObj;
		var cast = owner.GetAimDirection() * owner.rayCast.CastTo.Length();
		var angle = cast.Angle();
		owner.rayCast.CastTo = cast;
		owner.arrow.Rotation = angle;
		owner.snapDetector.Rotation = angle;
		owner.rayCast.ForceRaycastUpdate(); //TODO: Delete this?
	}
}
