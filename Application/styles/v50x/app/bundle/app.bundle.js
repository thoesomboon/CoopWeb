'use strict';
var KTDemoPanel = function () {
  var t,
  e = KTUtil.getByID('kt_demo_panel');
  return {
    init: function () {
      !function () {
        t = new KTOffcanvas(e, {
          overlay: !0,
          baseClass: 'kt-demo-panel',
          closeBy: 'kt_demo_panel_close',
          toggleBy: 'kt_demo_panel_toggle'
        });
        var i = KTUtil.find(e, '.kt-demo-panel__head'),
        a = KTUtil.find(e, '.kt-demo-panel__body');
        KTUtil.scrollInit(a, {
          disableForMobile: !0,
          resetHeightOnDestroy: !0,
          handleWindowResize: !0,
          height: function () {
            var t = parseInt(KTUtil.getViewPort().height);
            return i && (t -= parseInt(KTUtil.actualHeight(i)), t -= parseInt(KTUtil.css(i, 'marginBottom'))),
            t -= parseInt(KTUtil.css(e, 'paddingTop')),
            t -= parseInt(KTUtil.css(e, 'paddingBottom'))
          }
        }),
        void 0 === t || $.isEmptyObject(t) || t.on('hide', function () {
          alert(1);
          var t = new Date((new Date).getTime() + 3600000);
          Cookies.set('kt_demo_panel_shown', 1, {
            expires: t
          })
        })
      }(),
      'keenthemes.com' != encodeURI(window.location.hostname) && 'www.keenthemes.com' != encodeURI(window.location.hostname) || setTimeout(function () {
        if (!Cookies.get('kt_demo_panel_shown')) {
          var e = new Date((new Date).getTime() + 900000);
          Cookies.set('kt_demo_panel_shown', 1, {
            expires: e
          }),
          t.show()
        }
      }, 4000)
    }
  }
}();
KTUtil.ready(function () {
  KTDemoPanel.init()
});
var KTLib = {
  initMiniChart: function (t, e, i, a, n, o) {
    if (0 != t.length) {
      n = void 0 !== n && n,
      o = void 0 !== o && o;
      var l = {
        type: 'line',
        data: {
          labels: [
            'January',
            'February',
            'March',
            'April',
            'May',
            'June',
            'July',
            'August',
            'September',
            'October'
          ],
          datasets: [
            {
              label: '',
              borderColor: i,
              borderWidth: a,
              pointHoverRadius: 4,
              pointHoverBorderWidth: 4,
              pointBackgroundColor: Chart.helpers.color('#000000').alpha(0).rgbString(),
              pointBorderColor: Chart.helpers.color('#000000').alpha(0).rgbString(),
              pointHoverBackgroundColor: KTApp.getStateColor('brand'),
              pointHoverBorderColor: Chart.helpers.color('#000000').alpha(0.1).rgbString(),
              fill: n,
              backgroundColor: i,
              data: e
            }
          ]
        },
        options: {
          title: {
            display: !1
          },
          tooltips: !!o && {
            enabled: !0,
            intersect: !1,
            mode: 'nearest',
            bodySpacing: 5,
            yPadding: 10,
            xPadding: 10,
            caretPadding: 0,
            displayColors: !1,
            backgroundColor: KTApp.getStateColor('brand'),
            titleFontColor: '#ffffff',
            cornerRadius: 4,
            footerSpacing: 0,
            titleSpacing: 0
          },
          legend: {
            display: !1,
            labels: {
              usePointStyle: !1
            }
          },
          responsive: !1,
          maintainAspectRatio: !0,
          hover: {
            mode: 'index'
          },
          scales: {
            xAxes: [
              {
                display: !1,
                gridLines: !1,
                scaleLabel: {
                  display: !1,
                  labelString: 'Month'
                }
              }
            ],
            yAxes: [
              {
                display: !1,
                gridLines: !1,
                scaleLabel: {
                  display: !1,
                  labelString: 'Month'
                }
              }
            ]
          },
          elements: {
            line: {
              tension: 0.5
            },
            point: {
              radius: 2,
              borderWidth: 4
            }
          },
          layout: {
            padding: {
              left: 6,
              right: 0,
              top: 4,
              bottom: 0
            }
          }
        }
      };
      new Chart(t, l)
    }
  },
  initMediumChart: function (t, e, i, a, n) {
    if (document.getElementById(t)) {
      n = n || 2;
      var o = document.getElementById(t).getContext('2d'),
      l = o.createLinearGradient(0, 0, 0, 100);
      l.addColorStop(0, Chart.helpers.color(a).alpha(0.3).rgbString()),
      l.addColorStop(1, Chart.helpers.color(a).alpha(0).rgbString());
      var r = {
        type: 'line',
        data: {
          labels: [
            'January',
            'February',
            'March',
            'April',
            'May',
            'June',
            'July',
            'August',
            'September',
            'October'
          ],
          datasets: [
            {
              label: 'Orders',
              borderColor: a,
              borderWidth: n,
              backgroundColor: l,
              pointBackgroundColor: KTApp.getStateColor('brand'),
              data: e
            }
          ]
        },
        options: {
          responsive: !0,
          maintainAspectRatio: !1,
          title: {
            display: !1,
            text: 'Stacked Area'
          },
          tooltips: {
            enabled: !0,
            intersect: !1,
            mode: 'nearest',
            bodySpacing: 5,
            yPadding: 10,
            xPadding: 10,
            caretPadding: 0,
            displayColors: !1,
            backgroundColor: KTApp.getStateColor('brand'),
            titleFontColor: '#ffffff',
            cornerRadius: 4,
            footerSpacing: 0,
            titleSpacing: 0
          },
          legend: {
            display: !1,
            labels: {
              usePointStyle: !1
            }
          },
          hover: {
            mode: 'index'
          },
          scales: {
            xAxes: [
              {
                display: !1,
                scaleLabel: {
                  display: !1,
                  labelString: 'Month'
                },
                ticks: {
                  display: !1,
                  beginAtZero: !0
                }
              }
            ],
            yAxes: [
              {
                display: !1,
                scaleLabel: {
                  display: !1,
                  labelString: 'Value'
                },
                gridLines: {
                  color: '#eef2f9',
                  drawBorder: !1,
                  offsetGridLines: !0,
                  drawTicks: !1
                },
                ticks: {
                  max: i,
                  display: !1,
                  beginAtZero: !0
                }
              }
            ]
          },
          elements: {
            point: {
              radius: 0,
              borderWidth: 0,
              hoverRadius: 0,
              hoverBorderWidth: 0
            }
          },
          layout: {
            padding: {
              left: 0,
              right: 0,
              top: 0,
              bottom: 0
            }
          }
        }
      },
      s = new Chart(o, r);
      KTUtil.addResizeHandler(function () {
        s.update()
      })
    }
  }
},
KTOffcanvasPanel = function () {
  var t = KTUtil.get('kt_offcanvas_toolbar_notifications'),
  e = KTUtil.get('kt_offcanvas_toolbar_quick_actions'),
  i = KTUtil.get('kt_offcanvas_toolbar_profile'),
  a = KTUtil.get('kt_offcanvas_toolbar_search');
  return {
    init: function () {
      !function () {
        var e = KTUtil.find(t, '.kt-offcanvas-panel__head'),
        i = KTUtil.find(t, '.kt-offcanvas-panel__body');
        new KTOffcanvas(t, {
          overlay: !0,
          baseClass: 'kt-offcanvas-panel',
          closeBy: 'kt_offcanvas_toolbar_notifications_close',
          toggleBy: 'kt_offcanvas_toolbar_notifications_toggler_btn'
        });
        KTUtil.scrollInit(i, {
          disableForMobile: !0,
          resetHeightOnDestroy: !0,
          handleWindowResize: !0,
          height: function () {
            var i = parseInt(KTUtil.getViewPort().height);
            return e && (i -= parseInt(KTUtil.actualHeight(e)), i -= parseInt(KTUtil.css(e, 'marginBottom'))),
            i -= parseInt(KTUtil.css(t, 'paddingTop')),
            i -= parseInt(KTUtil.css(t, 'paddingBottom'))
          }
        })
      }(),
      function () {
        var t = KTUtil.find(e, '.kt-offcanvas-panel__head'),
        i = KTUtil.find(e, '.kt-offcanvas-panel__body');
        new KTOffcanvas(e, {
          overlay: !0,
          baseClass: 'kt-offcanvas-panel',
          closeBy: 'kt_offcanvas_toolbar_quick_actions_close',
          toggleBy: 'kt_offcanvas_toolbar_quick_actions_toggler_btn'
        });
        KTUtil.scrollInit(i, {
          disableForMobile: !0,
          resetHeightOnDestroy: !0,
          handleWindowResize: !0,
          height: function () {
            var i = parseInt(KTUtil.getViewPort().height);
            return t && (i -= parseInt(KTUtil.actualHeight(t)), i -= parseInt(KTUtil.css(t, 'marginBottom'))),
            i -= parseInt(KTUtil.css(e, 'paddingTop')),
            i -= parseInt(KTUtil.css(e, 'paddingBottom'))
          }
        })
      }(),
      function () {
        var t = KTUtil.find(i, '.kt-offcanvas-panel__head'),
        e = KTUtil.find(i, '.kt-offcanvas-panel__body');
        new KTOffcanvas(i, {
          overlay: !0,
          baseClass: 'kt-offcanvas-panel',
          closeBy: 'kt_offcanvas_toolbar_profile_close',
          toggleBy: 'kt_offcanvas_toolbar_profile_toggler_btn'
        });
        KTUtil.scrollInit(e, {
          disableForMobile: !0,
          resetHeightOnDestroy: !0,
          handleWindowResize: !0,
          height: function () {
            var e = parseInt(KTUtil.getViewPort().height);
            return t && (e -= parseInt(KTUtil.actualHeight(t)), e -= parseInt(KTUtil.css(t, 'marginBottom'))),
            e -= parseInt(KTUtil.css(i, 'paddingTop')),
            e -= parseInt(KTUtil.css(i, 'paddingBottom'))
          }
        })
      }(),
      function () {
        var t = KTUtil.find(a, '.kt-offcanvas-panel__head'),
        e = (KTUtil.find(a, '.kt-offcanvas-panel__body'), KTUtil.get('kt_quick_search_offcanvas')),
        i = KTUtil.find(e, '.kt-quick-search__form'),
        n = KTUtil.find(e, '.kt-quick-search__wrapper');
        new KTOffcanvas(a, {
          overlay: !0,
          baseClass: 'kt-offcanvas-panel',
          closeBy: 'kt_offcanvas_toolbar_search_close',
          toggleBy: 'kt_offcanvas_toolbar_search_toggler_btn'
        });
        KTUtil.scrollInit(n, {
          disableForMobile: !0,
          resetHeightOnDestroy: !0,
          handleWindowResize: !0,
          height: function () {
            var e = parseInt(KTUtil.getViewPort().height);
            return e -= parseInt(KTUtil.actualHeight(i)),
            e -= parseInt(KTUtil.css(i, 'marginBottom')),
            t && (e -= parseInt(KTUtil.actualHeight(t)), e -= parseInt(KTUtil.css(t, 'marginBottom'))),
            e -= parseInt(KTUtil.css(a, 'paddingTop')),
            e -= parseInt(KTUtil.css(a, 'paddingBottom'))
          }
        })
      }()
    }
  }
}();
KTUtil.ready(function () {
  KTOffcanvasPanel.init()
});
var KTQuickPanel = function () {
  var t = KTUtil.get('kt_quick_panel'),
  e = KTUtil.get('kt_quick_panel_tab_notifications'),
  i = KTUtil.get('kt_quick_panel_tab_actions'),
  a = KTUtil.get('kt_quick_panel_tab_settings'),
  n = function () {
    var e = KTUtil.find(t, '.kt-offcanvas-panel__nav');
    KTUtil.find(t, '.kt-offcanvas-panel__body');
    return parseInt(KTUtil.getViewPort().height) - parseInt(KTUtil.actualHeight(e)) - parseInt(KTUtil.css(e, 'margin-bottom')) - 2 * parseInt(KTUtil.css(e, 'padding-top')) - 10
  };
  return {
    init: function () {
      new KTOffcanvas(t, {
        overlay: !0,
        baseClass: 'kt-offcanvas-panel',
        closeBy: 'kt_quick_panel_close_btn',
        toggleBy: 'kt_quick_panel_toggler_btn'
      }),
      KTUtil.scrollInit(e, {
        disableForMobile: !0,
        resetHeightOnDestroy: !0,
        handleWindowResize: !0,
        height: function () {
          return n()
        }
      }),
      KTUtil.scrollInit(i, {
        disableForMobile: !0,
        resetHeightOnDestroy: !0,
        handleWindowResize: !0,
        height: function () {
          return n()
        }
      }),
      KTUtil.scrollInit(a, {
        disableForMobile: !0,
        resetHeightOnDestroy: !0,
        handleWindowResize: !0,
        height: function () {
          return n()
        }
      }),
      $(t).find('a[data-toggle="tab"]').on('shown.bs.tab', function (t) {
        KTUtil.scrollUpdate(e),
        KTUtil.scrollUpdate(i),
        KTUtil.scrollUpdate(a)
      })
    }
  }
}();
KTUtil.ready(function () {
  KTQuickPanel.init()
});
var KTQuickSearch = function () {
  var t,
  e,
  i,
  a,
  n,
  o,
  l,
  r,
  s = '',
  c = !1,
  d = !1,
  p = !1,
  f = 'kt-spinner kt-spinner--input kt-spinner--sm kt-spinner--brand kt-spinner--right',
  g = 'kt-quick-search--has-result',
  u = function () {
    p = !1,
    KTUtil.removeClass(r, f),
    a && (i.value.length < 2 ? KTUtil.hide(a)  : KTUtil.show(a, 'flex'))
  },
  _ = function () {
    l && !KTUtil.hasClass(o, 'show') && ($(l).dropdown('toggle'), $(l).dropdown('update'))
  },
  h = function () {
    l && KTUtil.hasClass(o, 'show') && $(l).dropdown('toggle')
  },
  T = function () {
    if (c && s === i.value) return u(),
    KTUtil.addClass(t, g),
    _(),
    void KTUtil.scrollUpdate(n);
    s = i.value,
    KTUtil.removeClass(t, g),
    p = !0,
    KTUtil.addClass(r, f),
    a && KTUtil.hide(a),
    h(),
    setTimeout(function () {
      $.ajax({
        url: 'https://keenthemes.com/keen/themes/themes/keen/dist/preview/inc/api/quick_search.php',
        data: {
          query: s
        },
        dataType: 'html',
        success: function (e) {
          c = !0,
          u(),
          KTUtil.addClass(t, g),
          KTUtil.setHTML(n, e),
          _(),
          KTUtil.scrollUpdate(n)
        },
        error: function (e) {
          c = !1,
          u(),
          KTUtil.addClass(t, g),
          KTUtil.setHTML(n, '<span class="kt-quick-search__message">Connection error. Pleae try again later.</div>'),
          _(),
          KTUtil.scrollUpdate(n)
        }
      })
    }, 1000)
  },
  K = function (e) {
    i.value = '',
    s = '',
    c = !1,
    KTUtil.hide(a),
    KTUtil.removeClass(t, g),
    h()
  },
  k = function () {
    if (i.value.length < 2) return u(),
    void h();
    1 != p && (d && clearTimeout(d), d = setTimeout(function () {
      T()
    }, 200))
  };
  return {
    init: function (s) {
      t = s,
      e = KTUtil.find(t, '.kt-quick-search__form'),
      i = KTUtil.find(t, '.kt-quick-search__input'),
      a = KTUtil.find(t, '.kt-quick-search__close'),
      n = KTUtil.find(t, '.kt-quick-search__wrapper'),
      o = KTUtil.find(t, '.dropdown-menu'),
      l = KTUtil.find(t, '[data-toggle="dropdown"]'),
      r = KTUtil.find(t, '.input-group'),
      KTUtil.addEvent(i, 'keyup', k),
      KTUtil.addEvent(i, 'focus', k),
      e.onkeypress = function (t) {
        13 == (t.charCode || t.keyCode || 0) && t.preventDefault()
      },
      KTUtil.addEvent(a, 'click', K)
    }
  }
},
KTQuickSearchMobile = KTQuickSearch;
KTUtil.ready(function () {
  KTUtil.get('kt_quick_search_dropdown') && KTQuickSearch().init(KTUtil.get('kt_quick_search_dropdown')),
  KTUtil.get('kt_quick_search_inline') && KTQuickSearchMobile().init(KTUtil.get('kt_quick_search_inline')),
  KTUtil.get('kt_quick_search_offcanvas') && KTQuickSearchMobile().init(KTUtil.get('kt_quick_search_offcanvas'))
});
