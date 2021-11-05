import './IntroductionComponent.css'
import React, { useState } from 'react';
import { Button } from 'react-bootstrap'
import { SignUpComponent } from '..';
import { JWTState, userState } from '../../state';
import { useRecoilState } from 'recoil';

export function IntroductionComponent() {
    const [globalUserState] = useRecoilState(userState);
    const [isSignUpOpen, setIsSignUpOpen] = useState(false);
    return (
        <>
            <div className="introductionBackground container-fluid">
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
                            {!globalUserState.user_id &&
                                <>
                                    <button type="button" className="btn btn-dark btn-lg signUpButton" onClick={() => setIsSignUpOpen(!isSignUpOpen)}>SIGN IN</button>
                                    <div className="text-start">
                                        <SignUpComponent isOpen={isSignUpOpen} className="LoginComponent"></SignUpComponent>
                                    </div>
                                </>
                            }
                        </div>
                    </div>
                </div>
            </div>
            <div className="introductionContainer container-fluid">
                <div className="row">
                    <div className="offset-xl-1 col-xl-6 col-12 introductionInformation">
                        <h1 className="introductionHeader">Begin your adventure today!</h1>
                        <h2 className="introductionFooter">Explore &amp; Discover</h2>
                        <p className="introductionText">
                            Our new cloud technology will allow you to resume your adventure any time, any where.<br />
                            Compete with other players on the official rankings or play for fun!
                        </p>
                        {globalUserState.user_id == null &&
                            <>
                                <Button className="introductionSignInButton" variant="light" size="lg" onClick={() => setIsSignUpOpen(!isSignUpOpen)}><b>LOG IN</b></Button>
                                <Button className="introductionPlayButton disabled" variant="primary" size="lg"><b>PLAY</b></Button>
                            </>
                        }
                        {globalUserState.user_id != null &&
                            <Button className="introductionPlayButton" variant="primary" size="lg" onClick={() => window.scrollTo(0, 950)}><b>PLAY</b></Button>
                        }

                    </div>
                </div>
            </div>
        </>
    )
}