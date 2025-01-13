export default class AuthView {

    constructor() {
        this.usernameInput = document.getElementById("username");
        this.passwordInput = document.getElementById("password");
        this.alertBox = document.getElementById("alertBox");
        this.alertContainer = document.getElementById("alertContainer");
    }

    bindLogin(handler) {
        const loginButton = document.getElementById("loginButton");
        if(loginButton == null){
            return;
        }
        loginButton.addEventListener("click", (evt) => {
            evt.preventDefault()
            const username = this.usernameInput.value;
            const password = this.passwordInput.value;
            handler(username, password);
        });
    }

    bindRegister(handler){
        const registerButton = document.getElementById("registerButton");
        const fullNameInput = document.getElementById("fullName");
        if(registerButton == null){
            return;
        }
        registerButton.addEventListener("click", (evt) => {
            evt.preventDefault();
            const username = this.usernameInput.value;
            const password = this.passwordInput.value;
            const fullName = fullNameInput.value
            handler(username, password, fullName);
        });
    }

    showMessage(type, message) {
        this.alertContainer.style.display = 'block';
        this.alertBox.className = `alert alert-${type} fade`;
        this.alertBox.innerText = message;
        this.alertBox.classList.add('show');
    }
}