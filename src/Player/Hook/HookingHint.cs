using Godot;
using System;

[Tool]
public class HookingHint : Position2D
{
	[Export]
	public Color _color;
	
	[Export]
	public float _radius = 12;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetAsToplevel(true);
		Update();
	}

	public override void _Draw()
	{
		DrawCircle(Vector2.Zero, _radius, _color);
	}
}
