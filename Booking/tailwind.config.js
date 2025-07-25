/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./Views/**/*.cshtml",     // Razor views
    "./Pages/**/*.cshtml",     // Razor pages (si t'en as)
    "./wwwroot/js/**/*.js"     // Tes scripts
  ],
  theme: {
    extend: {},
  },
  plugins: [],
}
