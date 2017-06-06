var App = App || {};
(function () {

    var appLocalizationSource = abp.localization.getSource('CourseManager');
    App.localize = function () {
        return appLocalizationSource.apply(this, arguments);
    };

})(App);