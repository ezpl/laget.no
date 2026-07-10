# laget.no

Kildekoden for **www.laget.no** — et personlig nettsted med «informasjon og
nyttige ting», ett tema om gangen. Startsiden er en **Blazor WebAssembly**-app
(C#) i kode-editor-stil; hvert tema er sin egen mappe.

Live: <https://www.laget.no> · Repo: <https://github.com/ezpl/laget.no>

## Struktur

```
src/laget-web/            Blazor WASM-app (startsiden = "skallet")
  Pages/                  _hei (Home), _tema, _om-siden, _kontakt
  Components/Snake.razor  snake-spillet (ren C#)
  Components/Konsoll.razor skjult terminal (easter egg) — trykk ` (backtick)
  KonsollBus.cs           bro fra JS-hurtigtast til terminal-komponenten
  Services/LangService.cs språk (nb/en): valg, persistens, nettleser-standard
  LocalizedComponentBase.cs  basisklasse: injiserer Lang + re-render ved bytte
  Layout/MainLayout.razor faner (topp) + språkvelger + statuslinje (bunn)
  Models/Tema.cs          register: temaer + 0z0-apper (lokalisert) — én kilde
  wwwroot/                index.html, css/app.css (Grafitt-tema)
  wwwroot/js/laget.js     språkhjelpere, konsoll-hilsen, backtick, matrix
static/
  bantu-expansion/        tema: Bantu & kikuyu (selvstendig statisk infografikk)
.github/workflows/deploy.yml   bygger + deployer til GitHub Pages ved push til main
```

`static/`-mappene kopieres inn i web-roten ved deploy, så de ligger på
`https://www.laget.no/<mappe>/`. Bantu-infografikken har sitt eget design og
røres ikke av Blazor-appen; se `static/bantu-expansion/README.md`.

## Kjøre lokalt

```bash
dotnet run --project src/laget-web
# åpne http://localhost:5175
```

De statiske tema-mappene serveres ikke av dev-serveren (de bor i `static/`,
utenfor appens `wwwroot/`). For å teste hele nettstedet slik det deployes:

```bash
dotnet publish src/laget-web -c Release -o publish
cp -r static/. publish/wwwroot/
# server publish/wwwroot med en hvilken som helst statisk server
```

## Legge til nytt innhold

**Nytt statisk tema** (som Bantu):
1. Lag `static/<slug>/index.html` (URL-vennlig slug: små bokstaver, bindestrek).
2. Legg til én linje i `src/laget-web/Models/Tema.cs` (`Ekstern: true`).
   Da dukker temaet opp både i kodeblokken på forsiden og under `_tema`.

**Nytt C#-verktøy** (interaktiv side):
1. Lag `src/laget-web/Pages/<Navn>.razor` med `@page "/<rute>"`.
2. Legg til en linje i `Models/Tema.cs` med `Ekstern: false` og `Href: "<rute>"`.

## Språk (i18n)

Siden er tospråklig (norsk/engelsk) uten `.resx`/kulturer — appen beholder
`InvariantGlobalization` for kort lastetid. `LangService` holder valgt språk;
komponenter arver `LocalizedComponentBase` og henter tekst via `Lang.T("nb",
"en")` (inline) eller `Lang.L(ls)` (lokalisert data). Nytt tekstpar = to
argumenter på stedet, ingen egen ressursfil. Valget lagres i `localStorage`
(`laget-lang`); standard utledes fra nettleserspråket. Bytt med `nb`/`en` i
fane-linja.

## Deploy

Push til `main` → GitHub Actions (`.github/workflows/deploy.yml`) bygger appen,
setter sammen web-roten (statiske tema + `.nojekyll` + `CNAME` + `404.html` for
SPA-fallback) og publiserer til GitHub Pages. Ingen manuelle steg.

Appen publiseres med `InvariantGlobalization` (dropper ICU-globaliseringsdata,
~1,5 MB spart) og standard IL-trimming for kortere lastetid.

### Påskeegg 🥚

Trykk `` ` `` (backtick) hvor som helst — eller `>_` i statuslinja — for en
skjult terminal med kommandoer (`help`, `ls`, `open <tema>`, `matrix`, …).
Skrevet i C#; kommandotolkeren ligger i `Components/Konsoll.razor`. Åpne også
DevTools-konsollen for en hilsen.

### Domene (Cloudflare)

DNS for laget.no styres i Cloudflare. `www` peker på GitHub Pages via
`CNAME www → ezpl.github.io` (**DNS only** / grå sky, så GitHub kan utstede
SSL-sertifikat). GitHub Pages sitt egendomene er satt til `www.laget.no` via
`CNAME`-fila i deploy-outputen.
