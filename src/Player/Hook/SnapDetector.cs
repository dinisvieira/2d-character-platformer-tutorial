using Godot;
using System;

public class SnapDetector : Area2D
{

	private Position2D _hookingHint;
	private RayCast2D _rayCast;

	private HookTarget _hookTarget;
	public HookTarget HookTarget
	{
		get
		{
			return _hookTarget;
		}
		set
		{
			_hookTarget = value;
			_hookingHint.Visible = HasTarget();
			if (_hookTarget != null)
			{
				_hookingHint.GlobalPosition = _hookTarget.GlobalPosition;
			}
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_hookingHint = GetNode("HookingHint") as Position2D;
		_rayCast = GetNode("RayCast2D") as RayCast2D;
		_rayCast.SetAsToplevel(true);
	}

	public override void _PhysicsProcess(float delta)
	{
		HookTarget = FindBestTarget();
	}

	public HookTarget FindBestTarget()
	{
		HookTarget closestTarget = null;
		var targets = GetOverlappingAreas();
		foreach (var area in targets)
		{
			if (area is HookTarget target && target.IsActive)
			{
				_rayCast.GlobalPosition = GlobalPosition;
				_rayCast.CastTo = target.GlobalPosition - _rayCast.GlobalPosition;
				if (!_rayCast.IsColliding())
				{
					closestTarget = target;
				}
			}
		}
		return closestTarget;
	}

	public bool HasTarget()
	{
		return HookTarget != null;
	}
}
