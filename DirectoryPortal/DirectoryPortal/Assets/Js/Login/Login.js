function Login() {

    var email = jQuery('#inputEmail').val();
    var password = jQuery('#inputPassword').val();
    var params = "?email=" + email + "&password=" + password;

    $.ajax({
        type: "POST",
        url: "http://localhost:50922/Login/Login" + params,
        cache: false,
        success: function (data) {
            if (data !== null && data !== undefined && data !== "undefined" && data !== "") {

                window.location = "http://localhost:50922/" + data;
            } else {

                alert("Giriş işleminiz sırasında bir hata oluştu!");
            }
        },
        error: function (xhr, txtStatus, errorThrown) {
            alert("Hata Kodu:" + xhr.status + " " + txtStatus + "\n" + errorThrown);
        }
    });
}

function Register() {

    var name = jQuery('#inputNameRegister').val();
    var email = jQuery('#inputEmailRegister').val();
    var phone = jQuery('#inputPhoneRegister').val();
    var password = jQuery('#inputPasswordRegister').val();
    var passwordAgain = jQuery('#inputPasswordAgainRegister').val();
    if (password !== passwordAgain) {
        alert("Giriş işleminiz sırasında bir hata oluştu!");
    } else {

        var params = "?name=" + name + "&email=" + email + "&password=" + password + "&phone=" + phone;

        $.ajax({
            type: "POST",
            url: "http://localhost:50894/api/Authanticate/Register" + params,
            cache: false,
            success: function (data) {
                alert("İşleminiz başarıyla tamamlandı");
            },
            error: function (xhr, txtStatus, errorThrown) {
                alert("Hata Kodu:" + xhr.status + " " + txtStatus + "\n" + errorThrown);
            }
        });
    }
}