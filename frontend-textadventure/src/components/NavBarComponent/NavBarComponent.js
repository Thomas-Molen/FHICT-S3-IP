import { Icon } from '@iconify/react';
import React, { useState } from 'react';
import { useRecoilValue } from 'recoil';
import { SignUpComponent } from '..';
import { userAtom } from '../../state';
import './NavBarComponent.css';
import { UseFetchWrapper } from '../../helpers';

export function NavBarComponent() {
    const user = useRecoilValue(userAtom);
    const fetchWrapper = UseFetchWrapper();
    const [isSignUpOpen, setIsSignUpOpen] = useState(false);
    const [isLoggingOut, setIsLoggingOut] = useState(false);
    return (
        <>
            <div className="navbarBackground container-fluid">
                <div className="row">
                    <div className="col-xl-6 col-sm-8 col-6 row">
                        <div className="col-10 col-sm-2 col-xl-1">
                            <a className="TAlogo" href="/"></a>
                        </div>
                        <div className="col-0 col-md-10 col-xl-11">
                            <h1 className="TAtitle">Text Adventure</h1>
                        </div>
                    </div>
                    <div className="col-xl-6 col-sm-4 col-6 row">
                        <div className="col-xl-9 TAannouncementCol">
                            <p className="TAannouncement">Welcome to the 0.1 launch. Sign in and play for free!</p>
                        </div>
                        <div className="col-xl-3 col-12 text-end">
                            {user.id == null ?
                                <>
                                    <button type="button" className="btn btn-dark btn-lg signUpButton" onClick={() => setIsSignUpOpen(!isSignUpOpen)}>SIGN IN</button>
                                    <div className="text-start">
                                        <SignUpComponent isOpen={isSignUpOpen} className="LoginComponent"></SignUpComponent>
                                    </div>
                                </>
                                :
                                <>
                                    <Icon icon="si-glyph:sign-out" color="black" width="50" className={"signOutButton pointer float-end " + (isLoggingOut ? "disabled" : "")}
                                        onClick={async () => {
                                            setIsLoggingOut(true);
                                            fetchWrapper.post("User/deactivate-token")
                                                .then(() => {
                                                    setIsLoggingOut(false);
                                                })
                                                .catch(() => {
                                                    setIsLoggingOut(false);
                                                })
                                            window.location.reload(false);
                                        }} />
                                </>
                            }
                        </div>
                    </div>
                </div>
            </div>
        </>
    )
}