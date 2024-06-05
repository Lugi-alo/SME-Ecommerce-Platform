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
        var checkboxes = document.querySelectorAll("#accommodationFeatures input[type='checkbox']");
        var selectedFeatureNames = [];
        checkboxes.forEach(function (checkbox) {
            if (checkbox.checked) {
                selectedFeatureNames.push(checkbox.value);
            }
        });

        // Debugging: Output selected feature names to console
        console.log("Selected Feature Names:", selectedFeatureNames);

        var selectedFeatureNamesInput = document.createElement("input");
        selectedFeatureNamesInput.setAttribute("type", "hidden");
        selectedFeatureNamesInput.setAttribute("name", "selectedFeatureNames");
        selectedFeatureNamesInput.setAttribute("value", selectedFeatureNames.join(","));
        document.querySelector("form").appendChild(selectedFeatureNamesInput);

        var selectedFeaturesDiv = document.getElementById("selectedFeatures");
        selectedFeaturesDiv.innerHTML = "Selected Features: " + selectedFeatureNames.join(", ");

        modal.style.display = "none";
    }


    window.onclick = function (event) {
        if (event.target == modal) {
            modal.style.display = "none";
        }
    }
});
