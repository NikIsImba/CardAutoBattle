using Godot;

public partial class Card : ColorRect
{
	[Export]
	public int Cost { get; set; } = 4;

	[Export]
	public int Power { get; set; } = 4;

	public override void _Ready() { }

	public override void _Process(double delta) { }
}
