using Godot;
using System;
using System.Collections.Generic;

public class Spawn : State
{

	public Vector2 _startPosition = Vector2.Zero; 

	// Called when the node enters the scene tree for the first time.
	public override async void _Ready()
	{
		SetStateMachine();
		var owner = Owner as Player;
		await ToSignal(owner, "ready");
		_startPosition = owner.Position;
	}

	public override void Enter(IDictionary<string, object> msg = null)
	{
		var owner = Owner as Player;
		owner.IsActive = false;
		//owner._cameraRig.IsActive = false;
		owner.Position = _startPosition;
		owner._skin.Play("spawn");
		owner._skin.Connect("animation_finished", this, nameof(_on_Skin_animation_finished));
	}

	private void _on_Skin_animation_finished(string name)
	{
		_stateMachine.TransitionTo("Move/Idle");
	}

	public override void Exit()
	{
		var owner = Owner as Player;
		owner.IsActive = true;
		owner.hookObj.Visible = true;
		//owner._cameraRig.IsActive = true;
		owner._skin.Disconnect("animation_finished", this, nameof(_on_Skin_animation_finished));
	}
}
