/*
 * Overlap / overflow self-check for "Bantu Expansion.dc.html".
 * Run against the LIVE preview via eval_js (needs rendered DOM + geometry).
 * Returns [] when clean; otherwise an array of problems per page.
 *
 * Checks, for every page (.page-wrap > [data-screen-label]):
 *   1. OVERFLOW  — any text node whose box spills past the 1920x1080 page edge.
 *   2. TIMELINE  — any text outside a timeline strip whose box collides with it
 *                  (the recurring "timeline overlaps text" bug on Fig 1).
 *   3. PANEL     — any flex/þlock column whose scrollHeight exceeds clientHeight
 *                  by >4px real (content taller than its grid cell).
 *   4. SVG-LABEL — any two <svg> <text> labels whose boxes collide, excluding
 *                  intended label+sublabel two-line stacks.
 *
 * Usage (paste the body into eval_js, or load and call):
 *   return runOverlapCheck();
 */
function runOverlapCheck() {
  function overlap(a, b) {
    return !(a.right <= b.left || a.left >= b.right || a.bottom <= b.top || a.top >= b.bottom);
  }
  var pages = [].slice.call(document.querySelectorAll('.page-wrap'));
  var prevDisplay = pages.map(function (p) { return p.style.display; });
  var problems = [];

  pages.forEach(function (pw) {
    pw.style.display = 'flex';
    var page = pw.querySelector('[data-screen-label]');
    if (!page) return;
    var label = page.getAttribute('data-screen-label');
    var scale = page.getBoundingClientRect().width / 1920 || 1;
    var pr = page.getBoundingClientRect();

    // 1. edge overflow
    page.querySelectorAll('*').forEach(function (el) {
      if (!el.textContent.trim()) return;
      var r = el.getBoundingClientRect();
      if (!r.height || !r.width) return;
      if (r.bottom > pr.bottom + 1.5 || r.right > pr.right + 1.5) {
        problems.push({ page: label, type: 'OVERFLOW',
          text: el.textContent.trim().slice(0, 40),
          overBottom: +((r.bottom - pr.bottom) / scale).toFixed(0),
          overRight: +((r.right - pr.right) / scale).toFixed(0) });
      }
    });

    // 2. timeline collision
    var tls = [].slice.call(page.querySelectorAll('div')).filter(function (el) {
      var m = (el.textContent.match(/\b(BCE|CE)\b/g) || []).length;
      return m >= 3 && el.querySelectorAll('div').length >= 4 &&
             el.getBoundingClientRect().height < 260;
    });
    tls.sort(function (a, b) { return a.getBoundingClientRect().top - b.getBoundingClientRect().top; });
    var tl = tls[tls.length - 1];
    if (tl) {
      var tr = tl.getBoundingClientRect();
      page.querySelectorAll('p,h1,h2,h3,span,div').forEach(function (el) {
        if (tl.contains(el) || el.contains(tl) || el === tl) return;
        if (!el.textContent.trim()) return;
        var r = el.getBoundingClientRect();
        if (!r.height) return;
        if (overlap(r, tr)) {
          problems.push({ page: label, type: 'TIMELINE',
            text: el.textContent.trim().slice(0, 40),
            intoTimeline: +((r.bottom - tr.top) / scale).toFixed(0) });
        }
      });
    }

    // 3. panel content taller than its cell
    page.querySelectorAll('div').forEach(function (el) {
      var st = getComputedStyle(el);
      if (st.display.indexOf('flex') < 0 && st.overflow === 'visible') return;
      var over = el.scrollHeight - el.clientHeight;
      if (over > 4 && el.clientHeight > 120 && el.textContent.trim().length > 60) {
        problems.push({ page: label, type: 'PANEL',
          text: el.textContent.trim().slice(0, 40), overflowPx: over });
      }
    });

    // 4. SVG map label collisions (ignore intended label+sublabel stacks:
    //    a shorter line sitting directly under a longer one, same anchor x).
    var svgs = page.querySelectorAll('svg');
    svgs.forEach(function (svg) {
      var ts = [].slice.call(svg.querySelectorAll('text')).map(function (t) {
        var r = t.getBoundingClientRect();
        return { t: t, txt: t.textContent.trim().slice(0, 24), r: r };
      }).filter(function (o) { return o.r.width && o.r.height; });
      for (var a = 0; a < ts.length; a++) {
        for (var b = a + 1; b < ts.length; b++) {
          var A = ts[a].r, B = ts[b].r;
          if (A.right <= B.left || A.left >= B.right || A.bottom <= B.top || A.top >= B.bottom) continue;
          // stacked sublabel test: vertical neighbours (<18px real apart) that share a left/centre edge
          var vGap = Math.abs((A.top < B.top ? B.top - A.bottom : A.top - B.bottom)) / scale;
          var sharedX = Math.abs(A.left - B.left) < 6 || Math.abs((A.left + A.right) - (B.left + B.right)) < 8;
          if (vGap < 8 && sharedX) continue; // intended two-line label
          problems.push({ page: label, type: 'SVG-LABEL',
            a: ts[a].txt, b: ts[b].txt });
        }
      }
    });
  });

  pages.forEach(function (p, i) { p.style.display = prevDisplay[i] || (i === 0 ? 'flex' : 'none'); });
  // de-dup identical entries
  var seen = {};
  return problems.filter(function (p) {
    var k = p.page + '|' + p.type + '|' + p.text;
    if (seen[k]) return false; seen[k] = 1; return true;
  });
}
