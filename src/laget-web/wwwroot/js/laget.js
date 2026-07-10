// Små, rammeverk-uavhengige finesser som Blazor ikke gjør selv:
//  1) en hilsen i nettleserkonsollen til nysgjerrige som åpner DevTools
//  2) en global hurtigtast (`) som åpner den skjulte terminalen
//  3) en «matrix»-regn-effekt som terminalkommandoen `matrix` kaller
(function () {
    // 1) Konsoll-hilsen ------------------------------------------------------
    var big = "font:600 40px 'Fira Code',monospace;";
    console.log(
        "%claget%c.%cno",
        big + "color:#e5e9f0", big + "color:#fea55f", big + "color:#e5e9f0");
    console.log(
        "%cpsst — trykk  `  (backtick) hvor som helst for en overraskelse 🤓",
        "color:#43d9ad;font:14px 'Fira Code',monospace");

    // 2) Global hurtigtast for terminalen -----------------------------------
    document.addEventListener("keydown", function (e) {
        var t = e.target;
        var typing = t && (t.tagName === "INPUT" || t.tagName === "TEXTAREA" || t.isContentEditable);
        // Backtick åpner/lukker — men ikke mens man skriver i et tekstfelt.
        if (e.key === "`" && !typing) {
            e.preventDefault();
            if (window.DotNet) {
                DotNet.invokeMethodAsync("LagetWeb", "Toggle");
            }
        }
    });

    // 3) Matrix-regn (kort, selvopprydende overlay) -------------------------
    window.lagetMatrix = function (seconds) {
        if (document.getElementById("laget-matrix")) return;
        var canvas = document.createElement("canvas");
        canvas.id = "laget-matrix";
        canvas.style.cssText =
            "position:fixed;inset:0;z-index:9998;pointer-events:none;opacity:0.85";
        document.body.appendChild(canvas);

        var ctx = canvas.getContext("2d");
        var w, h, columns, drops;
        function resize() {
            w = canvas.width = window.innerWidth;
            h = canvas.height = window.innerHeight;
            columns = Math.floor(w / 14);
            drops = new Array(columns).fill(1);
        }
        resize();
        window.addEventListener("resize", resize);

        var glyphs = "01<>[]{}/\\=+*ﾊﾐﾋｰｳｼﾅﾉ日ﾎﾂ";
        var timer = setInterval(function () {
            ctx.fillStyle = "rgba(11,11,13,0.10)";
            ctx.fillRect(0, 0, w, h);
            ctx.fillStyle = "#43d9ad";
            ctx.font = "14px 'Fira Code', monospace";
            for (var i = 0; i < drops.length; i++) {
                var ch = glyphs[Math.floor(Math.random() * glyphs.length)];
                ctx.fillText(ch, i * 14, drops[i] * 14);
                if (drops[i] * 14 > h && Math.random() > 0.975) drops[i] = 0;
                drops[i]++;
            }
        }, 55);

        setTimeout(function () {
            clearInterval(timer);
            window.removeEventListener("resize", resize);
            canvas.style.transition = "opacity 0.8s";
            canvas.style.opacity = "0";
            setTimeout(function () { canvas.remove(); }, 800);
        }, (seconds || 6) * 1000);
    };

    // Auto-scroll terminalen til nyeste linje.
    window.lagetScrollBottom = function (el) {
        if (el) el.scrollTop = el.scrollHeight;
    };
})();
