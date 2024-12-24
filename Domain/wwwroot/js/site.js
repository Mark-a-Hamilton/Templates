﻿// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

function SwitchMode() {    // Function to switch between Light and dark mode
    document.documentElement.setAttribute(
        "data-bs-theme",
        document.documentElement.getAttribute("data-bs-theme") == "dark" ? "light" : "dark"
    );
}