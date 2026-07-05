(()=>{
    const LOGIN_PAGE = "/login.html";
    const rawFetch = window.fetch.bind(window)

    window.fetch = async (...args) => {
        const res = await rawFetch(...args);
        if (res.status === 401){
            window.location.href = LOGIN_PAGE;
        }
        return res;
    }
    rawFetch("/api/auth/me").then((res) => {
        if (!res.ok) window.location.href = LOGIN_PAGE;
    });

    document.addEventListener("DOMContentLoaded", () => {
        const btn = document.getElementById("logout-btn");

        btn.addEventListener("click", async () => {
            await rawFetch("/api/auth/logout", { method: "POST" });
            window.location.href = LOGIN_PAGE;
        });

    });


})();