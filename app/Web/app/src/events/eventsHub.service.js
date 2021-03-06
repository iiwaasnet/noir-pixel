﻿(function() {
    'use strict';

    angular.module('np.events')
        .service('EventsHub', eventsHubService);

    eventsHubService.$inject = ['Events', 'ApplicationLogging'];

    function eventsHubService(Events, ApplicationLogging) {
        var service = this,
            handlers = {};
        service.addListener = addListener;
        service.removeListener = removeListener;
        service.publishEvent = publishEvent;
        service.events = Events;

        function publishEvent(event, data) {
            var eventHandlers = handlers[event] || {};
            Object.keys(eventHandlers).forEach(function (handler) {
                try {
                    eventHandlers[handler](data);
                } catch (e) {
                    ApplicationLogging.error(e);
                } 
            });
        }

        function addListener(event, delegate) {
            if (event === undefined) {
                throw 'event is undefined!';
            }
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