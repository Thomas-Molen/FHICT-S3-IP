import './GameHomeComponent.css'
import React from 'react';
import {Button} from 'react-bootstrap'

export function GameHomeComponent() {

    return (
        <div className="gameBackground d-flex align-items-center justify-content-center">
            <div className="gameHeader">
                <h1 className="gameHeaderText">START YOUR ADVENTURE</h1>
                <Button className="gameButton" variant="light" size="sm" href="#"><p className="gameButtonText">C:\ Start</p></Button>
            </div>
        </div>
    )
}