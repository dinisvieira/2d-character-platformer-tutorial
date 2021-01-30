using Godot;
using System;

public class Arrow : Node2D
{

	private Sprite _head;
	private Line2D _tail;
	private Tween _tween;
	private float _startLength;

	private float _length = 40;
	public float Length
	{
		get
		{
			return _length;
		}
		set
		{
			_length = value;

			//update the head position and the tail length
			var lastPointInArray = _tail.Points[_tail.Points.Length - 1];
			lastPointInArray.x = _length;
			_tail.SetPointPosition(_tail.GetPointCount() - 1, lastPointInArray);
			_head.Position = new Vector2(lastPointInArray.x + _tail.Position.x, _head.Position.y);
		}
	}

	private Vector2 _hookPosition = Vector2.Zero;
	public Vector2 HookPosition
	{
		get
		{
			return _hookPosition;
		}
		set
		{
			_hookPosition = value;
			var distanceToTarget = HookPosition - GlobalPosition;
			Length = distanceToTarget.Length();
			Rotation = distanceToTarget.Angle();
			_tween.InterpolateProperty(this, "Length", _length, _startLength, 0.25f, Tween.TransitionType.Quad, Tween.EaseType.Out);
			_tween.Start();
		}
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		//if (@event.IsActionPressed("jump")) //Just for testing the hook
		//{
		//	HookPosition = GetGlobalMousePosition();
		//}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_head = GetNode("Head") as Sprite;
		_tail = GetNode("Tail") as Line2D;
		_tween = GetNode("Tween") as Tween;
		_startLength = _head.Position.x;
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
