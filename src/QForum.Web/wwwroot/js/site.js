﻿$(function() {
    $('#registerForm').submit(function (event) {
        var $this = $(this);
        var url = '/register';

        $.ajax({
            type: 'POST',
            url: url,
            data: $this.serialize(),
            success: function (data) {
                console.debug(data);
                toastr.success('Vi ses på kontoret.', 'Tack för din anmälan!');
                $this.trigger('reset');
            },
            error(data) {
                console.error(data);
                switch (data.status) {
                    case 400:
                        toastr.warning('Vänligen se över fälten i formuläret.', 'Fel vid inmatning!');
                        break;
                    default:
                        toastr.error('Vänligen försök igen senare.', 'Ett fel har inträffat!');
                }

            }
        });

        event.preventDefault();
    });
    $('#adminForm').submit(function (event) {
        var $this = $(this);
        var url = window.location.pathname + '/event_info';

        console.debug(url, event);

        $.ajax({
            type: 'POST',
            url: url,
            data: $this.serialize(),
            success: function (data) {
                console.debug(data);
                toastr.success('Informationen är nu uppdaterad.', 'Done!');
                $this.trigger('reset');
            },
            error(data) {
                console.error(data);
                switch (data.status) {
                    case 400:
                        toastr.warning('Vänligen se över fälten i formuläret.', 'Fel vid inmatning!');
                        break;
                    default:
                        toastr.error('Vänligen försök igen senare.', 'Ett fel har inträffat!');
                }

            }
        });

        event.preventDefault();
    });
});