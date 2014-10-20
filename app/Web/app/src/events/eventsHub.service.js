(function() {
    'use strict';

    angular.module('np.events')
        .service('EventsHub', eventsHubService);

    function eventsHubService() {
        var service = this,
            handlers = {};
        service.addListener = addListener;
        service.removeListener = removeListener;
        service.publishEvent = publishEvent;

        function publishEvent(event, data) {
            var eventHandlers = handlers[event] || {};
            Object.keys(eventHandlers).forEach(function(handler) {
                eventHandlers[handler](data);
            });
        }

        function addListener(event, delegate) {
            var eventHandlers = handlers[event] || [];
            
            if (!~eventHandlers.indexOf(delegate)) {
                eventHandlers.push(delegate);
            }
            
            handlers[event] = eventHandlers;
        }

        function removeListener(event, delegate) {
            var eventHandlers = handlers[event] || {};
            var index = eventHandlers.indexOf(delegate);
            if (~index) {
                eventHandlers.splice(index, 1);
            }
        }
    }
})();