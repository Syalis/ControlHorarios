using System.Web;
using System.Web.Optimization;

namespace webapp
{
    public class BundleConfig
    {
        // Para obtener más información sobre Bundles, visite http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = true;

            #if DEBUG
            BundleTable.EnableOptimizations = false;
            #endif

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            // preparado para la producción y podrá utilizar la herramienta de compilación disponible en http://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            // Bootstrap
            bundles.Add(new ScriptBundle("~/bundles/bootstrap/js").Include(
                "~/Plugins/bootstrap/dist/js/bootstrap.min.js"));

            // jQuery
            bundles.Add(new ScriptBundle("~/bundles/jquery/js").Include(
                "~/Plugins/jquery/dist/jquery.min.js",
                "~/Plugins/jquery/jquery-ui.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/animate.css",
                "~/Content/icons.css",
                "~/Content/Site.css",
                //"~/Content/font-awesome.css",
                //"~/Content/style.css",
                "~/Content/style_custom.css",
                "~/Content/smart-table-custom.css",
                "~/Content/webapp-loader.css"));

            // Plugins CSS
            bundles.Add(new StyleBundle("~/bundles/plugins/css").Include(
                "~/Plugins/bootstrap-markdown/bootstrap-markdown.min.css",
                "~/Plugins/gridstack/dist/gridstack.css",
                "~/Plugins/angular-carousel/angular-carousel.min.css",
                "~/Plugins/isteven-select/isteven-multi-select.css",
                "~/Plugins/angular-ui-clock/angular-clock.css",
                "~/Plugins/sweetalert2/sweetalert2.min.css",
                "~/Plugins/angular-bootstrap-datetimepicker/src/css/datetimepicker.css",
                "~/Plugins/jasny-bootstrap/jasny-bootstrap.css",
                "~/Plugins/angular-toastr/angular-toastr.min.css",
                "~/Plugins/angular-xeditable/xeditable.min.css",
                "~/Plugins/angular-ui-select/select.min.css",
                "~/Plugins/ladda/ladda-themeless.min.css",
                "~/Plugins/bootstrap-year-calendar/bootstrap-year-calendar.css"
                ));

            // Plugins JS
            bundles.Add(new ScriptBundle("~/bundles/plugins/js").Include(
                "~/Plugins/moment/moment.js",
                "~/Plugins/moment/locale/es.js",
                "~/Plugins/angular-animate/angular-animate.min.js",
                "~/Plugins/angular-ui-router/angular-ui-router.min.js",
                "~/Plugins/angular-bootstrap/ui-bootstrap.min.js",
                "~/Plugins/angular-bootstrap/ui-bootstrap-tpls-2.4.0.min.js",
                "~/Plugins/angular/angular-locale_es-es.min.js",
                "~/plugins/smart-table/smart-table.min.js",
                //"~/Plugins/chart.js/dist/Chart.min.js",
                "~/Plugins/chartJs/angles.js",
                "~/Plugins/chartJs/Chart.min.js",
                "~/Plugins/angular-filter/dist/angular-filter.min.js",
                "~/Plugins/angular-flot/angular-flot.js",
                "~/Plugins/angular-carousel/angular-carousel.min.js",
                "~/Plugins/bootstrap-markdown/markdown.js",
                "~/Plugins/bootstrap-markdown/bootstrap-markdown.js",
                "~/Plugins/slimscroll/jquery.slimscroll.min.js",
                "~/Plugins/angular-bind-compile/angular-bind-html-compile.js",
                "~/Plugins/angular-sanitize/angular-sanitize.min.js",
                "~/Plugins/isteven-select/isteven-multi-select.js",
                "~/Plugins/angular-ui-clock/angular-clock.js",
                "~/Plugins/sweetalert2/es6-promise.auto.min.js",
                "~/Plugins/sweetalert2/sweetalert2.min.js",
                //"~/Plugins/angular-chart/angular-chart.min.js",
                "~/Plugins/angular-bootstrap-datetimepicker/src/js/datetimepicker.js",
                "~/Plugins/angular-bootstrap-datetimepicker/src/js/datetimepicker.templates.js",
                "~/Plugins/jasny-bootstrap/jasny-bootstrap.js",
                "~/Plugins/toastr/toastr.min.js",
                "~/Plugins/angular-toastr/angular-toastr.min.js",
                "~/plugins/angular-toastr/angular-toastr.tpls.min.js",
                "~/Plugins/jszip/jszip.min.js",
                "~/Plugins/alaSQL/alasql.min.js",
                "~/Plugins/alaSQL/xlsx.min.js",
                "~/Plugins/angular-xeditable/xeditable.min.js",
                "~/Plugins/angular-ui-select/select.min.js",
                "~/Plugins/angular-bootstrap-confirm/angular-bootstrap-confirm.min.js",
                "~/Plugins/angular-datetime/datetime.js",
                "~/Plugins/angular-ui-mask/mask.js",
                "~/Plugins/filesaver/filesaver.min.js",
                "~/Plugins/ladda/spin.min.js",
                "~/Plugins/ladda/ladda.min.js",
                "~/Plugins/angular-ladda/angular-ladda.min.js",
                "~/Plugins/bootstrap-year-calendar/bootstrap-year-calendar.js"
                ));

            // webapp scripts
            bundles.Add(new ScriptBundle("~/bundles/webapp/js").Include(
                "~/app/app.js",
                "~/app/config.js",
                "~/app/jquery.app.js")
                .IncludeDirectory("~/app/directives", "*.js")
                .IncludeDirectory("~/app/filters", "*.js")
                .IncludeDirectory("~/app/factories", "*.js")
                .IncludeDirectory("~/app/components", "*.js", true)
                .IncludeDirectory("~/app/controllers", "*.js"));
        }
    }
}
