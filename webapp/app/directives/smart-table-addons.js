(function (ng) {
    //TODO: Directivas para filtros True/False, multiselect, etc...
    angular.module('webapp')
        /*
         * pageSelect - Directiva para paginador de Smart Table Customizado
         */
        .directive('pageSelect', function pageSelect() {
            return {
                restrict: 'E',
                template: '<input type="text" ng-model="inputPage" ng-change="selectPage(inputPage)" style="padding: 0;margin: -5px 0;width: 50px;text-align: center;border: 1px solid #cccccc;" />',
                link: function (scope, element, attrs) {
                    scope.$watch('currentPage', function (c) {
                        scope.inputPage = c;
                    });
                }
            };
        })
        /*
         * Botón para limpiar todos los filtros aplicados
         */
        .directive("stClearFilters", function () {
            return {
                restrict: 'EA',
                require: '^stTable',
                link: function (scope, element, attrs, ctrl) {
                    return element.bind('click', function () {
                        return scope.$apply(function () {
                            var tableState;
                            tableState = ctrl.tableState();
                            tableState.search.predicateObject = {};
                            tableState.pagination.start = 0;
                            return ctrl.pipe();
                        });
                    });
                }
            };
        })
        /*
         * Devuelve la colección filtrada y ordenada
         */
        .directive('stFilteredCollection', function () {
            return {
                restrict: 'A',
                require: '^stTable',
                scope: {
                    stFilteredCollection: '='
                },
                controller: 'stTableController',
                link: function (scope, element, attr, ctrl) {
                    scope.$watch(function () {
                        return ctrl.getFilteredCollection();
                    }, function (newValue, oldValue) {
                        scope.stFilteredCollection = ctrl.getFilteredCollection();
                    });
                }
            };
        })
        .directive('stSummary', function () {
            return {
                restrict: 'E',
                require: '^stTable',
                template: '<div style="padding-top: 10px">Registros: <b>{{size}}</b></div>',
                scope: {},
                link: function (scope, element, attr, ctrl) {
                    scope.$watch(ctrl.getFilteredCollection, function (val) {
                        scope.size = (val || []).length;
                    })
                }
            }
        })
        .directive('stSelectDistinct', [function () {
            return {
                restrict: 'E',
                require: '^stTable',
                scope: {
                    collection: '=',
                    predicate: '@',
                    predicateExpression: '='
                },
                template: '<select ng-model="selectedOption" ng-change="optionChanged(selectedOption)" ng-options="opt for opt in distinctItems" class="form-control input-sm"></select>',
                link: function (scope, element, attr, table) {
                    var getPredicate = function () {
                        var predicate = scope.predicate;
                        if (!predicate && scope.predicateExpression) {
                            predicate = scope.predicateExpression;
                        }
                        return predicate;
                    }

                    scope.$watch('collection', function (newValue) {
                        var predicate = getPredicate();

                        if (newValue) {
                            var temp = [];
                            scope.distinctItems = ['Todo'];

                            angular.forEach(scope.collection, function (item) {
                                var value = item[predicate];

                                if (value && value.trim().length > 0 && temp.indexOf(value) === -1) {
                                    temp.push(value);
                                }
                            });
                            temp.sort();

                            scope.distinctItems = scope.distinctItems.concat(temp);
                            scope.selectedOption = scope.distinctItems[0];
                            scope.optionChanged(scope.selectedOption);
                        }
                    }, true);

                    scope.optionChanged = function (selectedOption) {
                        var predicate = getPredicate();

                        var query = {};

                        query.distinct = selectedOption;

                        if (query.distinct === 'Todo') {
                            query.distinct = '';
                        }

                        table.search(query, predicate);
                    };
                }
            }
        }])
        .directive('stSelectMultiple', ['$filter', function ($filter) {
            return {
                restrict: 'E',
                require: '^stTable',
                scope: {
                    collection: '=',
                    predicate: '@',
                    predicateExpression: '='
                },
                template: '<div class="dropdown">' +
                            '<button class="btn btn-sm btn-default btn-block dropdown-toggle" type="button" id="dropdownMenu1" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true"' +
                            'style="text-align: left; padding-left: 15px;">' +
                                '{{dropdownLabel}}' +
                                '<span class="caret" style="float: right; margin-right: 5px; margin-top: 8px;"></span>' +
                            '</button>' +
                            '<ul class="dropdown-menu" aria-labelledby="dropdownMenu1" style="padding-left: 10px; padding-right: 10px">' +
                                '<li ng-click="$event.stopPropagation()"><input type="text" ng-model="search" class="form-control input-sm" /></li>' +
                                '<li ng-repeat="item in distinctItems | filter:search" ng-click="$event.stopPropagation()">' +
                                    '<div class="checkbox">' +
                                        '<input type="checkbox" ng-model="item.selected" ng-click="filterChanged()">' +
                                        '<label>{{item.value}}</label>' +
                                    '</div>' +
                                '</li>' +
                                '<li><hr style="margin-top: 10px; margin-bottom: 10px" /></li>' +
                                '<li><button type="button" class="btn btn-default btn-sm pull-right">Cerrar</button></li>' +
                            '</ul>' +
                        '</div>',
                link: function (scope, element, attr, table) {
                    scope.dropdownLabel = '';
                    scope.filterChanged = filterChanged;

                    function getPredicate() {
                        var predicate = scope.predicate;
                        if (!predicate && scope.predicateExpression) {
                            predicate = scope.predicateExpression;
                        }
                        return predicate;
                    }

                    scope.$watch('collection', function (newValue, oldValue) {
                        var predicate = getPredicate();
                        var distinctItems = [];

                        angular.forEach(scope.collection, function (item) {
                            var value = item[predicate];
                            fillDistinctItems(value, distinctItems);
                        });

                        distinctItems.sort(function (obj, other) {
                            if (obj.value > other.value) {
                                return 1;
                            } else if (obj.value < other.value) {
                                return -1;
                            }
                            return 0;
                        });

                        scope.distinctItems = distinctItems;

                        filterChanged();
                    }, true);

                    function getDropdownLabel() {
                        var allCount = scope.distinctItems.length;

                        var selected = getSelectedOptions();

                        if (allCount === selected.length || selected.length === 0) {
                            return 'Todo';
                        }

                        if (selected.length === 1) {
                            return selected[0];
                        }

                        return selected.length + ' Seleccionados';
                    }

                    function getSelectedOptions() {
                        var selectedOptions = [];

                        angular.forEach(scope.distinctItems, function (item) {
                            if (item.selected) {
                                selectedOptions.push(item.value);
                            }
                        });

                        return selectedOptions;
                    }

                    function filterChanged() {
                        scope.dropdownLabel = getDropdownLabel();

                        var predicate = getPredicate();

                        var query = {
                            matchAny: {}
                        };

                        query.matchAny.items = getSelectedOptions();
                        var numberOfItems = query.matchAny.items.length;
                        if (numberOfItems === 0 || numberOfItems === scope.distinctItems.length) {
                            query.matchAny.all = true;
                        } else {
                            query.matchAny.all = false;
                        }

                        table.search(query, predicate);
                    }

                    function fillDistinctItems(value, distinctItems) {
                        var itemExist = $filter("filter")(distinctItems, { value: value }).length > 0;
                        if (value && value.trim().length > 0 && !itemExist) {
                            var itemsFound = $filter("filter")(scope.distinctItems, { value: value });
                            if (itemsFound.length) {
                                distinctItems.push(itemsFound[0]);
                            } else {
                                distinctItems.push({ value: value, selected: false });
                            }
                        }
                    }

                    //function fillDistinctItems(value, distinctItems) {
                    //    if (value && value.trim().length > 0 && !findItemWithValue(distinctItems, value)) {
                    //        distinctItems.push({
                    //            value: value,
                    //            selected: false
                    //        });
                    //    }
                    //}

                    //function findItemWithValue(collection, value) {
                    //    var found = _.find(collection, function (item) {
                    //        return item.value === value;
                    //    });

                    //    return found;
                    //}
                }
            }
        }])
        .filter('customFilter', ['$filter', function ($filter) {
            var filterFilter = $filter('filter');
            var standardComparator = function standardComparator(obj, text) {
                text = ('' + text).toLowerCase();
                return ('' + obj).toLowerCase().indexOf(text) > -1;
            };

            return function customFilter(array, expression) {
                function customComparator(actual, expected) {

                    var isBeforeActivated = expected.before;
                    var isAfterActivated = expected.after;
                    var isLower = expected.lower;
                    var isHigher = expected.higher;
                    var higherLimit;
                    var lowerLimit;
                    var itemDate;
                    var queryDate;

                    if (ng.isObject(expected)) {
                        //exact match
                        if (expected.distinct) {
                            if (!actual || actual.toLowerCase() !== expected.distinct.toLowerCase()) {
                                return false;
                            }

                            return true;
                        }

                        //matchAny
                        if (expected.matchAny) {
                            if (expected.matchAny.all) {
                                return true;
                            }

                            if (!actual) {
                                return false;
                            }

                            for (var i = 0; i < expected.matchAny.items.length; i++) {
                                if (actual.toLowerCase() === expected.matchAny.items[i].toLowerCase()) {
                                    return true;
                                }
                            }

                            return false;
                        }

                        //date range
                        if (expected.before || expected.after) {
                            try {
                                if (isBeforeActivated) {
                                    higherLimit = expected.before;

                                    itemDate = new Date(actual);
                                    queryDate = new Date(higherLimit);

                                    if (itemDate > queryDate) {
                                        return false;
                                    }
                                }

                                if (isAfterActivated) {
                                    lowerLimit = expected.after;


                                    itemDate = new Date(actual);
                                    queryDate = new Date(lowerLimit);

                                    if (itemDate < queryDate) {
                                        return false;
                                    }
                                }

                                return true;
                            } catch (e) {
                                return false;
                            }

                        } else if (isLower || isHigher) {
                            //number range
                            if (isLower) {
                                higherLimit = expected.lower;

                                if (actual > higherLimit) {
                                    return false;
                                }
                            }

                            if (isHigher) {
                                lowerLimit = expected.higher;
                                if (actual < lowerLimit) {
                                    return false;
                                }
                            }

                            return true;
                        }
                        //etc

                        return true;

                    }
                    return standardComparator(actual, expected);
                }

                var output = filterFilter(array, expression, customComparator);
                return output;
            };
        }]);
})(angular);