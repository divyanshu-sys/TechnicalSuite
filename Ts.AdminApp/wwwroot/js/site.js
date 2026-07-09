/*------------------------------------------------------------------
* Bootstrap Simple Admin Template
* Version: 3.0
* Author: Alexis Luna
* Website: https://github.com/alexis-luna/bootstrap-simple-admin-template
-------------------------------------------------------------------*/

$(function () {
    'use strict';

    // Add active state to sidbar nav links
    let path = window.location.href; // because the 'href' property of the DOM element is the absolute path
    let splitPath = path.split('/');
    $("#sidebar ul li a").each(function () {
        let currhref = this.href;
        let splitCurrhref = currhref.split('/');
        if (this.href === path || (splitPath.length >= 5 && splitCurrhref.length >= 5 && splitPath[4] == splitCurrhref[4])) {
            $(this).addClass("active");
            if (splitPath[3] != '')
                $('a[href="/#' + splitPath[3] + '"]').addClass("main-active");
            return false;
        }
    });

    // Toggle sidebar on Menu button click
    $('#sidebarCollapse').on('click', function () {
        $('#sidebar').toggleClass('active');
        $('#body').toggleClass('active');
    });

    //// Auto-hide sidebar on window resize if window size is small
    //$(window).on('resize', function () {
    //    if ($(window).width() <= 768) {
    //        $('#sidebar').addClass('active');
    //    }
    //});
});

let spinnerButton = ' <div class="spinner-border spinner-border-sm text-light" role="status"><span class="visually-hidden">Loading...</span></div>';
function showBtnLoader(btnId) {
    $(btnId).append(spinnerButton).attr('disabled', true);
}

function hideBtnLoader(btnId) {
    $(btnId).attr('disabled', false).find('div.spinner-border.spinner-border-sm').remove();
}

function reCaptcha(submitBtnId, googleCaptchaSiteKey, actionName, recaptchaId, event) {
    let form = $(submitBtnId).closest("form");
    //event.preventDefault();

    if (!navigator.onLine) {
        failMsg('There is a network problem.');
    }
    else if ($(form).valid()) {
        showBtnLoader(submitBtnId);
        grecaptcha.enterprise.ready(async () => {
            try {
                const token = await grecaptcha.enterprise.execute(googleCaptchaSiteKey, { action: actionName });
                $(recaptchaId).val(token);
                $(form).submit();
            }
            catch (error) {
                failMsg("An error occurred while verifying the reCAPTCHA. Please refresh page and try again.");
                hideBtnLoader(submitBtnId);
                event.preventDefault();
            }
        });
    }
}

function successMsg(msg) {
    $('#divStatusAppend').append(`<div class="position-fixed m-3 alert alert-success alert-dismissible fade show myalert" style="z-index:6; top:50px;">
                                    <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
                                    <strong>Info!</strong> ${msg}
                                    <script>
                                    setTimeout(function () { $(".myalert").alert('close'); }, 25000);
                                    </script>
                                </div>`
    );
}

function failMsg(msg) {
    $('#divStatusAppend').append(`<div class="position-fixed m-3 alert alert-danger alert-dismissible fade show myalert" style="z-index:6; top:50px;">
                                    <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
                                    <strong>Info!</strong> ${msg}
                                    <script>
                                    setTimeout(function () { $(".myalert").alert('close'); }, 35000);
                                    </script>
                                </div>`
    );
}

function sessionNotification(expireTimeMilli) {
    var expireInMilliSec = 120000;
    if (expireTimeMilli > 0) {
        let startDisplayingAt;
        let showExpireinSec;
        if (expireTimeMilli > expireInMilliSec) {
            startDisplayingAt = expireTimeMilli - expireInMilliSec;
            showExpireinSec = Math.floor(expireInMilliSec / 1000)
        }
        else {
            startDisplayingAt = 0;
            showExpireinSec = Math.floor(expireTimeMilli / 1000)
        }

        setTimeout(() => {
            $('#divStatusAppend').append(`<div class="position-fixed m-3 alert alert-warning alert-dismissible fade show" style="z-index:6; bottom:50px; right:0;">
                                        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
                                        <strong>Warning!</strong> <span id="expireContent">Your session is about to expire in <span id="expireCountDown">${showExpireinSec}</span><br /> Please save you work.</span>
                                    </div>`
            );

            setInterval(function () {
                if (showExpireinSec > 0)
                    $('#expireCountDown').html(showExpireinSec--);
                else {
                    $('#expireContent').html('Your session has expired.');

                    $.post('/logout',
                        null,
                        function (data, status, jqXHR) {
                            location.reload();
                            return;
                        });
                }
            }, 1000);
        }, startDisplayingAt);
    }
    else {
        $.post('/logout',
            null,
            function (data, status, jqXHR) {
                location.reload();
                return;
            });
    }
}

