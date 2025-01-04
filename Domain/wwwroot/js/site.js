// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Function to switch between Light and dark mode
function SwitchMode()
{    
    document.documentElement.setAttribute(
        "data-bs-theme",
        document.documentElement.getAttribute("data-bs-theme") == "dark" ? "light" : "dark");
}

function displayToastrNotification() {
    fetch(window.location.href)
        .then(response => {
            const errorMessage = response.headers.get('X-Error-Message');
            if (errorMessage) {
                toastr.error(errorMessage, 'Error', { timeOut: 5000 });
            } else if (!response.ok) {
                toastr.error('An unexpected error occurred', 'Error', { timeOut: 5000 });
            }
        })
        .catch(() => {
            toastr.error('An unexpected error occurred', 'Error', { timeOut: 5000 });
        });
}

document.addEventListener("DOMContentLoaded", displayToastrNotification);
