using System;
using IronCrusade.Shared.Types;

namespace IronCrusade.Shared;

public class WorkoutLogModel
{
    public string Username { get; set; } = null!;
    public CategorieType Categorie { get; set; }
    public DateTime WorkoutStart { get; set; }
    public DateTime? WorkoutEind { get; set; }
    public TimeSpan? Duur => WorkoutEind - WorkoutStart;
}