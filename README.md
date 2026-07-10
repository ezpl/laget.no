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
  Layout/MainLayout.razor faner (topp) + statuslinje (bunn)
  Models/Tema.cs          registeret over temaene — én kilde
  wwwroot/                index.html, css/app.css (Grafitt-tema)
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

## Deploy

Push til `main` → GitHub Actions (`.github/workflows/deploy.yml`) bygger appen,
setter sammen web-roten (statiske tema + `.nojekyll` + `CNAME` + `404.html` for
SPA-fallback) og publiserer til GitHub Pages. Ingen manuelle steg.

### Domene (Cloudflare)

DNS for laget.no styres i Cloudflare. `www` peker på GitHub Pages via
`CNAME www → ezpl.github.io` (**DNS only** / grå sky, så GitHub kan utstede
SSL-sertifikat). GitHub Pages sitt egendomene er satt til `www.laget.no` via
`CNAME`-fila i deploy-outputen.
