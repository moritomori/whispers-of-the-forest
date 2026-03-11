using Godot;
using System.Collections.Generic;

namespace WhispersOfTheForest.Data;

/// <summary>
/// Loads and stores character definitions by their identifier.
/// </summary>
public partial class CharacterDB : Node
{
	private readonly Dictionary<string, CharacterDef> _byId = new();

	// TODO: Replace singleton access with an injected or exported dependency.
	public static CharacterDB? Instance { get; private set; }

	[Export] private string _charactersDir = "res://data/characters";
	public string CharactersDir => _charactersDir;

	public override void _EnterTree()
	{
		if (Instance is not null && Instance != this)
		{
			GD.PushError("[CharacterDB] Another instance already exists.");
			return;
		}

		Instance = this;
	}

	public override void _ExitTree()
	{
		if (Instance == this)
		{
			Instance = null;
		}
	}

	public override void _Ready()
	{
		LoadAllCharacters();
	}

	/// <summary>
	/// Loads all character resources from the configured directory.
	/// </summary>
	private void LoadAllCharacters()
	{
		_byId.Clear();

		DirAccess? dir = DirAccess.Open(_charactersDir);
		if (dir is null)
		{
			GD.PushError($"[CharacterDB] Cannot open directory: {_charactersDir}");
			return;
		}

		dir.ListDirBegin();

		try
		{
			string file = dir.GetNext();

			while (!string.IsNullOrEmpty(file))
			{
				if (!dir.CurrentIsDir() &&
					(file.EndsWith(".tres") || file.EndsWith(".res")))
				{
					string path = $"{_charactersDir}/{file}";
					CharacterDef? def = ResourceLoader.Load<CharacterDef>(path);

					if (def is null || string.IsNullOrWhiteSpace(def.Id))
					{
						GD.PushWarning($"[CharacterDB] Invalid character file: {path}");
					}
					else
					{
						_byId[def.Id] = def;
					}
				}

				file = dir.GetNext();
			}
		}
		finally
		{
			dir.ListDirEnd();
		}
	}

	/// <summary>
	/// Returns a character definition by its identifier, or null if it is missing.
	/// </summary>
	public CharacterDef? Get(string id)
	{
		if (string.IsNullOrWhiteSpace(id))
		{
			GD.PushWarning("[CharacterDB] Character id is empty.");
			return null;
		}

		if (_byId.TryGetValue(id, out CharacterDef? def))
		{
			return def;
		}

		GD.PushWarning($"[CharacterDB] Missing character id '{id}'.");
		return null;
	}
}