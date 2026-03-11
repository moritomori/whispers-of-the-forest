using Godot;

namespace WhispersOfTheForest.Data;

/// <summary>
/// Defines dialogue portrait placement for a character.
/// </summary>
public enum DialogueSide
{
	Left,
	Right
}

/// <summary>
/// Stores dialogue-related presentation data for a character.
/// </summary>
[GlobalClass]
public partial class CharacterDef : Resource
{
	[Export] public string Id { get; set; } = string.Empty;
	[Export] public string DisplayName { get; set; } = string.Empty;
	[Export] public Texture2D? Portrait { get; set; }
	[Export] public DialogueSide Side { get; set; } = DialogueSide.Left;
}