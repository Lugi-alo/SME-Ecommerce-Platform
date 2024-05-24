// Move your script to the end of the HTML body or use event listener for DOMContentLoaded
document.addEventListener("DOMContentLoaded", function () {
    let slideIndex = 0;
    let slides = document.querySelectorAll(".container");

    function showSlide(index) {
        if (index < 0) {
            slideIndex = slides.length - 1;
        } else if (index >= slides.length) {
            slideIndex = 0;
        }

        slides.forEach((slide, i) => {
            if (i === slideIndex) {
                slide.style.display = "block";
            } else {
                slide.style.display = "none";
            }
        });
    }

    function prevSlide() {
        slideIndex--;
        showSlide(slideIndex);
    }

    function nextSlide() {
        slideIndex++;
        showSlide(slideIndex);
        console.log("Function called");
    }

    showSlide(slideIndex);

    document.getElementById("prevButton").addEventListener("click", prevSlide);
    document.getElementById("nextButton").addEventListener("click", nextSlide);
});
