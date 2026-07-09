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
    const consent = document.cookie.includes("Ts.Public.Consent");

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

function showFormSubmitLoader(btnProperty) {
    let form = $(btnProperty).closest("form");
    if (!navigator.onLine) {
        alert('There is a network problem.');
    }
    else if ($(form).valid()) {
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
    menuIcon.removeClass('fa-times');
    menuIcon.addClass('fa-bars');
});

function showBtnLoader(btnId) {
    $(btnId).append(' <i class="fa fa-spinner fa-spin"></i>').attr('disabled', true);
}

function hideBtnLoader(btnId) {
    $(btnId).attr('disabled', false).find('i.fa-spinner.fa-spin').remove();
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
                alert('There is a network problem.');
            }
            else
                alert('Error loading data.');
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
                alert('There is a network problem.');
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