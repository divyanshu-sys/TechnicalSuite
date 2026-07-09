document.addEventListener('DOMContentLoaded', function () {
    const pages = document.querySelectorAll('.story-page');
    const progressBars = document.querySelectorAll('.progress-bar');
    const progressContainer = document.querySelector('.progress-bar-container');
    const prevButton = document.getElementById('prev-btn');
    const nextButton = document.getElementById('next-btn');
    const shareButton = document.getElementById('share-btn');
    let currentPage = 0;
    //const changeInterval = 12000; // Change story every 12 seconds
    //let autoChangeInterval;
    let adIndex = null;

    function loadAnotherImagesForPage(index) {
        let loadImage = (img) => {
            img.src = img.getAttribute('data-src');
            img.removeAttribute('data-src');
        };

        let pagesToLoad = [index];
        pagesToLoad.forEach((pageIndex) => {
            if (pageIndex < pages.length) {
                let images = pages[pageIndex].querySelectorAll('img[data-src]');
                images.forEach(loadImage);
            }
        });
    }

    function showPage(index) {
        // Check if the current page contains AdSense ad
        let isAd = containsAdSense(pages[index]);

        pages.forEach((page, i) => {
            page.classList.remove('active-page');
            if (i === index) {
                if (!isAd)
                    page.classList.add('active-page');
            }
        });

        updateProgressBar(index, isAd);
    }

    function updateProgressBar(index, isAd) {
        let newIndex = index;

        if (isAd) {
            adIndex = index;
            shareButton.style.display = 'none';
            progressContainer.style.display = 'none';
            //clearInterval(autoChangeInterval); // Stop auto change on AdSense page
        } else {
            shareButton.style.display = 'initial';
            progressContainer.style.display = 'flex';
            //resetAutoChange(); // Resume auto change on other pages
        }

        if (!isAd) {
            if (adIndex != null && adIndex < index)
                newIndex--;
            progressBars.forEach((bar, i) => {
                bar.classList.remove('active-page');
                //bar.style.animation = 'none';
                if (i < newIndex) {
                    bar.classList.add('completed');
                    bar.style.width = '100%';
                } else {
                    bar.classList.remove('completed');
                    bar.style.width = '0';
                }
            });

            if (newIndex < progressBars.length) {
                progressBars[newIndex].classList.add('active-page');
                //progressBars[newIndex].style.animation = `progress ${changeInterval / 1000}s linear forwards`;
            }
        }
    }

    function nextPage() {
        if (currentPage < pages.length - 1) {
            currentPage++;
            loadAnotherImagesForPage(currentPage + 1);
            showPage(currentPage);
        } else {
            //clearInterval(autoChangeInterval); // Stop auto change when the last page is reached
        }
    }

    function prevPage() {
        if (currentPage > 0) {
            currentPage--;
            showPage(currentPage);
        }
    }

    //function resetAutoChange() {
    //    clearInterval(autoChangeInterval);
    //    if (!containsAdSense(pages[currentPage])) {
    //        autoChangeInterval = setInterval(function () {
    //            if (!containsAdSense(pages[currentPage])) {
    //                nextPage();
    //            }
    //        }, changeInterval);
    //    }
    //}

    prevButton.addEventListener('click', function () {
        prevPage();
        //resetAutoChange();
    });

    nextButton.addEventListener('click', function () {
        nextPage();
        //resetAutoChange();
    });

    document.addEventListener('keydown', function (event) {
        if (event.key === 'ArrowRight') {
            nextPage();
            //resetAutoChange();
        } else if (event.key === 'ArrowLeft') {
            prevPage();
            //resetAutoChange();
        }
    });

    // Function to check if a page contains AdSense ad
    function containsAdSense(page) {
        return page.querySelector('.adsense-ad') !== null;
    }

    shareButton.addEventListener('click', () => {
        if (navigator.share) {
            navigator.share({
                title: 'Web Story',
                text: 'Check out this web story!',
                url: window.location.href,
            })
                .catch((error) => console.log('Error in sharing', error));
        } else {
            alert('Your browser does not support the web share.');
        }
    });

    // Initialize the first page and progress bar
    showPage(currentPage);
    loadAnotherImagesForPage(currentPage + 1);
    //resetAutoChange(); // Start the auto-change interval
});

function ServerRequest(url) {
    this.url = url;
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
