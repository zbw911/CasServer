
;
(function($) {
    var settings = {
        inEffect: { opacity: 'show' }, // in effect
        inEffectDuration: 600, 			// in effect duration in miliseconds
        stayTime: 3000, 			// time in miliseconds before the item has to disappear
        text: '', 				// content of the item. Might be a string or a jQuery object. Be aware that any jQuery object which is acting as a message will be deleted when the toast is fading away.
        sticky: false, 			// should the toast item sticky or not?
        type: 'notice', 			// notice, warning, error, success
        position: 'top-right',        // top-left, top-center, top-right, middle-left, middle-center, middle-right ... Position of the toast container holding different toast. Position can be set only once at the very first call, changing the position after the first call does nothing
        closeText: '',                 // text which will be shown as close button, set to '' when you want to introduce an image via css
        close: null,                // callback function when the toastmessage is closed
        single: true                // 在同一时刻只显示一个
    };

    var methods = {
        init: function(options) {
            if (options) {
                $.extend(settings, options);
            }
        },

        showToast: function(options) {
            var localSettings = { };
            $.extend(localSettings, settings, options);

            // declare variables
            var toastWrapAll, toastItemOuter, toastItemInner, toastItemClose, toastItemImage;

            toastWrapAll = (!$('.toast-container').length) ? $('<div></div>').addClass('toast-container').addClass('toast-position-' + localSettings.position).appendTo('body') : $('.toast-container');
            //            toastWrapAll.removeClass('toast-position-' + localSettings.position);
            //            toastWrapAll.addClass('toast-position-' + localSettings.position);
            if (localSettings.single) $(toastWrapAll).empty();
            toastItemOuter = $('<div></div>').addClass('toast-item-wrapper');
            toastItemInner = $('<div></div>').hide().addClass('toast-item toast-type-' + localSettings.type).appendTo(toastWrapAll).html($('<p>').append(localSettings.text)).animate(localSettings.inEffect, localSettings.inEffectDuration).wrap(toastItemOuter);
            toastItemClose = $('<div></div>').addClass('toast-item-close').prependTo(toastItemInner).html(localSettings.closeText).click(function() { $().toastmessage('removeToast', toastItemInner, localSettings); });
            toastItemImage = $('<div></div>').addClass('toast-item-image').addClass('toast-item-image-' + localSettings.type).prependTo(toastItemInner);

            if (navigator.userAgent.match(/MSIE 6/i)) {
                toastWrapAll.css({ top: document.documentElement.scrollTop });
            }

            if (!localSettings.sticky) {
                setTimeout(function() {
                    $().toastmessage('removeToast', toastItemInner, localSettings);
                },
                    localSettings.stayTime);
            }
            return toastItemInner;
        },

        showNoticeToast: function(message) {
            var options = { text: message, type: 'notice' };
            return $().toastmessage('showToast', options);
        },

        showSuccessToast: function(message) {
            var options = { text: message, type: 'success' };
            return $().toastmessage('showToast', options);
        },

        showErrorToast: function(message) {
            var options = { text: message, type: 'error' };
            return $().toastmessage('showToast', options);
        },

        showWarningToast: function(message) {
            var options = { text: message, type: 'warning' };
            return $().toastmessage('showToast', options);
        },

        removeToast: function(obj, options) {
            obj.animate({ opacity: '0' }, 600, function() {
                obj.parent().animate({ height: '0px' }, 300, function() {
                    obj.parent().remove();
                });
            });
            // callback
            if (options && options.close !== null) {
                options.close();
            }
        }
    };

    $.fn.toastmessage = function(method) {

        // Method calling logic
        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on jQuery.toastmessage');
        }
    };

})(jQuery);


//if ($("head").length > 0)
//    ($("head").append("<link href=\"\/Js\/plugin\/toastmessage\/css\/jquery.toastmessage.css\" rel=\"stylesheet\" type=\"text\/css\" \/>");
////document.close();
var autohideSecond = 2;


function _showBase(message, autohide, funclose, type) {

    $(".toast-container").removeAttr("style");
    sticky = false;
    if (autohide == undefined) {
        autohide = autohideSecond;
        sticky = false;
    }

    if (autohide == 0) {

        sticky = true;
    }


    $().toastmessage('showToast', {
        text: message,
        stayTime: autohide * 1000,
        sticky: sticky,
        position: 'middle-center',
        type: type,
        closeText: '',
        close: function() {
            // console.log("toast is closed ...");
            //                    window.alert("");
            if ($.isFunction(funclose)) funclose();
        }
    });
}

function showError(message, funclose, autohide) {
    _showBase(message, autohide, funclose, "error");
}

function showWarning(message, funclose, autohide) {
    _showBase(message, autohide, funclose, "warning");
}

function showSuccess(message, funclose, autohide) {
    _showBase(message, autohide, funclose, 'success');
}

function showInfo(message, funclose, autohide) {
    _showBase(message, autohide, funclose, 'notice');
}


function showPosition(left, top) {

    $(".toast-container")
        .css("top", top + "px")
        .css("left", left + "px")
        .css("margin-left", 0)
        .css("margin-top", 0);
}