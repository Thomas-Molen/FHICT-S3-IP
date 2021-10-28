import './GameHomeComponent.css'
import { React, useState } from 'react';
import { Button } from 'react-bootstrap'
import { JWTState, userState } from '../../state';
import { useRecoilState } from 'recoil';

export function GameHomeComponent() {
    const [globalUserState] = useRecoilState(userState);
    const [globalJWTState] = useRecoilState(JWTState);
    const [isGettingAdventurers, setIsGettingAdventurers] = useState(true);
    const [isSelectingAdventurers, setIsSelectingAdventurers] = useState(false);
    const [adventurers, setAdventurers] = useState([]);

    if (!isSelectingAdventurers)
        return (
            <div className="gameBackground container-fluid">
                <div className="gameHeader">
                    <h1 className="gameHeaderText">START YOUR ADVENTURE</h1>
                </div>
                <div className="d-flex justify-content-center">
                    {globalUserState.user_id == null &&
                        <Button variant="light" className="gameButton" disabled><p className="gameButtonText">C:\ Start</p></Button>
                    }
                    {globalUserState.user_id != null &&
                        <Button variant="light" className="gameButton" onClick={() => GetAdventurers('https://backendtextadventure.azurewebsites.net/api/Adventurer/get')}><p className="gameButtonText">C:\ Start</p></Button>
                    }
                </div>
            </div>
        )

    function GetAdventurers(route) {
        setIsGettingAdventurers(true);
        fetch(route, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + globalJWTState,
            },
            credentials: "include",
        })
            .then(function (response) {
                if (!response.ok) {
                    setIsGettingAdventurers(false);
                    throw Error(response.statusText);
                }
                return response.json();
            })
            .then(data => {
                setAdventurers(data);
                console.log(data);
                setIsGettingAdventurers(false);
                setIsSelectingAdventurers(true);
            })
            .catch(error => {
                setIsGettingAdventurers(false);
                console.error('Error:', error);
            });

    }

    return (
        <div className="gameBackground container-fluid">
            <div className="gameHeader">
                <h1 className="gameHeaderText">SELECT YOUR CHARACTER</h1>
            </div>
            <div className="d-flex justify-content-center">
                <div className="GameCharacterSelection">
                    wdadwadwaadwdw
                    <div className="row">
                        <Button variant="Secondary" ><p className="gameButtonText">new adventure</p></Button>
                        <Button variant="Primary"  ><p className="gameButtonText">Resume</p></Button>
                    </div>
                </div>
            </div>
        </div>
    )
}