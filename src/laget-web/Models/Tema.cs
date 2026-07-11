namespace LagetWeb.Models;

/// <summary>Lokalisert streng — norsk og engelsk variant.</summary>
public record Ls(string Nb, string En);

/// <summary>Ett tema på laget.no.</summary>
/// <param name="Slug">Filnavn-vennlig id, brukes i kodeblokk og terminal.</param>
/// <param name="Tittel">Visningsnavn.</param>
/// <param name="Tag">Kort kategori.</param>
/// <param name="Beskrivelse">Én setning om temaet.</param>
/// <param name="Href">Lenke — relativ (intern rute / statisk mappe) eller ekstern.</param>
/// <param name="Ekstern">
/// True når Href peker utenfor Blazor-ruteren (statisk mappe eller ekstern URL)
/// og må lastes med full sidelast; false for interne Blazor-ruter.
/// </param>
/// <param name="Publisert">False = vises som «kommer …», ikke klikkbar.</param>
public record Tema(
    string Slug,
    Ls Tittel,
    Ls Tag,
    Ls Beskrivelse,
    string Href,
    bool Ekstern,
    bool Publisert = true);

/// <summary>
/// Registeret over temaene — én kilde for kodeblokken på forsiden, terminalen
/// og utforskeren under _tema. Legg til et nytt tema med én linje her.
/// </summary>
public static class Temaer
{
    public static readonly IReadOnlyList<Tema> Alle =
    [
        new Tema(
            Slug: "bantu-kikuyu",
            Tittel: new Ls("Bantu & kikuyu", "Bantu & Kikuyu"),
            Tag: new Ls("historie & språk", "history & language"),
            Beskrivelse: new Ls(
                "Bantu-ekspansjonen — en av historiens største folkevandringer. "
              + "Kart, tidslinje, språk, tro og kikuyufolket, presentert som infografikk.",
                "The Bantu expansion — one of history's great migrations. "
              + "Map, timeline, language, belief and the Kikuyu people, presented as an infographic."),
            Href: "bantu-expansion/",
            Ekstern: true),
        new Tema(
            Slug: "wirute-gikuyu",
            Tittel: new Ls("Wĩrute Gĩkũyũ", "Wĩrute Gĩkũyũ"),
            Tag: new Ls("språk", "language"),
            Beskrivelse: new Ls(
                "Lær kikuyu: kort med spaced repetition, quiz, grammatikk, "
              + "setninger og søkbar ordbok. På norsk, engelsk, tysk og fransk.",
                "Learn Kikuyu: spaced-repetition flashcards, quizzes, grammar, "
              + "sentences and a searchable dictionary. In Norwegian, English, German and French."),
            Href: "https://ezpl.github.io/wirute-gikuyu/",
            Ekstern: true),
    ];
}

/// <summary>En applikasjon fra 0z0 / ZeroZero Software.</summary>
/// <param name="Navn">Appnavn.</param>
/// <param name="Om">Kort hva den gjør.</param>
/// <param name="Href">Direkte lenke (GitHub-repo e.l.), åpnes i ny fane.</param>
public record App(string Navn, Ls Om, string Href);

/// <summary>
/// Registeret over 0z0-appene — brukes av app-seksjonen på forsiden,
/// «Applikasjoner»-kategorien under _tema og <c>apper</c>-kommandoen i
/// terminalen. Legg til en app med én linje her.
/// </summary>
public static class Apper
{
    /// <summary>0z0-«hubben» — samlesiden for alle appene.</summary>
    public const string HubUrl = "https://0z0.xyz";

    public static readonly IReadOnlyList<App> Alle =
    [
        new App("ChargeKeeper",
            new Ls("holder laptop-batteriet friskt — ladegrenser, live måler og smart standby fra systemkurven",
                   "keeps your laptop battery healthy — charge limits, a live gauge and smart standby from the tray"),
            "https://github.com/0z00z0/ChargeKeeper"),
        new App("HyperVManagerTray",
            new Ls("bytter Hyper-V-VM-er til riktig virtuelt nett når verten flytter mellom nettverk",
                   "auto-switches Hyper-V VMs to the right virtual network as the host moves between networks"),
            "https://github.com/0z00z0/HyperVManagerTray"),
        new App("M365Migrator",
            new Ls("flytter e-post, kontakter og kalender ut av Microsoft 365 og inn i personvernvennlige europeiske tjenester",
                   "moves your mail, contacts and calendar out of Microsoft 365 into privacy-friendly European services"),
            "https://0z0.xyz"),
    ];
}
