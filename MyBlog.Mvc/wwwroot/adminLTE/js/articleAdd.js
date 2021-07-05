//textarea ya text editor özelliklerini kazandırdık
$(document).ready(function () {
    //Trumbowyg
    $('#textEditor').trumbowyg({
        btns: [
            ['viewHTML'],
            ['undo', 'redo'], // Only supported in Blink browsers
            ['formatting'],
            ['strong', 'em', 'del'],
            ['superscript', 'subscript'],
            ['link'],
            ['insertImage'],
            ['justifyLeft', 'justifyCenter', 'justifyRight', 'justifyFull'],
            ['unorderedList', 'orderedList'],
            ['horizontalRule'],
            ['removeformat'],
            ['fullscreen'],
            ['foreColor', 'backColor'],
            ['emoji'],
            ['fontfamily'],
            ['fontsize']
        ]
    });
     //Trumbowyg

    //Select2
    $('#categoryList').select2({
        //Select2 için bootstrap 4 ayarı
        theme: "bootstrap4",
        placeholder: "Lütfen bir kategori seçiniz",
        allowClear: true
    });
     //Select2

    //JQuery UI Datepicker
    $(function () {
        $("#datepicker").datepicker({
            dateFormat: "dd-mm-yy",
            altFieldTimeOnly: false,
            altFormat: "yy-mm-dd",
            altTimeFormat: "h:m",
            altField: "#tarih-db",
            monthNames: ["Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran", "Temmuz", "Ağustos", "Eylül", "Ekim", "Kasım", "Aralık"],
            dayNamesMin: ["Pa", "Pt", "Sl", "Ça", "Pe", "Cu", "Ct"],
            firstDay: 1,
            duration: 500,
            showAnim: "drop",
            showOptions: {direction:"down"},
            //Şuan ki tarihten kaç gün öncesi seçilebilsin
            minDate:-3,
            //Şuan ki tarihten kaç gün sonrası seçilebilsin
            maxDate:+3
        });
    });
     //JQuery UI Datepicker
});
