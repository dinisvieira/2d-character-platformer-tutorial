using Godot;
using System;
using System.Linq;

public class Skin : Node2D
{

	[Signal]
	public delegate void animation_finished(string name);

	private AnimationPlayer _animationPlayer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_animationPlayer = GetNode("AnimationPlayer") as AnimationPlayer;
		_animationPlayer.Connect("animation_finished", this, nameof(_on_AnimationPlayer_animation_finished));
	}

	public void Play(string name)
	{
		if (_animationPlayer.GetAnimationList().Contains(name))
		{
			_animationPlayer.Stop();
			_animationPlayer.Play(name);
		}
	}

	private void _on_AnimationPlayer_animation_finished(string name)
	{
		EmitSignal(nameof(animation_finished), name);
	}
}
