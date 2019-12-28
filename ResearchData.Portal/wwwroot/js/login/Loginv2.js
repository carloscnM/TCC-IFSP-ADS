function changeTab(ref){
  try {
  if(ref.getAttribute("data-tab") == "login"){
    document.getElementById("form-body").classList.remove('active');
    ref.parentNode.classList.remove('signup');
  } else {
    document.getElementById("form-body").classList.add('active');
    ref.parentNode.classList.add('signup');
  }
  } catch(msg){
    console.log(msg);
  }
}

(function ($) {
    "use strict";


    /*==================================================================
    [ Focus input ]*/
    $('.input100').each(function () {
        $(this).on('blur', function () {
            if ($(this).val().trim() != "") {
                $(this).addClass('has-val');
            }
            else {
                $(this).removeClass('has-val');
            }
        })
    })
})