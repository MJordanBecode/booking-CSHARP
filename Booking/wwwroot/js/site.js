// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
import 'bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';
import { createIcons } from 'lucide';

document.addEventListener('DOMContentLoaded', () => {
    createIcons(); // Active les icônes basées sur data-lucide
});


import $ from 'jquery';
window.$ = $;
window.jQuery = $;

