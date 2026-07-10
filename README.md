# laget.no — nettstedet

Denne mappen speiler web-roten på www.laget.no. Alt er statiske filer —
ingen byggetrinn, ingen server-kode. Ved publisering kopieres hele mappen
(minus README-filer) til web-roten.

## Struktur

```
index.html          ← startsiden (www.laget.no)
bantu-expansion/    ← tema: Bantu & kikuyu — infografikk om Bantu-ekspansjonen
  index.html        ← den publiserte, selvstendige siden
  README.md         ← hvordan infografikken redigeres og bygges
```

## Legge til et nytt tema

1. Lag en ny undermappe med et URL-vennlig navn (små bokstaver, bindestrek,
   ingen mellomrom eller æøå), f.eks. `fjellturer/`, med en `index.html` i.
2. Åpne `index.html` i roten og kopier en `<a class="card">`-blokk i
   tema-rutenettet. Endre `href`, kicker (kategorilinjen), tittel og tekst.
   Rutenettet tilpasser seg automatisk.

Startsidens design (farger og fonter) er hentet fra Bantu-infografikken:
pergament-bakgrunn, terrakotta/oker-aksenter, Spectral (brødtekst/titler)
og Barlow Condensed (etiketter). Gjenbruk gjerne disse i nye tema-sider
for et helhetlig uttrykk — variablene ligger i `:root` i `index.html`.
