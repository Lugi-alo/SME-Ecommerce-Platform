    document.addEventListener("DOMContentLoaded", function () {
        var modal = document.getElementById("modal");
    var featuresButton = document.getElementById("featuresButton");
    var saveButton = document.getElementById("modalButton");
    var closeButton = document.getElementsByClassName("close")[0];

    featuresButton.onclick = function () {
        modal.style.display = "block";
        }

    closeButton.onclick = function () {
        modal.style.display = "none";
        }

    saveButton.onclick = function () {
            var checkboxes = document.querySelectorAll("#accommodationFeatures .featureCheckbox");
    var selectedFeatureIds = [];
    checkboxes.forEach(function (checkbox) {
                if (checkbox.checked) {
        selectedFeatureIds.push(checkbox.value);
                }
            });

    console.log("Selected Feature IDs:", selectedFeatureIds);

    document.getElementById("selectedFeatureIds").value = selectedFeatureIds.join(",");

    var selectedFeaturesDiv = document.getElementById("selectedFeatures");
    selectedFeaturesDiv.innerHTML = "Selected Features: " + selectedFeatureIds.join(", ");

    modal.style.display = "none";
        }

    window.onclick = function (event) {
            if (event.target == modal) {
        modal.style.display = "none";
            }
        }
    });