function showConfirmation(modalButtonText, modalHead, modalBody, formAction) {
    $('#confirmButton').text(modalButtonText);
    $('#confirmModalLabel').text(modalHead);
    $('#confirmBody').text(modalBody);
    $('#confirmForm').attr('action', formAction);
    $('#confirmModal').modal('show');
}

function hideConfirmation() {
    $('#confirmButton').text('');
    $('#confirmModalLabel').text('');
    $('#confirmBody').text('');
    $('#confirmForm').attr('action', '');
    $('#confirmModal').modal('hide');
}

$(document).on('click', '.submitLoader', function (event) {
    //event.preventDefault();
    showFormSubmitLoader(this, event);
});

function showFormSubmitLoader(btnProperty) {
    let form = $(btnProperty).closest("form");
    if (!navigator.onLine) {
        failMsg('There is a network problem.');
    }
    else if ($(form).valid()) {
        showBtnLoader(btnProperty)
        $(form).submit();
    }
}

function showLoader(spinnerNumber) {
    $('body').prepend(`<div class="spinner-wrap" id="spinner-wrap${spinnerNumber}"><div class="spinner"></div></div>`);
}

function hideLoader(spinnerNumber) {
    $('#spinner-wrap' + spinnerNumber).remove();
}

function handleErrorApiResponse(xhr) {
    if (!navigator.onLine) {
        failMsg('There is a network problem.');
    }
    else if (xhr.status == 401) {
        location.reload();
        return;
    }
    else if (xhr.status == 403) {
        failMsg('You are unauthorised. Please try relogin.');
    }
    else if (xhr.status == 400) {
        failMsg(xhr.responseText);
    }
    else if (xhr.status == 500) {
        failMsg('Internal error.');
    }
    else {
        failMsg(xhr.responseText);
    }
}

function ServerRequest(url, appendId, selectedValueId, spinnerNumber, data, callBack, addEmptyOption = true, multiSelect = false) {
    this.url = url;
    this.appendId = appendId;
    this.selectedValueId = selectedValueId;
    this.spinnerNumber = spinnerNumber;
    this.data = data;
    this.callBack = callBack;
    this.addEmptyOption = addEmptyOption;
    this.multiSelect = multiSelect;
}

ServerRequest.prototype.loadDropDown = function () {
    let appendId = this.appendId;
    let selectedValueId = this.selectedValueId;
    let spinnerNumber = this.spinnerNumber;
    let url = this.url;
    let callBack = this.callBack;
    let addEmptyOption = this.addEmptyOption;
    let multiSelect = this.multiSelect;

    $(appendId).empty();

    $.ajax({
        beforeSend: function (xhr) {
            showLoader(spinnerNumber);
        },
        type: "GET",
        url: url,
        dataType: "json",
        success: function (data) {
            if (addEmptyOption)
                $(appendId).append($('<option></option>').attr('value', '').text('Select...'));

            $.each(data, function (key, value) {
                $(appendId).append($('<option></option>').attr('value', value.key).text(value.value));
            });
            if (selectedValueId)
                if (!multiSelect)
                    $(appendId).val($(selectedValueId).val());
                else
                    $(appendId).val($(selectedValueId).val().split(","));

            if (callBack)
                callBack();
        },
        complete: function (xhr, status) {
            hideLoader(spinnerNumber);
        },
        error: function (xhr, status, error) {
            hideLoader(spinnerNumber);
            ifhandleErrorApiResponse(xhr);
        }
    });
}

ServerRequest.prototype.saveData = function () {
    let url = this.url;
    let data = this.data;
    let spinnerNumber = this.spinnerNumber;

    $.ajax({
        beforeSend: function (xhr) {
            showLoader(spinnerNumber);
        },
        type: "POST",
        url: url,
        data: data,
        dataType: "json",
        success: function (data) {
            if (data)
                successMsg('Saved.');
            else
                failMsg('Autosave failed.');
        },
        complete: function (xhr, status) {
            hideLoader(spinnerNumber);
        },
        error: function (xhr, status, error) {
            hideLoader(spinnerNumber);
            handleErrorApiResponse(xhr);
        }
    });
}

