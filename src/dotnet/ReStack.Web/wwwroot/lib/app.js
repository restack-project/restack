window.IsScrollBottom = function () {
    return $(window).scrollTop() + $(window).height() == $(document).height();
}

window.GetCurrentUrl = function() {
    return window.location.href;
}

window.ScrollToBottom = function(id) {
    const element = document.getElementById(id);
   
    element.scrollTo(0, element.scrollHeight);
}

window.ScrollToTop = function (id) {
    const element = document.getElementById(id);

    element.scrollTo(0, 0);
}

window.setTheme = function (theme) {
    localStorage.theme = theme;
    loadTheme();
}

window.getTheme = function (theme) {
    return localStorage.theme;
}

window.loadTheme = function () {
    if (localStorage.theme === 'dark' || (!('theme' in localStorage) && window.matchMedia('(prefers-color-scheme: dark)').matches)) {
        document.documentElement.classList.add('dark')
    } else {
        document.documentElement.classList.remove('dark')
    }
}