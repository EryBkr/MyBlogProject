$(document).ready(function () {
    $(function () {
        $(document).on("click", "#btnSave", function (event) {
            //Butonun Default işlemini engelliyoruz
            event.preventDefault();

            const form = $("#form-comment-add");
            const actionUrl = form.attr("action");

            const dataToSend = form.serialize();

            $.post(actionUrl, dataToSend).done(function (data) {
                const commentAddAjaxModel = jQuery.parseJSON(data);
                console.log(commentAddAjaxModel);

                //Gelen View Model üzerindeki Card kısmını seçiyoruz
                const newFormBody = $(".form-card", commentAddAjaxModel.CommentAddPartial);

                //Sayfa üzerindeki Card kısmını seçiyoruz
                const cardBody = $(".form-card");

                //Gelen partial ile sayfada ki partial ı değiştirdik
                cardBody.replaceWith(newFormBody);

                //IsValid True mu
                const isValid = newFormBody.find("[name='IsValid']").val() === "True";

                if (isValid) {
                    const newSingleComment =
           ` <div class="media mb-4">
                <img class="d-flex mr-3 rounded-circle" style="width:30px;" src="/img/userImages/defaultUser.png" alt="">
                <div class="media-body">
                    <h5 class="mt-0">${commentAddAjaxModel.CommentDto.Comment.CreatedByName}</h5>
                    ${commentAddAjaxModel.CommentDto.Comment.Text}
                </div>
            </div>`;

                    //JQuery fonksiyonları kullanabilmek için Jquery objesi haline getirdik
                    const newSingleCommentObject = $(newSingleComment);
                    newSingleCommentObject.hide();

                    //Detail sayfamızda ki yorumların gösterileceği Div i seçtik ve oraya efektli bir şekilde ekleyeceğiz
                    $("#comments").append(newSingleCommentObject);
                    newSingleCommentObject.fadeIn(3000);
                    toastr.success(`Sayın ${commentAddAjaxModel.CommentDto.Comment.CreatedByName} yorumunuz başarıyla eklenmiştir.Bir örneği sizler için karşınıza gelecektir fakat yorumuz onaylandıktan sonra görünür olacaktır.`);

                    //Flood olmaması için geçici bir süre boyunca butonu disabled yapıyoruz
                    $("#btnSave").prop("disabled", true);
                    //15 sn sonra buton aktif olacaktır
                    setTimeout(function () {
                        $("#btnSave").prop("disabled", false);
                    },15000);
                }
                else {
                    let summaryText = "";
                    $('#validation-summary > ul > li').each(function () {
                        let text = $(this).text();
                        summaryText += `*${text}\n`;
                    });
                    toastr.warning(summaryText);
                }
            }).fail(function (err) {
                console.error(err);
            });
        });
    });
});