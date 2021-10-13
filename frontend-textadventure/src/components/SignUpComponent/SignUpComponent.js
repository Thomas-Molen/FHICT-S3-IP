import './SignUpComponent.css'
import { React, useState } from 'react';

export function SignUpComponent({ isOpen }) {
    const [isLogin, setIsLogin] = useState(true);

    const [email, setEmail] = useState("");
    const [username, setUsername] = useState("");
    const [loginPassword, setLoginPassword] = useState("");
    const [registerPassword, setRegisterPassword] = useState("");
    const [repeatRegisterPassword, setRepeatRegisterPassword] = useState("");

    if (!isOpen) return null
    if (isLogin)
        return (
            <div className="SignUpForm rounded border border-info">
                <div className="form-group signUpFormField">
                    <label>Email address</label>
                    <input type="email" className="form-control" value={email} placeholder="name@example.com" onChange={(e) => setEmail(e.target.value)} required />
                </div>
                <div className="form-group signUpFormField">
                    <label>Password</label>
                    <input type="password" className="form-control" value={loginPassword} id="exampleInputPassword" placeholder="Password" onChange={(e) => setLoginPassword(e.target.value)} required />
                </div>
                <div className="signUpSubmit">
                    <button className="btn btn-primary d-inline signUpSubmitButton" onClick={() => Login('https://localhost:5001/api/User/login', { email: email, password: loginPassword })}>
                        Log in
                    </button>
                    <button type="button" className="btn btn-link" onClick={() => setIsLogin(!isLogin)}>create account</button>
                </div>
            </div>
        )

    function Login(route, body) {
        fetch(route, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            credentials: "include",
            body: JSON.stringify(body),
        })
            .then(response => response.json())
            .then(data => {
                console.log(data);
            })
            .catch(error => {
                console.error('Error:', error);
            });
    }

    return (
        <div className="SignUpForm rounded border border-info">
            <div className="form-group signUpFormField">
                <label>Email address</label>
                <input type="email" className="form-control" value={email} placeholder="name@example.com" onChange={(e) => setEmail(e.target.value)} required />
            </div>
            <div className="form-group signUpFormField">
                <label>Username</label>
                <input type="email" className="form-control" value={username} placeholder="username" onChange={(e) => setUsername(e.target.value)} required />
            </div>
            <div className="form-group signUpFormField">
                <label>Password</label>
                <input type="password" className="form-control" value={registerPassword} placeholder="password" onChange={(e) => setRegisterPassword(e.target.value)} required />
            </div>
            <div className="form-group signUpFormField">
                <label>Confirm Password</label>
                <input type="password" className="form-control" value={repeatRegisterPassword} placeholder="password" onChange={(e) => setRepeatRegisterPassword(e.target.value)} required />
            </div>
            <div className="signUpSubmit">
                <button className="btn btn-primary d-inline signUpSubmitButton" onClick={() => Register('https://localhost:5001/api/User/register', {email: email, username: username, password: registerPassword})}>
                    Register
                </button>
                <button type="button" className="btn btn-link" onClick={() => setIsLogin(!isLogin)}>log in</button>
            </div>
        </div>
    )

    function Register(route, body) {
        if (checkPassword) {
            fetch(route, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json', 
                    'Accept': 'application/json',
                },
                credentials: "include",
                body: JSON.stringify(body),
            })
                .then(response => response.json())
                .then(data => {
                    console.log(data);
                })
                .catch(error => {
                    console.error('Error:', error);
                });
        }
    }

    function checkPassword() {
        if (registerPassword && repeatRegisterPassword && registerPassword == repeatRegisterPassword) {
            return true;
        }
        return false;
    }
}