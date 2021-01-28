using Godot;
using System;
using System.Collections.Generic;

public class Air : State
{
	[Export]
	public NodePath stateMachinePath;
	
	public Air() : base() {}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetStateMachine();
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
