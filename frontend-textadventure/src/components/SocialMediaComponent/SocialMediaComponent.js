import './SocialMediaComponent.css'
import React from 'react';

export function SocialMediaComponent() {

    return (
        <div className="socialmediaBackground container-fluid">
            <h1 className="socialmediaHeader">Find out what is happening</h1>
            <div className="d-flex align-items-center justify-content-center">
                <button className="btn btn-outline-dark rounded-circle socialmediaButton desktopSocialMediaButton">
                    <i className="fa fa-facebook fa-4x"></i>        
                </button>
                <button className="btn btn-outline-dark rounded-circle socialmediaButton">
                    <i className="fa fa-twitter fa-4x"></i>        
                </button>
                <button className="btn btn-outline-dark rounded-circle socialmediaButton">
                    <i className="fa fa-instagram fa-4x"></i>        
                </button>
                <button className="btn btn-outline-dark rounded-circle socialmediaButton desktopSocialMediaButton">
                    <i className="fa fa-linkedin fa-4x"></i>        
                </button>
                <button className="btn btn-outline-dark rounded-circle socialmediaButton">
                    <i className="fa fa-github fa-4x"></i>        
                </button>
            </div>
        </div>
    )
}