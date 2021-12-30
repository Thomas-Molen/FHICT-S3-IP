import './InformationComponent.css'
import React from 'react';

export function InformationComponent() {

    return (
        <>
            <div className="informationBackground container-fluid" id="informationComponent">
                <div className="row informationRow">
                    <div className="col-0 col-xl-4 offset-xl-1">
                        <div className="aboutInformation">
                            <h1 className="InformationHeader">What is this game?</h1>
                            <h2 className="InformationFooter">Text-based Adventure</h2>
                            <p className="InformationText">Our game is aimed towards all players who liked text-adventure games in the past, or might want a more next-gen experience of an old classic.<br /><br />

                                Our game is aimed towards delivering an unique experience to each player while still being able to compete against other players.
                            </p>
                        </div>
                    </div>
                    <div className="col-xl-4 offset-xl-2 col-0">
                        <div className="contactInformation">
                            <h1 className="InformationHeader">Contact us!</h1>
                            <h2 className="InformationFooter">Dev team</h2>
                            <p className="InformationText">We are more than happy to hear any feedback you might have on our game, or if you just want to chat!<br /><br />

                                You can contact us via our social media’s or send us a support message via our support email.<br />
                                <a href="mailto: TextAdventure@support.com">TextAdventure@support.com</a>
                            </p>
                        </div>
                    </div>
                </div>
            </div>
        </>
    )
}