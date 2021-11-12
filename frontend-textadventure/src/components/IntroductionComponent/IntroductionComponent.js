import './IntroductionComponent.css'
import React from 'react';
import { Button } from 'react-bootstrap'
import { userState } from '../../state';
import { useRecoilState } from 'recoil';

export function IntroductionComponent() {
    const [globalUserState] = useRecoilState(userState);

    return (
        <>
            <div className="introductionBackground container-fluid">
                <div className="row">
                    <div className="offset-xl-1 col-xl-6 col-12 introductionInformation">
                        <h1 className="introductionHeader">Begin your adventure today!</h1>
                        <h2 className="introductionFooter">Explore &amp; Discover</h2>
                        <p className="introductionText">
                            Our new cloud technology will allow you to resume your adventure any time, any where.<br />
                            Compete with other players on the official rankings or play for fun!
                        </p>
                        {globalUserState.user_id == null ?
                            <Button className="introductionPlayButton disabled" variant="primary" size="lg"><b>PLAY</b></Button>
                        :
                            <Button className="introductionPlayButton" variant="primary" size="lg" onClick={() => window.scrollTo(0, 950)}><b>PLAY</b></Button>
                        }

                    </div>
                </div>
            </div>
        </>
    )
}