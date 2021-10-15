import './IntroductionComponent.css'
import React, { useReducer, useState } from 'react';
import { Button } from 'react-bootstrap'
import { SignUpComponent } from '..';
import { JWTState, userState } from '../../state';
import { useRecoilState } from 'recoil';

export function IntroductionComponent() {
    const [globalUserState] = useRecoilState(userState);
    const [globalJWTState] = useRecoilState(JWTState);
    const [isSignUpOpen, setIsSignUpOpen] = useState(false);
    return (
        <div className="introductionBackground d-flex align-items-center justify-content-center">
            <div className="TAbar">
                <a className="TAlogo" href="/"></a>
                <h1 className="TAtitle">Text Adventure</h1>
                <p className="TAannouncement">Welcome to the 0.1 launch. Sign in and play for free!</p>
                { globalUserState.user_id == null &&
                <>
                    <Button className="TASignupButton" variant="dark" size="lg" onClick={() => setIsSignUpOpen(!isSignUpOpen)}><p className="TASignupText">SIGN IN</p></Button>
                    <SignUpComponent isOpen={isSignUpOpen} className="LoginComponent"></SignUpComponent>
                </>
                }
            </div>
            <div className="introductionField">
                <h1 className="introductionHeader">Begin your adventure today!</h1>
                <h2 className="introductionFooter">Explore &amp; Discover</h2>
                <p className="introductionText">
                    Our new cloud technology will allow you to resume your adventure any time, any where.<br />
                    Compete with other players on the official rankings or play for fun!
                </p>
                <div>
                    <Button className="introductionSignupButton" variant="primary" size="lg" href="#"><b>SIGN UP</b></Button>
                    <Button variant="light" size="lg" href="#"><b>PLAY</b></Button>
                </div>
            </div>
        </div>
    )
}