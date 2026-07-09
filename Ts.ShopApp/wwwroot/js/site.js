$(function () {
    // Add active state to menu links
    var path = window.location.href; // because the 'href' property of the DOM element is the absolute path
    var firstPath = path.split('/', 4);
    $("#menu a").each(function () {
        if (this.href === path) {
            $(this).addClass("active");
        }
    });
    $("#sub-menu a").each(function () {
        if (this.href === path) {
            $(this).addClass("active");
            if (firstPath.length >= 3) {
                $('a[sub-active="' + firstPath[3] + '"]').addClass("active");
            }
        }
    });

    const menu = document.querySelector('#menu');
    const menuGrabb = document.querySelectorAll('#menu a');
    if (menu != null) {
        draggableScroll(menu, menuGrabb);
    }

    const subMenu = document.querySelector('#sub-menu');
    const subMenuGrabb = document.querySelectorAll('#sub-menu a');
    if (subMenu != null) {
        draggableScroll(subMenu, subMenuGrabb);
    }

    const userMenu = document.querySelector('#user-menu');
    const userMenuGrabb = document.querySelectorAll('#user-menu a');
    if (userMenu != null) {
        draggableScroll(userMenu, userMenuGrabb);
    }
});

function draggableScroll(slider, grabb) {
    let isDown = false;
    let startX;
    let scrollLeft;

    slider.addEventListener('mousedown', (e) => {
        isDown = true;
        slider.classList.add('grabbing-menu');
        if (grabb != null) {
            grabb.forEach(function (sub) {
                sub.classList.add('grabbing-menu');
            });
        }
        startX = e.pageX - slider.offsetLeft;
        scrollLeft = slider.scrollLeft;
    });
    slider.addEventListener('mouseleave', () => {
        isDown = false;
        slider.classList.remove('grabbing-menu');
        if (grabb != null) {
            grabb.forEach(function (sub) {
                sub.classList.remove('grabbing-menu');
            });
        }
    });
    slider.addEventListener('mouseup', () => {
        isDown = false;
        slider.classList.remove('grabbing-menu');
        if (grabb != null) {
            grabb.forEach(function (sub) {
                sub.classList.remove('grabbing-menu');
            });
        }
    });
    slider.addEventListener('mousemove', (e) => {
        if (!isDown) return;
        e.preventDefault();
        const x = e.pageX - slider.offsetLeft;
        const walk = (x - startX) * 3; //scroll-fast
        slider.scrollLeft = scrollLeft - walk;
        //console.log(walk);
    });
}

document.addEventListener("DOMContentLoaded", function () {
    const consent = document.cookie.includes("Ts.Shop.Consent");

    if (consent) {
        var con = document.getElementById("cookieConsent");
        if (con)
            con.style.display = "none";
    }
});

$("#cookieConsent button[data-cookie-string]").on("click", function () {
    document.cookie = $("#cookieConsent button").attr("data-cookie-string");
    $("#cookieConsent").hide();
});

// Disable submit button after submitting
$(document).on('click', '.submitLoader', function (event) {
    //event.preventDefault();
    showFormSubmitLoader(this, event);
});

function showLoader(spinnerNumber) {
    $('body').prepend(`<div class="spinner-wrap" id="spinner-wrap${spinnerNumber}"><div class="spinner"></div></div>`);
}

function hideLoader(spinnerNumber) {
    $('#spinner-wrap' + spinnerNumber).remove();
}

function showFormSubmitLoader(btnProperty) {
    let form = $(btnProperty).closest("form");
    if (!navigator.onLine) {
        failAlert('There is a network problem.');
    }
    else if ($(form).valid()) {
        showBtnLoader(btnProperty)
        $(form).submit();
    }
}

$(document).on('click', '.submitLoaderSimple', function (event) {
    //event.preventDefault();
    showFormSubmitLoaderSimple(this, event);
});

function showFormSubmitLoaderSimple(btnProperty) {
    let form = $(btnProperty).closest("form");
    if (!navigator.onLine) {
        failAlert('There is a network problem.');
    }
    else {
        showBtnLoader(btnProperty)
        $(form).submit();
    }
}

$('#menu-icon').click(function () {
    var searchIcon = $('#search-icon');
    $(this).toggleClass('fa-bars');
    $(this).toggleClass('fa-times');
    $('#menu').toggle();
    $('#sub-menu').toggle();
    $('#search').hide();
    $('#user-menu').hide();
    searchIcon.removeClass('fa-times');
    searchIcon.addClass('fa-search');
});

