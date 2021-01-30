using Godot;
using System;
using System.Collections.Generic;

public class Hook : State
{
	[Export]
	protected float hookMaxSpeed = 1500;

	[Export]
	protected float arrivePush = 500; //Small push to project player after it arrives in hook

	private Vector2 _targetGlobalPosition = Vector2.Inf;
	private Vector2 _velocity = Vector2.Zero;
	private float _currentDistanceAux = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetStateMachine();
	}

	public override void PhysicsProcess(float delta)
	{
		var owner = Owner as Player;
		var newVelocity = Steering.ArriveTo(_velocity, owner.GlobalPosition, _targetGlobalPosition, hookMaxSpeed);
		if (newVelocity.Length() > arrivePush)
		{
			newVelocity = newVelocity;
		}
		else
		{
			newVelocity = newVelocity.Normalized() * arrivePush;
		}

		_velocity = owner.MoveAndSlide(newVelocity, Player.FloorNormal);

		var events = GetNode("/root/Events");
		events.EmitSignal("player_moved", owner);//For the camera to update position

		var toTarget = _targetGlobalPosition - owner.GlobalPosition;

		GD.Print("TargetPos:"+_targetGlobalPosition.x + ","+_targetGlobalPosition.y + "|"+"OwnerPos:"+owner.GlobalPosition.x + ","+owner.GlobalPosition.y);

		var distanceToTarget = toTarget.Length();

		GD.Print("_velocity.Length() * delta: "+_velocity.Length() * delta);
		GD.Print("distanceToTarget: "+distanceToTarget);



		if ((distanceToTarget < _velocity.Length() * delta) //check if we are really close to the target
			|| (distanceToTarget > _currentDistanceAux)) //check if we passed the target without being detected
		{
			GD.Print("Go from Hook to Air");
			_velocity = _velocity.Normalized() * arrivePush;
			_stateMachine.TransitionTo("Move/Air", new Dictionary<string, object>() { { "Velocity", _velocity } });
			return;
		}

		if (owner.IsOnFloor())
		{
			GD.Print("Go from Hook to Run");
			_stateMachine.TransitionTo("Move/Run");
			return;
		}

		_currentDistanceAux = distanceToTarget;
	}

	public override void Enter(IDictionary<string, object> msg = null)
	{
		if (msg != null && msg.ContainsKey("TargetGlobalPosition") && msg["TargetGlobalPosition"] is Vector2 pos
						&& msg.ContainsKey("Velocity") && msg["Velocity"] is Vector2 vel)
		{
			var owner = Owner as Player;
			_currentDistanceAux = float.PositiveInfinity;
			_targetGlobalPosition = pos;
			_velocity = vel;
		}
	}
	
	public override void Exit()
	{
		_targetGlobalPosition = Vector2.Inf;
		_velocity = Vector2.Zero;
		_currentDistanceAux = 0;
	}
}
