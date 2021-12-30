import './FooterComponent.css'
import React from 'react';

export function FooterComponent() {

    return (
        <>
            <div className="container-fluid footerBackground d-flex justify-content-center align-items-center" id="footerComponent">
                <div className="row ">
                    <p className="copyrightText">&reg;TextAdventure 2021</p>
                </div>
            </div>
        </>
    )
}