using Godot;
using System;
using System.Collections.Generic;

public class StateMachine : Node
{
	
	[Export]
	protected NodePath initialState;
	
	public StateMachine() : base()
	{
		AddToGroup("state_machine");	
	}
	
	// Called when the node enters the scene tree for the first time.
	public async override void _Ready()
	{
		State = GetNode(initialState) as State;
		await ToSignal(Owner, "ready");
		State.Enter();
	}
	
	private State _state;
	public State State
	{
		get { return _state; }
		set
		{
			_state = value;
			StateName = _state.Name;
		}
	}
	
	private string _stateName;
	public string StateName
	{
		get { return _stateName; }
		private set
		{
			_stateName = value;
		}
	}

	public override void _PhysicsProcess(float delta)
	{
		State.PhysicsProcess(delta);
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		State.UnhandledInput(@event);
	}
	
	public void TransitionTo(string targetStatePath, IDictionary<string, string> msg = null)
	{
		if(!HasNode(targetStatePath))
		{
			return;
		}
		
		var targetState = GetNode(targetStatePath) as State;
		State.Exit();
		State = targetState;
		State.Enter(msg);
	}
}
