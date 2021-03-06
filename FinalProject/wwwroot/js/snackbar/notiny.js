! function (t) {
    var i = t("body"),
        n = t('<div class="notiny" />').appendTo(i),
        e = { image: void 0, position: "right-bottom", theme: "dark", template: '<div class="notiny-base"><img class="notiny-img" /><div class="notiny-text"></div></div>', width: "300", text: "", background: !0, autohide: !0, clickhide: !0, delay: 3e3, animate: !0, animation_show: "notiny-animation-show 0.4s forwards", animation_hide: "notiny-animation-hide 0.5s forwards" },
        o = { container_class: "", notification_class: "", image_class: "", text_class: "" },
        a = { "left-top": t("<div />", { "class": "notiny-container", css: { top: 10, left: 10 } }).appendTo(n), "left-bottom": t("<div />", { "class": "notiny-container", css: { bottom: 10, left: 10 } }).appendTo(n), "right-top": t("<div />", { "class": "notiny-container", css: { top: 10, right: 10 } }).appendTo(n), "right-bottom": t("<div />", { "class": "notiny-container", css: { bottom: 10, right: 10 } }).appendTo(n) },
        s = function (t) {
            var i = !1,
                n = "Webkit Moz ms O".split(" "),
                e = document.createElement("div"),
                o = null;
            if (t = t.toLowerCase(), void 0 !== e.style[t] && (i = !0), i === !1) {
                o = t.charAt(0).toUpperCase() + t.substr(1);
                for (var a = 0; a < n.length; a++)
                    if (void 0 !== e.style[n[a] + o]) { i = !0; break }
            }
            return i
        },
        d = function (t, i) { i.animate ? i._state_closing || (i._state_closing = !0, s("animation") && s("transform") ? (t.css("animation", i.animation_hide), setTimeout(function () { t.remove() }, 550)) : t.fadeOut(400, function () { t.remove() })) : t.remove() },
        c = function (t, i) { i.animate && (s("animation") && s("transform") ? t.css("animation", i.animation_show) : (t.hide(), t.fadeIn(500))) },
        m = function (i) { r(t.extend({}, e, i)) },
        r = function (i) {
            var n = t(i.template);
            i.theme = t.notiny.themes[i.theme], n.addClass(i.theme.notification_class);
            var e = n.find(".notiny-text");
            e.addClass(i.theme.text_class), e.html(i.text);
            var o = n.find(".notiny-img");
            void 0 !== i.image ? (n.addClass("notiny-with-img"), o.css("display", "block"), o.addClass(i.theme.image_class), o.attr("src", i.image)) : (o.hide(), n.addClass("notiny-without-img")), n.css("width", i.width), l(n, i)
        },
        l = function (i, n) {
            (void 0 !== n.x || void 0 !== n.y) && t.notiny({ text: "<b>WARNING!:</b> <b>x</b> and <b>y</b> options was removed, please use <b>position</b> instead!", width: "auto" });
            var e = a[n.position];
            e.addClass(n.theme.container_class), "top" === n.position.slice(-3) ? e.prepend(i) : e.append(i);
            var o = n.position.split("-")[0];
            i.css("float", o), i.css("clear", o), n._state_closing = !1, n.clickhide && (i.css("cursor", "pointer"), i.on("click", function () { return d(i, n), !1 })), n.autohide && setTimeout(function () { d(i, n) }, n.delay + 500), c(i, n)
        };
    t.notiny = function (t) { return m(t), this }, t.notiny.addTheme = function (i, n) {
        var e = t.extend({}, o, n);
        (this.themes = this.themes || {})[i] = e
    }, t.notinyAddTheme = function () { t.notiny({ text: "<b>WARNING!:</b> <b>$.notinyAddTheme</b> was removed, please use <b>$.notiny.addTheme</b> instead!", width: "auto" }) }, t.notiny.addTheme("dark", { notification_class: "notiny-theme-dark notiny-default-vars" }), t.notiny.addTheme("light", { notification_class: "notiny-theme-light notiny-default-vars" }),
        t.notiny.addTheme("success", { notification_class: "notiny-theme-success notiny-default-vars" }),
        t.notiny.addTheme("danger", { notification_class: "notiny-theme-danger notiny-default-vars" }) 
}(jQuery);