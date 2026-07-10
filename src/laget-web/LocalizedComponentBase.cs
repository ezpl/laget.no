using Microsoft.AspNetCore.Components;
using LagetWeb.Services;

namespace LagetWeb;

/// <summary>
/// Basisklasse for komponenter som viser lokalisert tekst. Injiserer
/// <see cref="LangService"/> som <c>Lang</c> og re-rendrer automatisk når
/// språket byttes. Komponenter som overstyrer <see cref="OnInitialized"/>
/// eller <see cref="Dispose"/> må kalle <c>base</c>.
/// </summary>
public abstract class LocalizedComponentBase : ComponentBase, IDisposable
{
    [Inject] protected LangService Lang { get; set; } = default!;

    protected override void OnInitialized() => Lang.OnChange += Refresh;

    private void Refresh() => InvokeAsync(StateHasChanged);

    public virtual void Dispose() => Lang.OnChange -= Refresh;
}
