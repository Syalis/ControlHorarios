angular
    .module('webapp')
    .filter("jsonDate", ['$filter', jsonDate])
    .filter("sumByProp", sumByProp)
    .filter("sumDif", sumDif)
    .filter("formatExport", formatExport)
    .filter("startFrom", startFrom)
    .filter("propsFilterOR", propsFilterOR)
    .filter("headerStyle", headerStyle)
    .filter("capitalize", capitalize)
    .filter("isCurrency", ['$filter', isCurrency])

function jsonDate($filter) {
    return function (input, format) {
        return (input)
               ? $filter('date')(parseInt(input.substr(6)), format)
               : '';
    };
}

function sumByProp() {
    return function sum(items, prop) {
        if (items) {
            return items.reduce(function (a, b) {
                return a + b[prop];
            }, 0);
        } else {
            return 0;
        }
    };
}

function sumDif() {
    return function sum(items) {
        if (items) {
            return items.reduce(function (a, b) {
                return a + (b["rentabilidad"] - b["optimo"]);
            },
                    0);

        } else {
            return 0;
        }
    };
}

function formatExport() {
    return function (items, scope) {
        var outItems = [];
        if (angular.isArray(items)) {
            angular.forEach(items, function (item) {
                angular.forEach(item, function (value, key) {
                    if (value == null || value == '#EANF#' || value == 'NODATA') {
                        item[key] = '';
                    } else if (angular.isDate(value) || moment(value).isValid()) {
                        item[key] = moment(value).format("DD-MM-YYYY");
                    }
                });
                outItems.push(item);
            });
        }
        return outItems;
    }
}

function startFrom() {
    return function (input, start) {
        if (input) {
            start = +start; //parse to int
            return input.slice(start);
        }
        return [];
    }
}

/**
 * AngularJS default filter with the following expression:
 * "person in people | filter: {name: $select.search, age: $select.search}"
 * performs an AND between 'name: $select.search' and 'age: $select.search'.
 * We want to perform an OR.
 */
function propsFilterOR() {
    return function (items, props) {
        var out = [];

        if (angular.isArray(items)) {
            var keys = Object.keys(props);

            items.forEach(function (item) {
                var itemMatches = false;

                for (var i = 0; i < keys.length; i++) {
                    var prop = keys[i];
                    var text = props[prop].toLowerCase();
                    if (item[prop].toString().toLowerCase().indexOf(text) !== -1) {
                        itemMatches = true;
                        break;
                    }
                }

                if (itemMatches) {
                    out.push(item);
                }
            });
        } else {
            // Let the output be the input untouched
            out = items;
        }

        return out;
    };
}

function headerStyle()
{
    return function (items, props) {
        var out;

        if (items.contains("p_")) {
            items = items.replace("p_", "%_");
        } else {
            items = items.replace("p_", "%_");
        }
        var array = items.split("_");

        for (var i = 0; i < array.length; i++) {
            array[i] = array[i].charAt(0).toUpperCase() + array[i].slice(1);
        }

        out = array.join(" ");

        return out;
    };
}

function isCurrency($filter) {
    return function (items, props) {
        var out;

        if (props.contains("medio") || props == 'valor') {
            out = $filter('currency')(items, '€');
            return out;
        } else {
            return items
        }
    };
}

/**
 * Filtro para Capitalizar palabras (Primera letra en Mayúsculas)
 */
function capitalize() {
    return function (input, all) {
        var reg = (all) ? /([^\W_]+[^\s-]*) */g : /([^\W_]+[^\s-]*)/;
        return (!!input) ? input.replace(reg, function (txt) { return txt.charAt(0).toUpperCase() + txt.substr(1).toLowerCase(); }) : '';
    }
}