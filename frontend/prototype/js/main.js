// Function to open the modal
function openModal(modalId) {
    document.getElementById(modalId).style.display = "flex";
}

// Function to close the modal
function closeModal(modalId) {
    document.getElementById(modalId).style.display = "none";
}

// Function to save event data

function saveEventData() {
    //document.getElementById("event-name").innerText = document.getElementById("edit-event-name").value;
    //document.getElementById("event-start").innerText = document.getElementById("edit-event-start").value;
    //document.getElementById("event-finish").innerText = document.getElementById("edit-event-finish").value;
    //document.getElementById("event-location").innerText = document.getElementById("edit-event-location").value;
    //document.getElementById("event-bio").innerText = document.getElementById("edit-event-bio").value;
    closeModal('event-modal');
}

// Function to save networking data

function saveNetworkingData() {
    //document.getElementById("networking-start").innerText = document.getElementById("edit-networking-start").value;
    //document.getElementById("networking-finish").innerText = document.getElementById("edit-networking-finish").value;
    //document.getElementById("networking-session").innerText = document.getElementById("edit-networking-session").value;
    //document.getElementById("meeting-duration").innerText = document.getElementById("edit-meeting-duration").value;
    //document.getElementById("break-duration").innerText = document.getElementById("edit-break-duration").value;
    //document.getElementById("tables").innerText = document.getElementById("edit-tables").value;
    closeModal('networking-modal');
}


// Close modal if clicked outside
window.onclick = function(event) {
    let modals = document.querySelectorAll(".modal");
    modals.forEach(modal => {
        if (event.target === modal) {
            modal.style.display = "none";
        }
    });
};