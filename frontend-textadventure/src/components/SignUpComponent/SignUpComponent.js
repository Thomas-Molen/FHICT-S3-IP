import './SignUpComponent.css'
import { React, useState } from 'react';

export function SignUpComponent({ isOpen }) {
    const [isLogin, setIsLogin] = useState(true);
    const [password, setPassword] = useState("");
    const [repeatPassword, setRepeatPassword] = useState("");

    if (!isOpen) return null
    if (isLogin)
        return (
            <div className="SignUpForm rounded border border-info">
                <form>
                    <div class="form-group">
                        <label for="loginemail">Email address</label>
                        <input type="email" class="form-control" id="exampleFormControlInput1" placeholder="name@example.com" required />
                    </div>
                    <div class="form-group signUpFormField">
                        <label for="loginpassword">Password</label>
                        <input type="password" class="form-control" id="exampleInputPassword" placeholder="Password" required />
                    </div>
                    <div className="signUpSubmit">
                        <button class="btn btn-primary d-inline signUpSubmitButton">Log in</button>
                        <a className="d-inline" onClick={() => setIsLogin(!isLogin)} href="javascript:void(0)">create account</a>
                    </div>
                </form>
            </div>
        )

    function checkPassword() {
        if(password && repeatPassword && password == repeatPassword) {
            console.log("Same password yey");
            return;
        }
        console.log("not same");
    }

    return (
        <div className="SignUpForm rounded border border-info">
            <div class="form-group">
                <label for="registeremail">Email address</label>
                <input type="email" class="form-control" placeholder="name@example.com" required />
            </div>
            <div class="form-group signUpFormField">
                <label for="registerpassword1">Password</label>
                <input type="password" class="form-control" value={password} placeholder="Password" onChange={(e) => setPassword(e.target.value)} required />
            </div>
            <div class="form-group signUpFormField">
                <label for="registerpassword2">Confirm Password</label>
                <input type="password" class="form-control" value={repeatPassword} placeholder="Password" onChange={(e) => setRepeatPassword(e.target.value)} required />
            </div>
            <div className="signUpSubmit">
                <button class="btn btn-primary d-inline signUpSubmitButton" onClick={() => checkPassword()}>Register</button>
                <a className="d-inline" onClick={() => setIsLogin(!isLogin)} >log in</a>
            </div>
        </div>
    )
}