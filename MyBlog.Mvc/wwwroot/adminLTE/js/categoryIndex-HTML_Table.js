$(document).ready(function () {
    $('#categoriesTable').DataTable({
        dom:
            "<'row'<'col-sm-3'l><'col-sm-6 text-center'B><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        buttons: [
            {
                text: "Ekle",
                attr: {
                    id: "btnAdd"
                },
                className: "btn btn-sm btn-success",
                action: function (e, dt, node, config) {
                }
            },
            {
                text: "Yenile",
                className: "btn btn-sm btn-warning",
                action: function (e, dt, node, config) {
                    $.ajax({
                        type: "GET",
                        url: '/Admin/Category/GetAllCategories',
                        contentType: "application/json",
                        beforeSend: function () {
                            $("#categoriesTable").hide();
                            $(".spinner-border").show();
                        },
                        success: function (data) {
                            const categoryListDto = jQuery.parseJSON(data);
                            if (categoryListDto.ResultStatus === 0) {
                                let tableBody = "";
                                $.each(categoryListDto.Categories.$values, function (index, category) {
                                    tableBody += `
                            <tr name="${category.Id}">
                                 <td>${category.Id}</td>
                                 <td>${category.Name}</td>
                                 <td>${category.Description}</td>
                                 <td>${category.IsActive ? "Evet" : "Hayır"}</td>
                                 <td>${category.IsDeleted ? "Evet" : "Hayır"}</td>
                                 <td>${category.Note}</td>
                                 <td>${convertToShortDate(category.CreatedDate)}</td>
                                 <td>${category.CreatedByName}</td>
                                 <td>${convertToShortDate(category.ModifiedDate)}</td>
                                 <td>${category.ModifiedByName}</td>
                                 <td>
                                    <button class="btn btn-sm btn-primary btn-update" data-id="${category.Id}"><span class="fas fa-edit"></span></button>
                                    <button class="btn btn-sm btn-danger btn-delete" data-id="${category.Id}"><span class="fas fa-minus-circle"></span></button>
                                </td>
                            </tr>`;
                                });
                                $("#categoriesTable > tbody").replaceWith(tableBody);
                                $(".spinner-border").hide();
                                $("#categoriesTable").fadeIn(1000);
                            }
                            else {
                                toastr.error(`${categoryListDto.Message}`, "İşlem Başarısız!");
                            }
                        },
                        error: function (err) {
                            $(".spinner-border").hide();
                            $("#categoriesTable").fadeIn(1000);
                            toastr.error(`${err.responseText}`, "Hata!");
                            $(".spinner-border").hide();
                        }
                    });
                }
            }
        ],
        language: {
            "emptyTable": "Tabloda herhangi bir veri mevcut değil",
            "info": "_TOTAL_ kayıttan _START_ - _END_ arasındaki kayıtlar gösteriliyor",
            "infoEmpty": "Kayıt yok",
            "infoFiltered": "(_MAX_ kayıt içerisinden bulunan)",
            "infoThousands": ".",
            "lengthMenu": "Sayfada _MENU_ kayıt göster",
            "loadingRecords": "Yükleniyor...",
            "processing": "İşleniyor...",
            "search": "Ara:",
            "zeroRecords": "Eşleşen kayıt bulunamadı",
            "paginate": {
                "first": "İlk",
                "last": "Son",
                "next": "Sonraki",
                "previous": "Önceki"
            },
            "aria": {
                "sortAscending": ": artan sütun sıralamasını aktifleştir",
                "sortDescending": ": azalan sütun sıralamasını aktifleştir"
            },
            "select": {
                "rows": {
                    "_": "%d kayıt seçildi",
                    "1": "1 kayıt seçildi",
                    "0": "-"
                },
                "0": "-",
                "1": "%d satır seçildi",
                "2": "-",
                "_": "%d satır seçildi",
                "cells": {
                    "1": "1 hücre seçildi",
                    "_": "%d hücre seçildi"
                },
                "columns": {
                    "1": "1 sütun seçildi",
                    "_": "%d sütun seçildi"
                }
            },
            "autoFill": {
                "cancel": "İptal",
                "fillHorizontal": "Hücreleri yatay olarak doldur",
                "fillVertical": "Hücreleri dikey olarak doldur",
                "info": "-",
                "fill": "Bütün hücreleri <i>%d<\/i> ile doldur"
            },
            "buttons": {
                "collection": "Koleksiyon <span class=\"ui-button-icon-primary ui-icon ui-icon-triangle-1-s\"><\/span>",
                "colvis": "Sütun görünürlüğü",
                "colvisRestore": "Görünürlüğü eski haline getir",
                "copySuccess": {
                    "1": "1 satır panoya kopyalandı",
                    "_": "%ds satır panoya kopyalandı"
                },
                "copyTitle": "Panoya kopyala",
                "csv": "CSV",
                "excel": "Excel",
                "pageLength": {
                    "-1": "Bütün satırları göster",
                    "1": "-",
                    "_": "%d satır göster"
                },
                "pdf": "PDF",
                "print": "Yazdır",
                "copy": "Kopyala",
                "copyKeys": "Tablodaki veriyi kopyalamak için CTRL veya u2318 + C tuşlarına basınız. İptal etmek için bu mesaja tıklayın veya escape tuşuna basın."
            },
            "infoPostFix": "-",
            "searchBuilder": {
                "add": "Koşul Ekle",
                "button": {
                    "0": "Arama Oluşturucu",
                    "_": "Arama Oluşturucu (%d)"
                },
                "condition": "Koşul",
                "conditions": {
                    "date": {
                        "after": "Sonra",
                        "before": "Önce",
                        "between": "Arasında",
                        "empty": "Boş",
                        "equals": "Eşittir",
                        "not": "Değildir",
                        "notBetween": "Dışında",
                        "notEmpty": "Dolu"
                    },
                    "number": {
                        "between": "Arasında",
                        "empty": "Boş",
                        "equals": "Eşittir",
                        "gt": "Büyüktür",
                        "gte": "Büyük eşittir",
                        "lt": "Küçüktür",
                        "lte": "Küçük eşittir",
                        "not": "Değildir",
                        "notBetween": "Dışında",
                        "notEmpty": "Dolu"
                    },
                    "string": {
                        "contains": "İçerir",
                        "empty": "Boş",
                        "endsWith": "İle biter",
                        "equals": "Eşittir",
                        "not": "Değildir",
                        "notEmpty": "Dolu",
                        "startsWith": "İle başlar"
                    },
                    "array": {
                        "contains": "İçerir",
                        "empty": "Boş",
                        "equals": "EşiUrltir",
                        "not": "Değildir",
                        "notEmpty": "Dolu",
                        "without": "Hariç"
                    }
                },
                "data": "Veri",
                "deleteTitle": "Filtreleme kuralını silin",
                "leftTitle": "Kriteri dışarı çıkart",
                "logicAnd": "ve",
                "logicOr": "veya",
                "rightTitle": "Kriteri içeri al",
                "title": {
                    "0": "Arama Oluşturucu",
                    "_": "Arama Oluşturucu (%d)"
                },
                "value": "Değer",
                "clearAll": "Filtreleri Temizle"
            },
            "searchPanes": {
                "clearMessage": "Hepsini Temizle",
                "collapse": {
                    "0": "Arama Bölmesi",
                    "_": "Arama Bölmesi (%d)"
                },
                "count": "{total}",
                "countFiltered": "{shown}\/{total}",
                "emptyPanes": "Arama Bölmesi yok",
                "loadMessage": "Arama Bölmeleri yükleniyor ...",
                "title": "Etkin filtreler - %d"
            },
            "searchPlaceholder": "Ara",
            "thousands": ".",
            "datetime": {
                "amPm": [
                    "öö",
                    "ös"
                ],
                "hours": "Saat",
                "minutes": "Dakika",
                "next": "Sonraki",
                "previous": "Önceki",
                "seconds": "Saniye",
                "unknown": "Bilinmeyen"
            },
            "decimal": ",",
            "editor": {
                "close": "Kapat",
                "create": {
                    "button": "Yeni",
                    "submit": "Kaydet",
                    "title": "Yeni kayıt oluştur"
                },
                "edit": {
                    "button": "Düzenle",
                    "submit": "Güncelle",
                    "title": "Kaydı düzenle"
                },
                "error": {
                    "system": "Bir sistem hatası oluştu (Ayrıntılı bilgi)"
                },
                "multi": {
                    "info": "Seçili kayıtlar bu alanda farklı değerler içeriyor. Seçili kayıtların hepsinde bu alana aynı değeri atamak için buraya tıklayın; aksi halde her kayıt bu alanda kendi değerini koruyacak.",
                    "noMulti": "Bu alan bir grup olarak değil ancak tekil olarak düzenlenebilir.",
                    "restore": "Değişiklikleri geri al",
                    "title": "Çoklu değer"
                },
                "remove": {
                    "button": "Sil",
                    "confirm": {
                        "_": "%d adet kaydı silmek istediğinize emin misiniz?",
                        "1": "Bu kaydı silmek istediğinizden emin misiniz?"
                    },
                    "submit": "Sil",
                    "title": "Kayıtları sil"
                }
            }
        }
    });

    /*DataTable Ends*/

    /*Ajax Get _CategoryAddPartial*/
    $(function () {
        const placeHolderDiv = $("#modelPlaceHolder");
        const url = "/Admin/Category/Add";

        $("#btnAdd").click(function () {
            $.get(url).done(function (data) {
                placeHolderDiv.html(data);
                placeHolderDiv.find(".modal").modal("show");
            });
        });

        /*Ajax POST*/
        placeHolderDiv.on("click", "#btnSave", function (event) {
            /*Submit işlemini engelliyoruz*/
            event.preventDefault();

            const form = $("#form-category-add");
            const actionUrl = form.attr("action");
            const dataToSend = form.serialize();

            $.post(actionUrl, dataToSend).done(function (data) {

                const categoryAddAjaxModel = jQuery.parseJSON(data);
                const newFormBody = $(".modal-body", categoryAddAjaxModel.CategoryAddPartial);
                placeHolderDiv.find(".modal-body").replaceWith(newFormBody);
                const isValid = newFormBody.find("[name='IsValid']").val() === "True";

                if (isValid) {
                    placeHolderDiv.find(".modal").modal("hide");
                    const newTableRow =
                        `<tr name="${categoryAddAjaxModel.CategoryDto.Category.Id}">
                                <td>${categoryAddAjaxModel.CategoryDto.Category.Id}</td>
                                <td>${categoryAddAjaxModel.CategoryDto.Category.Name}</td>
                                <td>${categoryAddAjaxModel.CategoryDto.Category.Description}</td>
                                <td>${categoryAddAjaxModel.CategoryDto.Category.IsActive ? "Evet" : "Hayır"}</td>
                                <td>${categoryAddAjaxModel.CategoryDto.Category.IsDeleted ? "Evet" : "Hayır"}</td>
                                <td>${categoryAddAjaxModel.CategoryDto.Category.Note}</td>
                                <td>${convertToShortDate(categoryAddAjaxModel.CategoryDto.Category.CreatedDate)}</td>
                                <td>${categoryAddAjaxModel.CategoryDto.Category.CreatedByName}</td>
                                <td>${convertToShortDate(categoryAddAjaxModel.CategoryDto.Category.ModifiedDate)}</td>
                                <td>${categoryAddAjaxModel.CategoryDto.Category.ModifiedByName}</td>
                                <td>
                                    <button class="btn btn-sm btn-primary btn-update" data-id="${categoryAddAjaxModel.CategoryDto.Category.Id}"><span class="fas fa-edit"></span></button>
                                    <button class="btn btn-sm btn-danger btn-delete" data-id="${categoryAddAjaxModel.CategoryDto.Category.Id}"><span class="fas fa-minus-circle"></span></button>
                                </td>
                            </tr>`;

                    const newTableRowObject = $(newTableRow);
                    newTableRowObject.hide();
                    $("#categoriesTable").append(newTableRowObject);
                    newTableRowObject.fadeIn(1500);
                    toastr.success(`${categoryAddAjaxModel.CategoryDto.Message}`, "Başarılı İşlem!");
                }
                else {
                    let summaryText = "";
                    $("#validation-summary > ul > li").each(function () {
                        let text = $(this).text();
                        summaryText = `*${text}\n`
                    });
                    toastr.warning(summaryText);
                }
            });
        });

    });

    /*Ajax POST DELETE*/
    $(document).on("click", ".btn-delete", function (event) {
        /*Submit işlemini engelliyoruz*/
        event.preventDefault();

        const id = $(this).attr("data-id");
        const tableRow = $(`[name="${id}"]`);
        const categoryName = tableRow.find("td:eq(1)").text();

        Swal.fire({
            title: 'Silmek istediğinize emin misiniz?',
            text: `${categoryName} adlı kategoriyi silmek istediğinize emin misiniz?`,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Evet, silmek istiyorum!',
            cancelButtonText: "Hayır,silmek istemiyorum!"
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    type: "POST",
                    data: {
                        categoryId: id
                    },
                    url: '/Admin/Category/Delete',
                    success: function (deletedData) {

                        const categoryDto = jQuery.parseJSON(deletedData);
                        if (categoryDto.ResultStatus === 0) {
                            tableRow.fadeOut(1000);
                            Swal.fire(
                                'Silindi!',
                                `${categoryDto.Category.Name} adlı kategori başarıyla silinmiştir`,
                                'success'
                            );


                        }
                        else {
                            Swal.fire({
                                icon: 'error',
                                title: 'Başarısız İşlem!..',
                                text: `${categoryDto.Message}`
                            })
                        }
                    },
                    error: function (err) {
                        toastr.error(`${err.responseText}`, "Hata");
                    }
                });
            }
        })
    });

    /*Ajax GET UPDATE Partial*/
    $(function () {
        const url = "/Admin/Category/Update";
        const placeHolderDiv = $("#modelPlaceHolder");
        $(document).on("click", ".btn-update", function (event) {
            event.preventDefault();
            const id = $(this).attr("data-id");
            //Get işlemiyle beraber parametre gönderiyoruz ve parametre isminin birebir aynı olması bizim için önemli
            $.get(url, { categoryId: id })
                .done(function (data) {
                    placeHolderDiv.html(data);
                    placeHolderDiv.find(".modal").modal("show");
                }).fail(function () {
                    toastr.error("Bir Hata Oluştu");
                });
        });

        /*Ajax POST UPDATE*/
        placeHolderDiv.on("click", "#btnUpdate", function () {
            event.preventDefault();
            const form = $("#form-category-update");
            const actionUrl = form.attr("action");
            const dataToSend = form.serialize();

            $.post(actionUrl, dataToSend)
                .done(function (data) {
                    const categoryUpdateAjaxModel = jQuery.parseJSON(data);

                    /*Bize dönen view partial ından gerekli kısmı okuyoruz*/
                    const newFormBody = $(".modal-body", categoryUpdateAjaxModel.CategoryUpdatePartial);

                    /*Açılan modal body ile yeni gelen modal body i yer değiştiriyoruz*/
                    placeHolderDiv.find(".modal-body").replaceWith(newFormBody);

                    const isValid = newFormBody.find("[name='IsValid']").val() === "True";

                    if (isValid) {
                        placeHolderDiv.find(".modal").modal("hide");

                        const newTableRow =
                            `<tr name="${categoryUpdateAjaxModel.CategoryDto.Category.Id}">
                                <td>${categoryUpdateAjaxModel.CategoryDto.Category.Id}</td>
                                <td>${categoryUpdateAjaxModel.CategoryDto.Category.Name}</td>
                                <td>${categoryUpdateAjaxModel.CategoryDto.Category.Description}</td>
                                <td>${categoryUpdateAjaxModel.CategoryDto.Category.IsActive ? "Evet" : "Hayır"}</td>
                                 <td>${categoryUpdateAjaxModel.CategoryDto.Category.IsDeleted ? "Evet" : "Hayır"}</td>
                                <td>${categoryUpdateAjaxModel.CategoryDto.Category.Note}</td>
                                <td>${convertToShortDate(categoryUpdateAjaxModel.CategoryDto.Category.CreatedDate)}</td>
                                <td>${categoryUpdateAjaxModel.CategoryDto.Category.CreatedByName}</td>
                                <td>${convertToShortDate(categoryUpdateAjaxModel.CategoryDto.Category.ModifiedDate)}</td>
                                <td>${categoryUpdateAjaxModel.CategoryDto.Category.ModifiedByName}</td>
                                <td>
                                    <button class="btn btn-sm btn-primary btn-update" data-id="${categoryUpdateAjaxModel.CategoryDto.Category.Id}"><span class="fas fa-edit"></span></button>
                                    <button class="btn btn-sm btn-danger btn-delete" data-id="${categoryUpdateAjaxModel.CategoryDto.Category.Id}"><span class="fas fa-minus-circle"></span></button>
                                </td>
                            </tr>`;

                        //Jquery fonksiyonları kullanabilmek için böyle bir işlem yaptık
                        const newTableObject = $(newTableRow);

                        //Düzenlenecek olan kategori satırına ulaşıyoruz ki düzenleme sonrası değişiklikler bu satıra yansısın
                        const categoryTableRow = $(`[name="${categoryUpdateAjaxModel.CategoryDto.Category.Id}"]`);
                        newTableObject.hide();
                        categoryTableRow.replaceWith(newTableObject);
                        newTableObject.fadeIn(1200);

                        toastr.success(`${categoryUpdateAjaxModel.CategoryDto.Message}`, "Başarılı İşlem");
                    }
                    else {
                        let summaryText = "";
                        $("#validation-summary > ul > li").each(function () {
                            let text = $(this).text();
                            summaryText += `*${text}\n`
                        });
                        toastr.warning(summaryText);
                    }
                })
                .fail(function (response) {
                    console.log(response);
                });
        });

    });




});