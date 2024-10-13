let msgContainer = document.getElementById("msg-container");
let msg = document.getElementById("msg");
let msgOuter = document.getElementById("msg-outer");
let img = document.getElementById("img");
function ShowMsg(info, status) {
    if (status == 1) {
        img.src = "/Images/icons8-error-64.png";
        msgOuter.style.background = "#a60000";
    } else {    
        img.src = "/Images/icons8-ok-512.png"

        msgOuter.style.background = "green";
    }
    msg.textContent = info;
    msgContainer.style.display = "flex";
    setTimeout(() => { msgContainer.style.display = "none" }, 2000)
}
window.ShowMsg = ShowMsg;





