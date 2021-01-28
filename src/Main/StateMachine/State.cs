using Godot;
using System;
using System.Collections.Generic;

public abstract class State : Node
{
	public StateMachine _stateMachine;
	
	public State() { }
	
	public void SetStateMachine()
	{
		_stateMachine = GetStateMachine(this) as StateMachine;
	}
	
	public Node GetStateMachine(Node node)
	{	
		if(node != null && !node.IsInGroup("state_machine"))
		{
			return GetStateMachine(node.GetParent());
		}
		return node;
	}

	public virtual void PhysicsProcess(float delta)
	{
	}

	public virtual void UnhandledInput(InputEvent @event)
	{
	}
	
	public virtual void Enter(IDictionary<string, object> msg = null)
	{
	}
	
	public virtual void Exit()
	{
	}
}
