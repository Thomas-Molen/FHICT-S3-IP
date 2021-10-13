import './SocialMediaComponent.css'
import React from 'react';
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faFacebookF, faGithub, faGoogle, faLinkedin, faTwitter } from "@fortawesome/free-brands-svg-icons";

export function SocialMediaComponent() {

    return (
        <div className="socialmediaBackground">
            <h1 className="socialmediaHeader">Find out what is happening</h1>
            <div className="socialmediaButtons d-flex align-items-center justify-content-center">
                    <a className="btn btn-outline-dark m-4 rounded-circle socialmediaButton" href="#" role="button">
                        <FontAwesomeIcon icon={faFacebookF} size="3x"/>
                    </a>

                    <a className="btn btn-outline-dark m-4 rounded-circle socialmediaButton" href="#" role="button">
                        <FontAwesomeIcon icon={faTwitter} size="3x"/>
                    </a>

                    <a className="btn btn-outline-dark m-4 rounded-circle socialmediaButton" href="#" role="button">
                        <FontAwesomeIcon icon={faGoogle} size="3x"/>
                    </a>

                    <a className="btn btn-outline-dark m-4 rounded-circle socialmediaButton" href="#" role="button">
                        <FontAwesomeIcon icon={faLinkedin} size="3x"/>
                    </a>

                    <a className="btn btn-outline-dark m-4 rounded-circle socialmediaButton" href="#" role="button">
                        <FontAwesomeIcon icon={faGithub} size="3x"/>
                    </a>
            </div>
        </div>
    )
}