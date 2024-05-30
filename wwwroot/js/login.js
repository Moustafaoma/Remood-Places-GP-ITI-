console.clear();

const loginBtn = document.getElementById('login');
const signupBtn = document.getElementById('signup');

loginBtn.addEventListener('click', (e) => {
	let parent = e.target.parentNode.parentNode;
	Array.from(e.target.parentNode.parentNode.classList).find((element) => {
		if(element !== "slide-up") {
			parent.classList.add('slide-up')
		}else{
			signupBtn.parentNode.classList.add('slide-up')
			parent.classList.remove('slide-up')
		}
	});
});

signupBtn.addEventListener('click', (e) => {
	let parent = e.target.parentNode;
	Array.from(e.target.parentNode.classList).find((element) => {
		if(element !== "slide-up") {
			parent.classList.add('slide-up')
		}else{
			loginBtn.parentNode.parentNode.classList.add('slide-up')
			parent.classList.remove('slide-up')
		}
	});
});
document.addEventListener("DOMContentLoaded", function() {
    // Sign up button click event
    document.getElementById("signup-btn").addEventListener("click", function() {
        var name = document.getElementById("signup-name").value.trim();
        var email = document.getElementById("signup-email").value.trim();
        var password = document.getElementById("signup-password").value.trim();

        // Regular expressions
        var nameRegex = /^[a-zA-Z]{2,}$/; // أقل من حرفين
        var passwordRegex = /^.{10,}$/; // أقل من عشرة حروف
        var emailRegex = /^[a-zA-Z0-9._%+-]+@gmail.com$/; // نفس حساب Gmail

        // تحقق من تطابق القواعد
        if (!nameRegex.test(name)) {
            alert("Please enter a name with at least two letters.");
            return;
        }
        if (!passwordRegex.test(password)) {
            alert("Password must be at least ten characters long.");
            return;
        }
        if (!emailRegex.test(email)) {
            alert("Please enter a Gmail account email address.");
            return;
        }

        // If all validation passed, proceed with sign up
        alert("Sign up successful!");
        // Clear input fields
        document.getElementById("signup-name").value = "";
        document.getElementById("signup-email").value = "";
        document.getElementById("signup-password").value = "";
    });

    // Log in button click event
    document.getElementById("login-btn").addEventListener("click", function() {
        var email = document.getElementById("login-email").value.trim();
        var password = document.getElementById("login-password").value.trim();

        // Regular expression for password
        var passwordRegex = /^.{10,}$/; // At least ten characters

        // Check if password meets the criteria
        if (!passwordRegex.test(password)) {
            alert("Password must be at least ten characters long.");
            return;
        }

        // If all validation passed, proceed with log in
        alert("Log in successful!");
        // Clear input fields
        document.getElementById("login-email").value = "";
        document.getElementById("login-password").value = "";
    });
});

