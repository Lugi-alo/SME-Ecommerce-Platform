document.addEventListener("DOMContentLoaded", function () {
    var modal = document.getElementById("modal");
    var featuresButton = document.getElementById("featuresButton");
    var saveButton = document.getElementById("saveButton");
    var closeButton = document.getElementsByClassName("close")[0];

    featuresButton.onclick = function () {
        modal.style.display = "block";
    }

    closeButton.onclick = function () {
        modal.style.display = "none";
    }

    saveButton.onclick = function () {
        var checkboxes = document.querySelectorAll("#accommodationFeatures input[type='checkbox']");
        var selectedFeatures = [];
        checkboxes.forEach(function (checkbox) {
            if (checkbox.checked) {
                selectedFeatures.push(checkbox.value);
            }
        });

        var selectedFeaturesDiv = document.getElementById("selectedFeatures");
        selectedFeaturesDiv.innerHTML = "Selected Features: " + selectedFeatures.join(", ");

        modal.style.display = "none";
    }

    window.onclick = function (event) {
        if (event.target == modal) {
            modal.style.display = "none";
        }
    }
});