ServerRequest.prototype.showHtmlData = function () {
    let appendId = this.appendId;
    let spinnerNumber = this.spinnerNumber;
    let url = this.url;

    $(appendId).empty();

    $.ajax({
        beforeSend: function (xhr) {
            showLoader(spinnerNumber);
        },
        type: "GET",
        url: url,
        dataType: "json",
        success: function (data) {
            $(appendId).html(data)
        },
        complete: function (xhr, status) {
            hideLoader(spinnerNumber);
        },
        error: function (xhr, status, error) {
            hideLoader(spinnerNumber);
            handleErrorApiResponse(xhr);
        }
    });
}

ServerRequest.prototype.downloadDocument = function () {
    let spinnerNumber = this.spinnerNumber;
    let url = this.url;

    $.ajax({
        beforeSend: function (xhr) {
            showLoader(spinnerNumber);
        },
        type: "GET",
        url: url,
        dataType: "json",
        success: function (data) {
            $.ajax({
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("Authorization", data.authType + " " + data.token);
                },
                method: "POST",
                url: data.fileUrl,
                data: JSON.stringify(data.encryptedFileName),
                contentType: "application/json",
                xhrFields: {
                    responseType: 'blob'
                },
                success: function (blob, status, xhr) {
                    // 🔹 Extract filename safely
                    let fileName = "downloaded_file";
                    const disposition = xhr.getResponseHeader("Content-Disposition");
                    if (disposition != null) {
                        // Handles both filename and filename*
                        let fileNameMatch = disposition.match(/filename\*?=(?:UTF-8'')?([^;]+)/i);
                        if (fileNameMatch && fileNameMatch.length > 1) {
                            fileName = decodeURIComponent(fileNameMatch[1].replace(/['"]/g, ""));
                        }
                    }

                    // 🔹 Create object URL
                    const blobUrl = window.URL.createObjectURL(blob);

                    // 🔹 Create temporary anchor
                    const link = document.createElement("a");
                    link.href = blobUrl;
                    link.download = fileName;

                    // 🔹 Trigger download (no need to append to DOM)
                    link.click();

                    // 🔹 Cleanup (important)
                    window.URL.revokeObjectURL(blobUrl);
                },
                complete: function (xhr, status) {
                    hideLoader(spinnerNumber);
                },
                error: function (xhr, status, error) {
                    hideLoader(spinnerNumber);
                    handleErrorApiResponse(xhr);
                }
            });
        },
        complete: function (xhr, status) {
        },
        error: function (xhr, status, error) {
            hideLoader(spinnerNumber);
            handleErrorApiResponse(xhr);
        }
    });
}

function htmlEditor(selector, saveFunction) {
    $(selector).richText({
        useParagraph: true,
        fonts: false,
        fontColor: false,
        backgroundColor: false,
        fontSize: false,
        imageUpload: false,
        fileUpload: false,
        videoEmbed: false,
        urls: false,
        height: 480,
        save: true,
        saveOnBlur: 2000,
        saveCallback: function (editor, source, content) {
            saveFunction();
        },
    });
}

// Callback javascript
//function longTask(callBack) {
//    // Some long work/async work. (Example ajax work.)
//    callBack();
//}
//longTask(function () {
//    // Do work here after completing longTask/async work.
//});

// Submit form ajax
//$('#myForm').on('submit', function (event) {
//    event.preventDefault(); // Prevent the default form submission

//    // Serialize the form data
//    var formData = $(this).serialize();

//    // AJAX request
//    $.ajax({
//        url: 'your-server-endpoint-url', // Replace with your server URL
//        type: 'POST',
//        data: formData,
//        success: function (response) {
//            // Handle success - display a success message, clear the form, etc.
//            alert('Form submitted successfully!');
//            $('#myForm')[0].reset();
//        },
//        error: function (xhr, status, error) {
//            // Handle error - display an error message, etc.
//            alert('Error submitting form: ' + error);
//        }
//    });
//});

function getTodayRange() {
    const now = new Date();

    const start = new Date(
        now.getFullYear(),
        now.getMonth(),
        now.getDate(),
        0, 0, 0, 0
    );

    const end = new Date(start);
    end.setDate(end.getDate() + 1);

    return {
        start: start.toISOString(),
        end: end.toISOString()
    };
}

function getCurrentYearRange() {
    const now = new Date();

    const start = new Date(
        now.getFullYear(),
        0,
        1,
        0, 0, 0, 0
    );

    const end = new Date(start);
    end.setFullYear(end.getFullYear() + 1);

    return {
        start: start.toISOString(),
        end: end.toISOString()
    };
}

function validateImage(submitBtnId, imageFieldId, errorFieldId, event) {
    const form = $(submitBtnId).closest("form");
    let ifFormValid = $(form).valid();

    const imageField = document.getElementById(imageFieldId);
    const errorField = document.getElementById(errorFieldId);
    const file = imageField.files[0];
    errorField.textContent = "";

    const allowedExtensions = ["jpg", "png", "gif", "jpeg", "jpeg2", "webp"];
    const maxSize = 200000; // 200 KB

    let isimageFieldValid = true;

    if (file) {
        const fileExtension = file.name.split('.').pop().toLowerCase();

        if (!allowedExtensions.includes(fileExtension.toLowerCase())) {
            errorField.textContent = "Invalid file type. Only " + allowedExtensions.join(', ') + " allowed.";
            isimageFieldValid = false;
        } else if (file.size > maxSize) {
            errorField.textContent = "File size too large. Max " + (maxSize / 1024) + " KB allowed.";
            isimageFieldValid = false;
        }
    }

    if (!ifFormValid) {
        return;
    }
    else if (ifFormValid) {
        if (!isimageFieldValid) {
            imageField.focus();
            event.preventDefault();
            return;
        }
    }

    if (!navigator.onLine) {
        failMsg('There is a network problem.');
    }
    else {
        showBtnLoader(submitBtnId)
        form.submit();
    }
}

function validateDocument(submitBtnId, documentFieldId, errorFieldId, event) {
    const form = $(submitBtnId).closest("form");
    let ifFormValid = $(form).valid();

    const documentField = document.getElementById(documentFieldId);
    const errorField = document.getElementById(errorFieldId);
    const file = documentField.files[0];
    errorField.textContent = "";

    const allowedDocumentExtensions = ["pdf", "doc", "docx", "xls", "xlsx", "ppt", "pptx"];
    const allowedImageExtensions = ["jpg", "png", "gif", "jpeg", "jpeg2", "webp"];
    const maxSize = 10000000; // 10 MB


    let isDocumentFieldValid = true;

    if (file) {
        const fileExtension = file.name.split('.').pop().toLowerCase();

        if (!allowedDocumentExtensions.includes(fileExtension.toLowerCase()) && !allowedImageExtensions.includes(fileExtension.toLowerCase())) {
            errorField.textContent = "Invalid file type. Only " + allowedDocumentExtensions.join(', ') + ", " + allowedImageExtensions.join(', ') + " allowed.";
            isDocumentFieldValid = false;
        } else if (file.size > maxSize) {
            errorField.textContent = "File size too large. Max " + (maxSize / 1048576) + " MB allowed.";
            isDocumentFieldValid = false;
        }
    }

    if (!ifFormValid) {
        return;
    }
    else if (ifFormValid) {
        if (!isDocumentFieldValid) {
            documentField.focus();
            event.preventDefault();
            return;
        }
    }

    if (!navigator.onLine) {
        failMsg('There is a network problem.');
    }
    else {
        showBtnLoader(submitBtnId)
        form.submit();
    }
}

function validateSecondFieldLessThan(submitBtnId, firstFieldId, secondFieldId, errorFieldId, errorMsg, event) {
    const form = $(submitBtnId).closest("form");
    let ifFormValid = $(form).valid();

    const firstField = document.getElementById(firstFieldId);
    const secondField = document.getElementById(secondFieldId);
    const errorField = document.getElementById(errorFieldId);
    errorField.textContent = "";

    let isSecondFieldValid = true;
    if (firstField.value && secondField.value) {
        let firstValue = Number(firstField.value);
        let secondValue = Number(secondField.value);
        if (secondValue > firstValue) {
            errorField.textContent = errorMsg;
            isSecondFieldValid = false;
        }
    }

    if (!ifFormValid) {
        return;
    }
    else if (ifFormValid) {
        if (!isSecondFieldValid) {
            secondField.focus();
            event.preventDefault();
            return;
        }
    }

    if (!navigator.onLine) {
        failMsg('There is a network problem.');
    }
    else {
        showBtnLoader(submitBtnId)
        form.submit();
    }
}
