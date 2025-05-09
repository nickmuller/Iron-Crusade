﻿using IronCrusade.Client.Types;

namespace IronCrusade.Client.Models;

public readonly record struct OefeningModel()
{
    public required string Naam { get; init; }
    public required int AantalSets { get; init; }
    public required int AantalHerhalingen { get; init; }
    public ICollection<string> Tips { get; init; } = [];
    public TimeSpan DuurSet { get; init; } = TimeSpan.FromMinutes(1);
    public TimeSpan DuurPauze { get; init; } = TimeSpan.FromSeconds(40);
    public string? AfbeeldingUrl { get; init; }
    public string? VideoUrl { get; init; }
    public InitieelTonen InitieelTonen { get; init; } = InitieelTonen.Afbeelding;
}