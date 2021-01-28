using Godot;
using System;
using System.Collections.Generic;

public class Idle : State
{
	[Export]
	public NodePath stateMachinePath;

	private Move _parentMove;
	
	public Idle() : base()  { }
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetStateMachine();
		_parentMove = GetParent() as Move;
	}

	public override void PhysicsProcess(float delta)
	{
		var player = Owner as Player;
		if(player.IsOnFloor() && Move.GetMoveDirection().x != 0.0)
		{
			_stateMachine.TransitionTo("Move/Run");
		}
		else if(!player.IsOnFloor())
		{
			_stateMachine.TransitionTo("Move/Air");
		}
	}

	public override void UnhandledInput(InputEvent @event)
	{
		_parentMove.UnhandledInput(@event);
	}
	
	public override void Enter(IDictionary<string, object> msg = null)
	{
		_parentMove.Enter(msg);
		
		_parentMove._maxSpeed = _parentMove.maxSpeedDefault;
		_parentMove._velocity = Vector2.Zero;
	}
	
	public override void Exit()
	{
		_parentMove.Exit();
	}
}
