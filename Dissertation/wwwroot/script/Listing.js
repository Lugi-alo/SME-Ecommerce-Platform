document.getElementById("listingType").addEventListener("change", function () {
    var listingType = this.value;
    if (listingType === "Accommodation") {
        document.getElementById("accommodationFeatures").style.display = "block";
    } else {
        document.getElementById("accommodationFeatures").style.display = "none";
    }
});