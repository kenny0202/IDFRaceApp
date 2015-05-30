$(document).ready(function () {
    // hide everything
    $("#classesPerHeat1").hide();
    $("#classesPerHeat2").hide();

    $("#opt1").change(function () {
        $("#classesPerHeat1").show();
        $("#classesPerHeat2").hide();
    });
    $("#opt2").change(function () {
        $("#classesPerHeat1").hide();
        $("#classesPerHeat2").show();
    });

    /* Repechage */
    // hide everything
    $("#qualifierZero").hide();
    $("#qualifierFour").hide();
    $("#qualifierSix").hide();

    $("#repZero").change(function () {
        $("#qualifierZero").show();
        $("#qualifierFour").hide();
        $("#qualifierSix").hide();
    });
    $("#repFour").change(function () {
        $("#qualifierZero").hide();
        $("#qualifierFour").show();
        $("#qualifierSix").hide();
    });
    $("#repSix").change(function () {
        $("#qualifierZero").hide();
        $("#qualifierFour").hide();
        $("#qualifierSix").show();
    });
   





});