import './IntroductionPageComponent.css'
import React from 'react';

import Image from 'react-bootstrap/Image';

export function IntroductionPageComponent() {

    return (
        <div className="fullscreen d-flex align-items-center justify-content-center">
            <div className="wrapper">
                <div className="TAlogo"></div>
                <h1 className="TAtitle">Text Adventure</h1>
            </div>
        </div>
    )
}