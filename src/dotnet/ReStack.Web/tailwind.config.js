/** @type {import('tailwindcss').Config} */

/*
    npm install -D tailwindcss
    npm install -D @tailwindcss/forms
    npx tailwindcss -i ../ReStack.Web/Styles/Site.css -o ../ReStack.Web/wwwroot/css/app.css --watch
*/

const colors = require("tailwindcss/colors");

module.exports = {
    darkMode: 'class',
    content: [
        "../ReStack.Web/Components/**.{razor,cs}"
        , "../ReStack.Web/Modals/**.{razor,cs}"
        , "../ReStack.Web/Pages/**.{razor,cs}"
        , "../ReStack.Web/Pages/*/**.{razor,cs}"
        , "../ReStack.Web/Pages/*/*/**.{razor,cs}"
        , "../ReStack.Web/Shared/**.{razor,cs}"
        , "../ReStack.Web/Extensions/**.cs"
        , "../ReStack.Web/Styles/Site.css",
        , "../ReStack.Web/App.razor"
    ],
    theme: {
        extend: {},
        colors: {
            transparent: "transparent",
            current: "currentColor",
            black: "#000",
            white: "#fff",
            slate: colors.slate,
            gray: colors.gray,
            neutral: colors.neutral,
            stone: colors.stone,
            red: colors.red,
            orange: colors.orange,
            amber: colors.amber,
            yellow: colors.yellow,
            lime: colors.lime,
            green: colors.green,
            emerald: colors.emerald,
            teal: colors.teal,
            cyan: colors.cyan,
            sky: colors.sky,
            blue: colors.blue,
            indigo: colors.indigo,
            violet: colors.violet,
            purple: colors.purple,
            fuchsia: colors.fuchsia,
            pink: colors.pink,
            rose: colors.rose,
            zinc: colors.zinc,
            primary: {
                100: '#66a4a7',
                200: '#4d9599',
                300: '#33868a',
                400: '#1a777c',
                500: '#00686D',
                600: '#00494c',
                700: '#003437',
                800: '#001f21',
                900: '#001516',
            },
        }
    },
    plugins: [
        require('@tailwindcss/forms')
    ],
    //safelist: [{ pattern: /./ }],
}

