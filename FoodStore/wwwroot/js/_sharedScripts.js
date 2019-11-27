/**
 * Herhangi bir sayfa içerisinde çağırılan delete methodu.
 * @param {any} swalTitle Ekranda çıkacak olan confirm mesajının title bilgisi.
 * @param {any} deleteMessage başarılı silinme işleminde gözükecek başarılı mesajı.
 * @param {any} confirmMessage ekranda çıkacak doğrulama mesajı
 * @param {any} confirmButtonText onaylama butonu texti
 * @param {any} cancelButtonText iptal etme butonu texti
 * @param {any} successTitle başarılı silinme durumundaki title bilgisi
 * @param {any} loadingTitle işlem yapılıyorken ki loading title bilgisi
 * @param {any} loadingMessage işlem yapılıyorken lütfen bekleyiniz mesajı.
 * @param {any} data işlem yapılacak data bilgisi (örn. id vb.)
 * @param {any} url hangi endpointe gidilecek.
 * @param {any} methodType method tipi (POST,DELETE vb.)
 * @param {any} returnMethod eğer sayfa yönlendirmesi varsa buraya yönlendirilmeli.opsiyonel
 */
function DeleteDataWithConfirm(swalTitle, deleteMessage, confirmMessage, confirmButtonText, cancelButtonText, successTitle, loadingTitle, loadingMessage, data, url, methodType, returnMethod) {
    Swal.fire({
        title: swalTitle,
        icon: 'question',
        text: confirmMessage,
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: confirmButtonText,
        cancelButtonText: cancelButtonText
    }).then((result) => {
        if (result.value) {
            swal({
                title: loadingTitle,
                text: loadingMessage,
                allowOutsideClick: false,
                allowEscapeKey: false,
                allowEnterKey: false,
                onOpen: () => {
                    swal.showLoading()
                }
            });
            $.ajax({
                type: methodType,
                url: url,
                data: data,
                success: function (result) {
                    if (result.IsSuccess) {
                        Swal.fire(
                            successTitle,
                            deleteMessage,
                            'success'
                        );
                    }
                },
            });
        }
    })
}