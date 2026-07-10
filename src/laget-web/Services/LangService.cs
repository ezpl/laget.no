using Microsoft.JSInterop;
using LagetWeb.Models;

namespace LagetWeb.Services;

/// <summary>
/// Holder valgt språk (nb/en) for hele appen. Én kilde: komponentene henter
/// tekst via <see cref="T"/> (inline) eller <see cref="L"/> (data), og
/// re-rendrer når <see cref="OnChange"/> fyres. Valget lagres i localStorage;
/// standard utledes fra nettleserspråket. Bevisst uten .NET-kulturer, så
/// appen kan beholde InvariantGlobalization (kort lastetid).
/// </summary>
public class LangService
{
    private readonly IJSRuntime _js;

    public LangService(IJSRuntime js) => _js = js;

    /// <summary>Gjeldende språk: "nb" eller "en".</summary>
    public string Cur { get; private set; } = "nb";

    public bool IsEn => Cur == "en";

    public event Action? OnChange;

    /// <summary>Velg mellom to inline-varianter etter gjeldende språk.</summary>
    public string T(string nb, string en) => Cur == "en" ? en : nb;

    /// <summary>Velg riktig variant av en lokalisert data-streng.</summary>
    public string L(Ls s) => Cur == "en" ? s.En : s.Nb;

    /// <summary>Les lagret valg, ellers utled fra nettleserspråk. Kalles ved oppstart.</summary>
    public async Task InitAsync()
    {
        try
        {
            var saved = await _js.InvokeAsync<string?>("lagetLang.get");
            if (saved is "nb" or "en") { Cur = saved; return; }

            var browser = (await _js.InvokeAsync<string?>("lagetLang.browser") ?? "").ToLowerInvariant();
            Cur = browser.StartsWith("nb") || browser.StartsWith("no") || browser.StartsWith("nn") ? "nb" : "en";
        }
        catch
        {
            Cur = "nb"; // trygg standard om JS ikke er tilgjengelig
        }
    }

    /// <summary>Bytt språk, lagre valget og varsle komponentene.</summary>
    public async Task SetAsync(string lang)
    {
        if (lang is not ("nb" or "en") || lang == Cur) return;
        Cur = lang;
        await _js.InvokeVoidAsync("lagetLang.set", lang);
        OnChange?.Invoke();
    }
}
