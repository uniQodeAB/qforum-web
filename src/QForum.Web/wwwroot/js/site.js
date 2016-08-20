$(function() {
    $('#registerForm').submit(function (event) {
        var $this = $(this);
        var spinner = $('.spinner');
        var submitBtn = $('#registerForm input[type="submit"]');
        var url = '/register';

        spinner.show();
        submitBtn.prop('disabled', true);

        $.ajax({
            type: 'POST',
            url: url,
            data: $this.serialize(),
            success: function (data) {
                console.debug(data);
                spinner.hide();
                submitBtn.prop('disabled', false);
                toastr.success('Vi ses på kontoret.', 'Tack för din anmälan!');
                $this.trigger('reset');
            },
            error(data) {
                console.error(data);
                spinner.hide();
                submitBtn.prop('disabled', false);
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
        var spinner = $('.spinner');
        var submitBtn = $('#adminForm input[type="submit"]');
        var url = window.location.pathname + '/event_info';

        console.debug(url, event);

        spinner.show();
        submitBtn.prop('disabled', true);

        $.ajax({
            type: 'POST',
            url: url,
            data: $this.serialize(),
            success: function (data) {
                console.debug(data);
                spinner.hide();
                submitBtn.prop('disabled', false);
                toastr.success('Informationen är nu uppdaterad.', 'Done!');
                $this.trigger('reset');
            },
            error(data) {
                console.error(data);
                spinner.hide();
                submitBtn.prop('disabled', false);
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