angular.module('npApp').directive('npI18n', [
    'Strings', function(Strings) {
        var dir = {
            restrict: "A",
            link: function(scope, elm, attrs) {
                scope.$on('stringsUpdates', function() {
                    dir.updateText(elm, attrs.npI18n);
                });

                attrs.$observe('npI18n', function (value) {
                    dir.updateText(elm, attrs.npI18n);
                });
            },
            updateText: function(elm, token) {
                var values = token.split('|');
                if (values.length >= 1) {
                    // construct the tag to insert into the element
                    var tag = Strings.getLocalizedString(values[0]);
                    // update the element only if data was returned
                    if ((tag !== null) && (tag !== undefined) && (tag !== '')) {
                        if (values.length > 1) {
                            for (var index = 1; index < values.length; index++) {
                                var target = '{' + (index - 1) + '}';
                                tag = tag.replace(target, values[index]);
                            }
                        }
                        // insert the text into the element
                        elm.text(tag);
                    };
                }
            }
        };

        return dir;
    }
]);