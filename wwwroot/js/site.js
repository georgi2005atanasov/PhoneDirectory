function toggleAddress() {
    const addressFields = document.getElementById('addressFields');
    if (addressFields.style.display === "none") {
        addressFields.style.display = "block";
    } else {
        addressFields.style.display = "none";
    }
}

function previewPhoto(event) {
    const reader = new FileReader();
    reader.onload = function () {
        const preview = document.getElementById('photoPreview');
        const icon = document.getElementById('uploadIcon');
        preview.src = reader.result;
        preview.style.display = 'flex';
        icon.style.display = 'none';
    };
    reader.readAsDataURL(event.target.files[0]);
}

function goBack() {
    history.back();
}