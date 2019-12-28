/*-----------Login page ---------- */
$('.js-tilt').tilt({
    scale: 1.1
})


$(".wrap-input100 input").on("focus", function () {
    $(this).addClass("focus");
});


$(".wrap-input100 input").on("blur", function () {
    if ($(this).val() == "")
        $(this).removeClass("focus");
});

/*----------- Fim Login page ---------- */


/*-----------layout loginRegistro ---------- */

$('#modalErroLogin').on('shown.bs.modal', function () {
    $('#meuInput').trigger('focus')
})

/*----------- Fim layout loginRegistro ---------- */
