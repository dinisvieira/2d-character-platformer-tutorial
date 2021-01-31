using Godot;
using System;
using System.Collections.Generic;

public class Die : State
{
	// Called when the node enters the scene tree for the first time.
	public override async void _Ready()
	{
		SetStateMachine();
		var owner = Owner as Player;
		await ToSignal(owner, "ready");
	}

	public override void Enter(IDictionary<string, object> msg = null)
	{
		var owner = Owner as Player;
		owner.IsActive = false;
		//owner._cameraRig.IsActive = false;
		owner._skin.Play("die");
		owner._skin.Connect("animation_finished", this, nameof(_on_Skin_animation_finished));
	}

	private void _on_Skin_animation_finished(string name)
	{
		_stateMachine.TransitionTo("Spawn");
	}

	public override void Exit()
	{
		var owner = Owner as Player;
		owner.IsActive = false;
		owner.hookObj.Visible = false;
		//owner._cameraRig.IsActive = false;
		owner._skin.Disconnect("animation_finished", this, nameof(_on_Skin_animation_finished));
	}
}
