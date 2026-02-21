using Godot;
using System;

public enum DialogueSide
{
	Left,
	Right
}

[GlobalClass]
public partial class CharacterDef : Resource
{
	[Export] public string Id { get; set; } = "";
	[Export] public string DisplayName { get; set; } = "";
	[Export] public Texture2D Portrait { get; set; }
	[Export] public DialogueSide Side { get; set; } = DialogueSide.Left;
}
