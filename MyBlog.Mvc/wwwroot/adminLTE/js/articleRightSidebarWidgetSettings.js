//textarea ya text editor özelliklerini kazandırdık
$(document).ready(function () {

    //Select2
    $('#categoryList').select2({
        //Select2 için bootstrap 4 ayarı
        theme: "bootstrap4",
        placeholder: "Lütfen bir kategori seçiniz",
        allowClear: true
    });
    //Select2

    //Select2
    $('#filterByList').select2({
        //Select2 için bootstrap 4 ayarı
        theme: "bootstrap4",
        placeholder: "Lütfen bir filtre türü seçiniz",
        allowClear: true
    });
    //Select2

    //Select2
    $('#orderByList').select2({
        //Select2 için bootstrap 4 ayarı
        theme: "bootstrap4",
        placeholder: "Lütfen bir sıralama türü seçiniz",
        allowClear: true
    });
    //Select2

    //Select2
    $('#isAscendingList').select2({
        //Select2 için bootstrap 4 ayarı
        theme: "bootstrap4",
        placeholder: "Lütfen bir sıralama türü seçiniz",
        allowClear: true
    });
    //Select2


    //JQuery UI Datepicker
    $(function () {
        $("#startDatePicker").datepicker({
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
            showOptions: { direction: "down" },
            //Şuan ki tarihten kaç gün öncesi seçilebilsin
            minDate: -3,
            //Şuan ki tarihten kaç gün sonrası seçilebilsin
            maxDate: +3,
            onSelect: function (selectedDate) { //endDate başlangıç zamanının startDate göre ayarlanabilmesi için böyle bir kullanım gerçekleştirdik
                $("#endDatePicker").datepicker("options", "minDate", selectedDate || getTodaysDate());
            }
        });

        $("#endDatePicker").datepicker({
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
            showOptions: { direction: "down" },
            //Şuan ki tarihten kaç gün öncesi seçilebilsin
            minDate: 0,
            //Şuan ki tarihten kaç gün sonrası seçilebilsin
            maxDate: +3
        });
    });
    //JQuery UI Datepicker
});
