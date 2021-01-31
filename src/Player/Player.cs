using Godot;
using System;

public class Player : KinematicBody2D
{
	
	public StateMachine _stateMachine;
	private CollisionShape2D _collider;
	public Skin _skin;
	public HookObj hookObj;
	public static Vector2 FloorNormal = Vector2.Up;
	//public Position2D _cameraRig;
	
	public bool _isActive;
	public bool IsActive
	{
		get{
			return _isActive;
		}
		set{
			_isActive = value;
			
			if(_collider != null) { _collider.Disabled = !value; }
			hookObj.isActive = value;
		}
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_stateMachine = GetNode("StateMachine") as StateMachine;
		_collider = GetNode("CollisionShape2D") as CollisionShape2D;
		//_cameraRig = GetNode("CameraRig") as CameraRig;
		_skin = GetNode("Skin") as Skin;
		hookObj = GetNode("Hook") as HookObj;
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
