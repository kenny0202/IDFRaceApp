if (!Modernizr.inputtypes.date) {

    $(function () {

        $(".datecontrol").datepicker({
            startDate: Date
        });


    });

}