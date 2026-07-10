# The Bantu Expansion — infographic

A single-page, six-panel infographic about the Bantu migration, built as a
fixed 1920×1080 landscape design (six stacked panels / "figures"). It is a
**static site**: no server code, no database, no build server required to run —
just serve one HTML file.

---

## What's in this repo

```
index.html                      ← THE PUBLISHED SITE. Self-contained, works offline.
source/
  Bantu Expansion.dc.html        ← EDIT SOURCE OF TRUTH (the real, readable design)
  support.js                     ← runtime the source needs while editing
  checks/overlap-scan.js         ← QA helper that finds text/label overlaps
assets/
  africa-map.svg                 ← the bare Africa outline, exported separately
```

### The two HTML files — important

| File | Role | Edit it by hand? |
|------|------|------------------|
| `index.html` | The **built, self-contained** page you deploy. React, fonts, runtime and all six panels are inlined into this one ~1 MB file, so it runs on any web server with no internet needed. | Not comfortably — the markup is stored **escaped inside a `<script type="__bundler/template">` block**. Fine for a one-word typo fix; painful for real edits. |
| `source/Bantu Expansion.dc.html` | The **human-readable source**. Plain HTML with inline styles. This is where design/content changes should be made. | **Yes — this is the file to edit.** |

> ⚠️ `source/Bantu Expansion.dc.html` will **not render on a plain web server on its own.**
> It relies on a React runtime that is provided by the design tool it was created in.
> `index.html` is the version that has everything baked in — that is the one you host.

---

## Deploying to your own web server

`index.html` is completely self-contained. Any of these work:

**Plain web server (Apache / nginx / any static host)**
1. Copy `index.html` to your web root (e.g. `/var/www/bantu/index.html`).
2. Point a domain or path at it. Done — open it in a browser.

**GitHub Pages**
1. Push this repo to GitHub.
2. Repo → *Settings → Pages* → deploy from the `main` branch, root folder.
3. Your site appears at `https://<user>.github.io/<repo>/`.
   (GitHub Pages serves `index.html` from the root automatically.)

**Netlify / Cloudflare Pages / Vercel**
- Connect the repo; set build command = *none*, publish directory = repo root.

No build step is needed to deploy — `index.html` is already the finished artifact.

---

## Making future changes

The design was authored in an HTML-based design tool that owns the "compile"
step (inlining React + fonts into `index.html`). Because of that, there are two
sensible workflows:

### A. Design/content changes — do them on the source, then rebuild (recommended)
1. Edit `source/Bantu Expansion.dc.html` — it's ordinary HTML with inline
   styles. Each panel is a `<section … data-screen-label="…">`-style block; text
   lives directly in the markup.
2. Re-generate `index.html` from the updated source. The reliable way to
   rebuild is back in the original design workspace (it re-inlines React, the
   Google Fonts, and `support.js` into a fresh self-contained `index.html`).
   Commit the new `index.html`.
3. **Run the overlap check** before publishing (see below) — the layout is a
   fixed 1920×1080 canvas, so added text can collide with panels, the timeline,
   or map labels. This is the single most common regression.

### B. Tiny text fixes — edit index.html directly
For a quick typo, you can search the escaped template inside `index.html`'s
`<script type="__bundler/template">` block and fix it in place. Keep it to
small string edits; anything structural belongs in the source (workflow A).

### The overlap check (QA)
`source/checks/overlap-scan.js` scans every panel for four problems: text
overflowing a panel edge, text colliding with the timeline strip, a content
column taller than its cell, and overlapping SVG map labels. Paste the function
into the browser console on the rendered page and call `runOverlapCheck()` — it
returns `[]` when clean, or a list of the offending elements. Always run it
after editing copy, because this canvas has no auto-reflow.

---

## Using Claude Code for this repo

Claude Code is a great fit for the **repository, hosting and deployment** side:
- create the GitHub repo and commit history,
- write the deploy config (GitHub Pages workflow, or an nginx/Apache vhost, or a
  CI job that copies `index.html` to your server over SSH/rsync),
- make small copy fixes directly in `index.html`,
- keep `source/` and `index.html` in sync in commits.

For substantial **redesign or new-panel** work, the smoothest path is still to
edit `source/Bantu Expansion.dc.html` and rebuild `index.html` in the design
workspace, then let Claude Code commit and deploy the result.

---

## Design reference (for anyone rebuilding it elsewhere)

If you ever want to reimplement this in a framework instead of shipping the
static file, the essentials:

- **Canvas:** six panels, each `1920 × 1080` px, stacked vertically; the live
  page scales one panel to fit the viewport.
- **Palette (earthy / archaeological):**
  - Parchment background `#E9DEC7` (radial wash to `#DED0B3`)
  - Ink / headings `#2E241B`, body `#463829`, muted `#6E5D48`
  - Terracotta accent `#B4502A`, ochre `#C98A2E`, olive `#6F7A3A`,
    brown `#8a5a34`, slate `#64707a`
  - Panel fill `rgba(255,252,244,.5)`, hairline borders `rgba(59,46,34,.14)`
- **Type:** headings & body in **Spectral** (serif); labels/eyebrows/numbers in
  **Barlow Condensed** (uppercase, wide letter-spacing). Loaded from Google
  Fonts in the source; inlined in `index.html`.
- **Illustrations:** hand-built inline SVG line art, lightly CSS-animated
  (keyframes `ill-sway`, `ill-spin`, `ill-pulse`, `ill-grow`, `ill-twinkle`,
  `ill-drop`, `ill-dash`, `ill-bob`). No photographs.
- **Panels:** 1) The Bantu Expansion (map + routes + timeline), 2) The Bantu
  Advantage, 3) Spirits, Ancestors & the Creator, 4) Peoples, Country by
  Country, 5) The Kikuyu & Ngai, 6) How We Know.

## Sources
Content combines Jared Diamond's *Guns, Germs, and Steel* (1997) with more
recent linguistic, archaeological and DNA research (a later, staged expansion;
a smaller role for iron). Broad picture well established; exact dates and routes
still debated.
