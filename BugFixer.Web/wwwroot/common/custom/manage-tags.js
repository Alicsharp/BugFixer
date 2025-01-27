function loadTagsModal(url) {
    $.ajax({
        url: url,
        type: "GET",
        beforeSend: function () {
            StartLoading("#LargeModal");
        },
        success: function (response) {
            EndLoading("#LargeModal");
            $("#LargeModalBody").html(response);
            $("#LargeModalLabel").html("مدیریت تگ ها");
            $("#LargeModal").modal("show");
        },
        error: function (xhr, status, error) {
            EndLoading("#LargeModal");
            swal({
                title: "خطا",
                text: "عملیات با خطا مواجه شد. لطفا مجدد تلاش کنید.",
                icon: "error",
                button: "باشه"
            });
            console.error("AJAX Error: ", status, error); // برای دیباگ بهتر
        }
    });
}

function StartLoading(selector) {
    $(selector).append('<div class="loading-overlay"><div class="spinner"></div>در حال بارگذاری...</div>');
}

function EndLoading(selector) {
    $(selector).find('.loading-overlay').remove();
}