$('#search-icon').click(function () {
    var menuIcon = $('#menu-icon');
    $(this).toggleClass('fa-search');
    $(this).toggleClass('fa-times');
    $('#search').toggle();
    $('#menu').hide();
    $('#sub-menu').hide();
    $('#user-menu').hide();
    menuIcon.removeClass('fa-times');
    menuIcon.addClass('fa-bars');
});

$('#user-icon').click(function () {
    $('#user-menu').toggle();
    $('#menu').hide();
    $('#sub-menu').hide();
    $('#search').hide();

    var menuIcon = $('#menu-icon');
    menuIcon.removeClass('fa-times');
    menuIcon.addClass('fa-bars');

    var searchIcon = $('#search-icon');
    searchIcon.removeClass('fa-times');
    searchIcon.addClass('fa-search');
});

function showBtnLoader(btnId) {
    $(btnId).append(' <i class="fa fa-spinner fa-spin"></i>').attr('disabled', true);
}

function hideBtnLoader(btnId) {
    $(btnId).attr('disabled', false).find('i.fa-spinner.fa-spin').remove();
}

function showAlert(type, message) {
    const container = document.getElementById('alertContainer');

    const alert = document.createElement('div');
    alert.className = 'alert ' + type;

    alert.innerHTML = `
      ${message}
      <span class="close" onclick="this.parentElement.remove()"><i class="fa fa-times"></i></span>
    `;

    container.appendChild(alert);

    setTimeout(() => {
        alert.remove();
    }, 10000);
}

function successAlert(message) {
    showAlert('success', message);
}

function failAlert(message) {
    showAlert('error', message);
}

function redirectToLogin() {
    // current URL encode karo
    const returnUrl = encodeURIComponent(window.location.pathname + window.location.search);

    // redirect to login
    window.location.href = `/login?returnurl=${returnUrl}`;
    return;
}

function handleErrorApiResponse(xhr) {
    if (!navigator.onLine) {
        failAlert('There is a network problem.');
    }
    else if (xhr.status == 401) {
        location.reload();
        return;
    }
    else if (xhr.status == 403) {
        failAlert('You are unauthorised. Please try relogin.');
    }
    else if (xhr.status == 400) {
        failAlert(xhr.responseText);
    }
    else if (xhr.status == 500) {
        failAlert('Internal error.');
    }
    else {
        failAlert(xhr.responseText);
    }
}

function ServerRequest(url, btnId, appendId, data, callBack) {
    this.url = url;
    this.btnId = btnId;
    this.appendId = appendId;
    this.data = data;
    this.callBack = callBack;
}

ServerRequest.prototype.loadMoreData = function () {
    let callBack = this.callBack;
    let btnId = this.btnId;
    let appendId = this.appendId;
    let data = this.data;
    let url = this.url;

    $.ajax({
        beforeSend: function (xhr) {
            showBtnLoader(btnId);
        },
        type: "POST",
        url: url,
        data: data,
        dataType: "html",
        success: function (data, status, xhr) {
            if (xhr.status === 200) {
                $(appendId).append(data);
                callBack(true);
            }
            if (xhr.status === 204) {
                $(appendId).append(
                    '<div class="no-result"><b>You have reached the end.</b></div>'
                );
                callBack(false);
            }
        },
        complete: function (xhr, status) {
            hideBtnLoader(btnId);
        },
        error: function (xhr, status, error) {
            hideBtnLoader(btnId);
            if (!navigator.onLine) {
                failAlert('There is a network problem.');
            }
            else
                failAlert('Error loading data.');
        }
    });
}

ServerRequest.prototype.loadGalleryData = function () {
    let appendId = this.appendId;
    let data = this.data;
    let url = this.url;

    $.ajax({
        beforeSend: function (xhr) {
            $(appendId).append('<div class="txt-center"><i class="fa fa-spinner fa-spin"></i></div>');
        },
        type: "Post",
        url: url,
        data: data,
        dataType: "html",
        success: function (data, status, xhr) {
            if (xhr.status === 200) {
                $(appendId).html(data);
            }
            if (xhr.status === 204) {
                $(appendId).html(
                    '<div class="no-result"><b>No Data.</b></div>'
                );
            }
        },
        complete: function (xhr, status) {
        },
        error: function (xhr, status, error) {
            if (!navigator.onLine) {
                failAlert('There is a network problem.');
            }
            else
                $(appendId).html('<div class="no-result"><b>Error!</b></div>');
        }
    });
}

