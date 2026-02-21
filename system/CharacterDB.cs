using Godot;
using System;
using System.Collections.Generic;

public partial class CharacterDB : Node
{
	private readonly Dictionary<string, CharacterDef> _byId = new();
	public static CharacterDB Instance { get; private set; }

	[Export] public string CharactersDir = "res://data/characters";

	public override void _EnterTree()
	{
		Instance = this;
	}
	public override void _Ready()
	{
		LoadAllCharacters();
	}

	private void LoadAllCharacters()
	{
		_byId.Clear();

		var dir = DirAccess.Open(CharactersDir);
		if (dir == null)
		{
			GD.PushError($"CharacterDB: cannot open dir: {CharactersDir}");
			return;
		}

		dir.ListDirBegin();
		while (true)
		{
			var file = dir.GetNext();
			if (string.IsNullOrEmpty(file)) break;
			if (dir.CurrentIsDir()) continue;
			if (!file.EndsWith(".tres") && !file.EndsWith(".res")) continue;

			var path = $"{CharactersDir}/{file}";
			var def = ResourceLoader.Load<CharacterDef>(path);
			if (def == null || string.IsNullOrWhiteSpace(def.Id))
			{
				GD.PushWarning($"CharacterDB: invalid character file: {path}");
				continue;
			}

			_byId[def.Id] = def;
		}
		dir.ListDirEnd();
	}

	public CharacterDef Get(string id)
	{
		if (string.IsNullOrWhiteSpace(id))
			throw new ArgumentException("Character id is empty");

		if (_byId.TryGetValue(id, out var def))
			return def;

		GD.PushWarning($"CharacterDB: missing character id '{id}'");
		return null;
	}
}
