namespace LagetWeb.Models;

/// <summary>Ett tema på laget.no.</summary>
/// <param name="Slug">Filnavn-vennlig id, brukes i kodeblokk og utforsker.</param>
/// <param name="Tittel">Visningsnavn.</param>
/// <param name="Tag">Kort kategori, f.eks. "historie &amp; språk".</param>
/// <param name="Beskrivelse">Én setning om temaet.</param>
/// <param name="Href">Lenke. Relativ (intern rute) eller til statisk undermappe.</param>
/// <param name="Ekstern">
/// True når Href peker på en statisk undermappe utenfor Blazor-ruteren
/// (må lastes med full sidelast), false for interne Blazor-ruter.
/// </param>
/// <param name="Publisert">False = vises som "kommer …", ikke klikkbar.</param>
public record Tema(
    string Slug,
    string Tittel,
    string Tag,
    string Beskrivelse,
    string Href,
    bool Ekstern,
    bool Publisert = true);

/// <summary>
/// Registeret over temaene på siden — én kilde for både kodeblokken på
/// forsiden og utforskeren under _tema. Legg til et nytt tema ved å legge
/// til én linje her.
/// </summary>
public static class Temaer
{
    public static readonly IReadOnlyList<Tema> Alle =
    [
        new Tema(
            Slug: "bantu-kikuyu",
            Tittel: "Bantu & kikuyu",
            Tag: "historie & språk",
            Beskrivelse: "Bantu-ekspansjonen — en av historiens største folkevandringer. "
                       + "Kart, tidslinje, språk, tro og kikuyufolket, som infografikk.",
            Href: "bantu-expansion/",
            Ekstern: true),
    ];
}