ServerRequest.prototype.saveCountData = function () {
    let url = this.url;

    $.ajax({
        type: "Post",
        url: url,
        dataType: "html",
        success: function (data, status, xhr) {
            //if (xhr.status !== 204)
            //    console.log('count failed!');
        },
        complete: function (xhr, status) {
        },
        error: function (xhr, status, error) {
            //console.log('count error!');
        }
    });
}

ServerRequest.prototype.saveCart = function () {
    let btnId = this.btnId;
    let data = this.data;
    let url = this.url;

    $.ajax({
        beforeSend: function (xhr) {
            showBtnLoader(btnId);
        },
        type: "POST",
        url: url,
        data: {
            "Model.ItemCount": 0,
            "Model.ProductDetailId": data
        },
        dataType: "html",
        success: function (data, status, xhr) {
            if (xhr.status === 204) {
                successAlert('Item added to <i class="fa fa-shopping-cart"></i> cart. <a href="/cart"><b>Go to Cart</b></a>');
            }
            else {
                failAlert(data);
            }
        },
        complete: function (xhr, status) {
            hideBtnLoader(btnId);
        },
        error: function (xhr, status, error) {
            if (xhr.status === 401) {
                // current URL encode karo
                const returnUrl = encodeURIComponent(window.location.pathname + window.location.search);

                // redirect to login
                window.location.href = `/login?returnurl=${returnUrl}`;
                return;
            }
            hideBtnLoader(btnId);
            handleErrorApiResponse(xhr);
        }
    });
}

ServerRequest.prototype.downloadDocument = function () {
    let btnId = this.btnId;
    let url = this.url;

    $.ajax({
        beforeSend: function (xhr) {
            showBtnLoader(btnId);
        },
        type: "POST",
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
                    hideBtnLoader(btnId);
                },
                error: function (xhr, status, error) {
                    hideBtnLoader(btnId);
                    handleErrorApiResponse(xhr);
                }
            });
        },
        complete: function (xhr, status) {
        },
        error: function (xhr, status, error) {
            hideBtnLoader(btnId);
            handleErrorApiResponse(xhr);
        }
    });
}

function reCaptcha(submitBtnId, googleCaptchaSiteKey, actionName, recaptchaId, event) {
    let form = $(submitBtnId).closest("form");
    //event.preventDefault();

    if (!navigator.onLine) {
        failAlert('There is a network problem.');
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
                failAlert("An error occurred while verifying the reCAPTCHA. Please refresh page and try again.");
                hideBtnLoader(submitBtnId);
                event.preventDefault();
            }
        });
    }
}

function showAlertBottom(showExpireinSec) {
    const container = document.getElementById('sessionContainer');

    const alert = document.createElement('div');
    alert.className = 'alert warning';

    alert.innerHTML = `
      <strong>Warning!</strong> <span id="expireContent">Your session is about to expire in <span id="expireCountDown">${showExpireinSec}</span><br /> Please save you work.</span>
    `;

    container.appendChild(alert);
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
            showAlertBottom(showExpireinSec);

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

function checkAdBlocker() {
    // 1. Check AdSense aswift div
    const adDiv = document.querySelector('div[id^="aswift"]');

    // 2. Check if Adblock message already exists (from other system)
    const fcMessage = document.querySelector('.fc-ab-root, .fc-whitelist-root');

    // 3. Logic
    if (!adDiv && !fcMessage) {
        showAdblockMessage();
    }
}

function showAdblockMessage() {
    // Prevent duplicate overlay
    if (document.getElementById("blog-overlay")) return;

    const overlay = document.createElement("div");
    overlay.id = "blog-overlay";

    overlay.style.position = "fixed";
    overlay.style.top = "0";
    overlay.style.left = "0";
    overlay.style.width = "100%";
    overlay.style.height = "100%";
    overlay.style.background = "rgba(0, 0, 0, 0.6)";
    overlay.style.zIndex = "999999";
    overlay.style.display = "flex";
    overlay.style.justifyContent = "center";
    overlay.style.alignItems = "center";
    overlay.style.color = "white";
    overlay.style.fontSize = "20px";
    overlay.style.textAlign = "center";

    overlay.innerHTML = `
    <div>
        <h2>Please disable your ad blocker</h2>
        <p>To support us, please disable your ad blocker and refresh the page.</p>
    </div>
    `;

    document.body.appendChild(overlay);

    // Freeze screen
    document.body.style.overflow = "hidden";
}

function addToCart(btnId, productDetailId) {
    let serverRequest = new ServerRequest('/cart/add', btnId, null, productDetailId);
    serverRequest.saveCart();
}