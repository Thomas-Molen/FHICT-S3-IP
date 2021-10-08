import './IntroductionPageComponent.css'
import React from 'react';
import {Button} from 'react-bootstrap'
import Image from 'react-bootstrap/Image';

export function IntroductionPageComponent() {

    return (
        <div className="background d-flex align-items-center justify-content-center">
            <div>
                <div className="TAlogo"></div>
                <h1 className="TAtitle">Text Adventure</h1>
                <p className="TAannouncement">Welcome to the 0.1 launch. Sign in and play for free!</p>
                <Button className="TASignupButton" variant="dark" size="lg" href="#"><p className="TASignupText">SIGN IN</p></Button>
            </div>
            <div className="introductionField">
                <h1 className="introductionHeader">Begin your adventure today!</h1>
                <h2 className="introductionFooter">Explore &amp; Discover</h2>
                <p className="introductionText">
                    Our new cloud technology will allow you to resume your adventure any time, any where.<br />
                    Compete with other players on the official rankings or play for fun
                </p>
                <div>
                    <Button className="introductionSignupButton" variant="primary" size="lg" href="#"><b>SIGN UP</b></Button>
                    <Button variant="light" size="lg" href="#"><b>PLAY</b></Button>
                </div>
            </div>
        </div>
    )
}