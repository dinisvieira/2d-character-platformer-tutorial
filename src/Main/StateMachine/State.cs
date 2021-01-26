using Godot;
using System;
using System.Collections.Generic;

public abstract class State : Node
{
	private StateMachine _stateMachine;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_stateMachine = GetStateMachine(this) as StateMachine;
	}
	
	protected Node GetStateMachine(Node node)
	{
		if(node != null && node.IsInGroup("state_machine"))
		{
			return GetStateMachine(node.GetParent());
		}
		return node;
	}

	public void PhysicsProcess(float delta)
	{
		
	}

	public void UnhandledInput(InputEvent @event)
	{
		
		/*
		if (@event is InputEventMouseMotion eventMouse)
		{
			
		}
		*/
	}
	
	public void Enter(IDictionary<string, string> msg = null)
	{
		
	}
	
	public void Exit()
	{
		
	}
}
