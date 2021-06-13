

export async function ConfirmSweetAlert(title, text, delButtonText, cancelButtonText, icon = 'warning', confirmButtonColor = '#3085d6', cancelButtonColor = '#d33') {
    var res = false;
    await Swal.fire({
        title: title,
        text: text,
        icon: icon,
        showCancelButton: true,
        confirmButtonColor: confirmButtonColor,
        cancelButtonColor: cancelButtonColor,
        confirmButtonText: delButtonText,
        cancelButtonText: cancelButtonText
    }).then((result) => {
        res = result.isConfirmed;
    });
    return res;
}

export async function ShowSweetAlert(title, text, timer, icon) {
    Swal.fire({
        title: title,
        text: text,
        icon: icon,
        timer: timer,
        timerProgressBar: true,
    });
}

export async function ToastSweetAlert(title, text, timer,icon, position = 'top-end') {
    const Toast = Swal.mixin({
        toast: true,
        position: position,
        showConfirmButton: false,
        timer: timer,
        timerProgressBar: true,
        didOpen: (toast) => {
            toast.addEventListener('mouseenter', Swal.stopTimer)
            toast.addEventListener('mouseleave', Swal.resumeTimer)
        }
    });

    Toast.fire({
        icon: icon,
        title: title,
        text: text
    });
}