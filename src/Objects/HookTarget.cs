using Godot;
using System;

public class HookTarget : Area2D
{
	private Timer _timer;

	[Export]
	public bool isOneShot = false;

	public static Color ColorActive = new Color(0.9375f, 0.730906f, 0.025635f);
	public static Color ColorInactive = new Color(0.515625f, 0.484941f, 0.4552f);

	[Signal]
	public delegate void hooked_onto_from(Vector2 hookPosition);

	private bool _isActive = true;
	public bool IsActive
	{
		get
		{
			return _isActive;
		}
		set
		{
			_isActive = value;
			Color = _isActive ? ColorActive : ColorInactive;

			if (!_isActive && !isOneShot)
			{
				_timer.Start();
			}
		}
	}

	private Color _color = ColorActive;
	public Color Color
	{
		get
		{
			return _color;
		}
		set
		{
			_color = value;
			Update();
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_timer = GetNode("Timer") as Timer;
		_timer.Connect("timeout", this, nameof(_on_Timer_timeout));
	}

	public override void _Draw()
	{
		DrawCircle(Vector2.Zero, 12.0f, Color);
	}
	
	private void _on_Timer_timeout()
	{
		IsActive = true;
	}

	public void HookedFrom(Vector2 hookPosition)
	{
		IsActive = false;
		EmitSignal(nameof(hooked_onto_from), hookPosition);
	}
}
