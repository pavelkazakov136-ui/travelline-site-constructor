async function doLogin(){
    const username = document.getElementById("username").value;
    const password = document.getElementById("password").value;
    const msg = document.getElementById("login-msg");
    msg.textContent = ""; 
    const res = await fetch("/api/auth/login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ username, password })
    });
    if (res.ok){
        window.location.href = "/admin.html"
    }else{
        msg.textContent = "Неверный логин или пароль";
    }
}

    document.getElementById("login-btn").addEventListener("click", doLogin);
    document.getElementById("username").addEventListener("keydown",e=>{
        if (e.key === "Enter"){
            document.getElementById("password").focus();
        }
    });
    document.getElementById("password").addEventListener("keydown",e => {
        if (e.key ==="Enter") {
            doLogin();
        }
    });