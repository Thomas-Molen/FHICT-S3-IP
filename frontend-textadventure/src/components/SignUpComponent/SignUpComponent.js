import './SignUpComponent.css'
import { React, useState } from 'react';
import { useJWTActions, useUserActions } from '../../actions'
import { CreateRequest } from '../../actions/APIConnectionHelper'

export function SignUpComponent({ isOpen }) {
    const JWTActions = useJWTActions();
    const UserAtions = useUserActions();

    const [isLogin, setIsLogin] = useState(true);
    const [isRegistering, setIsRegistering] = useState(false);
    const [isLoggingIn, setIsLoggingIn] = useState(false);

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
                    {(() => {
                        if (isLoggingIn == true) {
                            return (
                                <button className="btn btn-primary" type="button" disabled>
                                    <span className="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>
                                    Loading...
                                </button>
                            )
                        }
                        return (
                            <button className="btn btn-primary d-inline signUpSubmitButton" onClick={() => Login()}>
                                Log in
                            </button>
                        )
                    })()}

                    <button type="button" className="btn btn-link signUpSwitchLink" onClick={() => setIsLogin(!isLogin)}>create account</button>
                </div>
            </div>
        )

    async function Login() {
        setIsLoggingIn(true);
        await CreateRequest('POST', 'User/login', { email: email, password: loginPassword })
            .then(data => {
                JWTActions.setGlobalJWTState(data.token);
                UserAtions.setGlobalUserState({ user_id: data.id, username: data.username, email: data.email, admin: data.admin });
            })
            .catch(error => {
                console.error('Error:', error);
                setIsLoggingIn(false);
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
                {(() => {
                    if (isRegistering == true) {
                        return (
                            <button className="btn btn-primary" type="button" disabled>
                                <span className="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>
                                Loading...
                            </button>
                        )
                    }
                    return (
                        <button className="btn btn-primary d-inline signUpSubmitButton" onClick={() => Register()}>
                            Register
                        </button>
                    )
                })()}

                <button type="button" className="btn btn-link signUpSwitchLink" onClick={() => setIsLogin(!isLogin)}>log in</button>
            </div>
        </div>
    )

    async function Register() {
        if (checkPassword) {
            setIsRegistering(true);
            await CreateRequest('POST', 'User/register', { email: email, username: username, password: registerPassword })
                .then(data => {
                    JWTActions.setGlobalJWTState(data.token);
                    UserAtions.setGlobalUserState({ user_id: data.id, username: data.username, email: data.email, admin: data.admin });
                    setIsRegistering(false);
                })
                .catch(error => {
                    console.error('Error:', error);
                    setIsRegistering(false);
                });
        }
    }

    function checkPassword() {
        if (registerPassword && repeatRegisterPassword && registerPassword === repeatRegisterPassword) {
            return true;
        }
        return false;
    }
}