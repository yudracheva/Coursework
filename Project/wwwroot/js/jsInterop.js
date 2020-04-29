

window.workWithLocalStorage = {
    setItem: function (key, value) {
        window.localStorage.setItem(key, value);
    },
    getItem: function (key) {
        return window.localStorage.getItem(key);
    },
    removeItem: function (key) {
        window.localStorage.removeItem(key);
    }
};

window.workWithMessage = {
    showError: function (message) {
        toastr.options = {
            "closeButton": true,
            "debug": false,
            "newestOnTop": true,
            "progressBar": true,
            "positionClass": "toast-bottom-right",
            "preventDuplicates": false,
            "onclick": null,
            "showDuration": "300",
            "hideDuration": "1000",
            "timeOut": "5000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        };
        toastr["error"](message, "Ошибка");
    },
    showSuccess: function (message) {
        toastr.options = {
            "closeButton": true,
            "debug": false,
            "newestOnTop": true,
            "progressBar": true,
            "positionClass": "toast-bottom-right",
            "preventDuplicates": false,
            "onclick": null,
            "showDuration": "300",
            "hideDuration": "1000",
            "timeOut": "50000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        };
        toastr["success"](message, "Успешно");
    },
    showInfo: function (message) {
        toastr.options = {
            "closeButton": true,
            "debug": false,
            "newestOnTop": true,
            "progressBar": true,
            "positionClass": "toast-bottom-right",
            "preventDuplicates": false,
            "onclick": null,
            "showDuration": "300",
            "hideDuration": "1000",
            "timeOut": "5000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        };
        toastr["info"](message, "Информация");
    },
    showWarning: function (message) {
        toastr.options = {
            "closeButton": true,
            "debug": false,
            "newestOnTop": true,
            "progressBar": true,
            "positionClass": "toast-bottom-right",
            "preventDuplicates": false,
            "onclick": null,
            "showDuration": "300",
            "hideDuration": "1000",
            "timeOut": "5000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        };
        toastr["warning"](message, "Предупреждение");
    }
};

window.fileWork = {
    save: function (base64, filename, type) {
        var linkSource = 'data:' + type + `;base64,${base64}`;
        var downloadLink = document.createElement("a");
        var fileName = filename;
        if (type === null) {
            fileName = filename;
        } else {
            fileName = filename + type;
        }
        downloadLink.href = linkSource;
        downloadLink.download = fileName;
        downloadLink.click();
    },
};

window.PrintDocuments = {
    printText: function (textBody) {
        var prtCSS = '<link href="/css/bootstrap/bootstrap.min.css" rel="stylesheet" />';
        var WinPrint = window.open('', '', 'left=0, top=0, width=800, height=900, toolbar=0, scrollbars=0, status=0');
        WinPrint.document.write('<!DOCTYPE html><html><head>');
        WinPrint.document.write(prtCSS);
        WinPrint.document.write('</head><body>');
        WinPrint.document.write('</head><body>');
        WinPrint.document.write(textBody);
        WinPrint.document.write('</body>');
        WinPrint.document.close();
        WinPrint.focus();
        WinPrint.onload = function () {
            WinPrint.print();
            WinPrint.document.close();
        };
    },
};


window.DataResultHelper = {
    scroll: function () {
        var elemDes = $(document.getElementById("result-search")).offset().top;
        $('html').animate({ scrollTop: elemDes }, 1100);
    },
};

