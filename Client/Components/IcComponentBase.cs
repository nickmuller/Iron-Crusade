using Microsoft.AspNetCore.Components;

namespace IronCrusade.Client.Components;

public abstract class IcComponentBase : ComponentBase
{
    [Parameter]
    public string? Id { get; set; }

    [Parameter]
    public string? Class { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object>? AdditionalAttributes { get; set; }
}