# Wĩrute Gĩkũyũ — handoff til Claude Code

Frontend-kjernen er ferdig som én selvstendig HTML-fil (`Wirute-Gikuyu.html`). Dette dokumentet beskriver
neste fase: gjøre den til en flerbruker-webapp for familien, hostet på Proxmox.

## Nåværende tilstand (levert fra Cowork)
- **`Wirute-Gikuyu.html`** — komplett app: kort (Leitner-SRS), quiz, grammatikk, eksempelsetninger, ordbok. Ingen backend; fremgang i `localStorage`.
- **Innhold kun fra pålitelige kilder.** Easy Way-bøkene er forkastet (bekreftede feil, f.eks. `thahabu`=gull ikke penger).
  - Vokabular: den native ordboken (~2178 oppslag), aksenter normalisert til tilde (ĩ/ũ).
  - Grammatikk: Sketch Grammar (Rice University) + Gecaga & Kirkaldy-Willis (1953).
  - Setninger: glosserte eksempler fra Sketch Grammar.
- Datagenerering: `build_data.py` (i outputs) → `app_data.json`, injisert i `template.html`.

## Foreslått arkitektur (valgt: ASP.NET Core + SQLite)
```
/src
  /Api            ASP.NET Core minimal API (.NET 8)
    Program.cs        endepunkter: /api/auth, /api/progress
    /Data             EF Core DbContext + SQLite
    /Models           User, CardState, QuizResult
  /wwwroot          statisk frontend (dagens HTML splittet i html/css/js)
  /data             gikuyu.db (SQLite, volum-montert), dictionary.json, decks.json, sentences.json, grammar.json
Dockerfile
docker-compose.yml   (én container + volum for /data)
```
- **Auth:** enkel for familiebruk. Alternativer: (a) ASP.NET Identity med e-post/passord, (b) magic-link, (c) reverse-proxy auth (Authelia/Authentik) foran containeren — minst kode. Anbefaling for få brukere: (c) hvis du alt kjører en proxy, ellers (a).
- **Fremgang:** flytt `localStorage`-modellen (`{cards:{deck::i:{box,due}}}`) til `CardState`-tabell pr. bruker. API: `GET/PUT /api/progress`. Behold localStorage som offline-cache.
- **Deploy:** container på Proxmox, HTTPS via din eksisterende proxy.

## i18n (norsk/engelsk-bryter)
Alle UI-strenger ligger allerede samlet i objektet **`T`** øverst i `<script>`. Migrering:
1. Flytt `T`, `GRAMMAR` og gruppenavn til `locales/no.json` og `locales/en.json`.
2. Innholdsdata (`decks`, `sentences`, `dict`) har Gĩkũyũ + engelsk gloss. Legg til norsk gloss-felt der du vil ha norsk (start med `decks` og `grammar`; ordboken kan forbli engelsk).
3. Enkel `t(key)`-funksjon + `<select>` for språk, lagre valg i localStorage.

## Gjenstående datajobb (bedre i Claude Code — ingen 45s-grense)
- **Full OCR av Gecaga-grammatikken** (83 sider, skannet). Kun de første ~12 sidene ble OCR-et i Cowork (hilsener, dager, «to be»/«to have»). Kjør `pdftoppm -r 300` + `tesseract` lokalt på alle sider, rens, og utvid grammatikk-/paradigmedelen.
- Vurder OCR av Grade 1-boka for barnevennlige øvelser.
- Ethnografien (*Southern Kikuyu before 1903*) er ikke språkdata — hold utenfor.

## Kvalitetsforbehold
- Toner er ikke markert (kildene markerer dem inkonsekvent).
- Ukedagsnavn varierer regionalt.
- Enkelte ordbok-glosser har OCR-artefakter; ordboken bør korrekturleses mot en morsmålskilde over tid.
