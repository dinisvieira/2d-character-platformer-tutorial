using Godot;
using System;
using System.Collections.Generic;

public class Fire : State
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetStateMachine();
	}

	public override void Enter(IDictionary<string, object> msg = null)
	{
		var owner = Owner as HookObj;
		owner.cooldown.Connect("timeout", this, nameof(_on_Cooldown_timeout));
		owner.cooldown.Start();

		var target = owner.snapDetector.HookTarget;
		owner.arrow.HookPosition = target.GlobalPosition;
		target.HookedFrom(owner.GlobalPosition);

		owner.EmitSignal("hooked_onto_target", target.GlobalPosition);
	}

	public override void Exit()
	{
		var owner = Owner as HookObj;
		owner.cooldown.Disconnect("timeout", this, nameof(_on_Cooldown_timeout));
	}

	private void _on_Cooldown_timeout()
	{
		_stateMachine.TransitionTo("Aim");
	}
}
