using System;

namespace IronCrusade.Shared;

public class PersoonlijkeGegevensLogModel
{
    public string Username { get; set; } = null!;
    public DateTime Datum { get; set; }
    public decimal Gewicht { get; set; }
}