using Microsoft.JSInterop;

namespace LagetWeb;

/// <summary>
/// Bro mellom den globale JS-hurtigtasten (`) og terminal-komponenten.
/// JS kaller den statiske <see cref="Toggle"/>; den montede <c>Konsoll</c>-
/// komponenten lytter på <see cref="ToggleRequested"/>.
/// </summary>
public static class KonsollBus
{
    public static event Action? ToggleRequested;

    [JSInvokable]
    public static void Toggle() => ToggleRequested?.Invoke();
}
