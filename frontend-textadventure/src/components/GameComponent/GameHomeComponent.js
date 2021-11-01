import './GameHomeComponent.css'
import { React, useState } from 'react';
import { Button } from 'react-bootstrap'
import { JWTState, userState } from '../../state';
import { useRecoilState } from 'recoil';
import { Icon } from '@iconify/react';

export function GameHomeComponent() {
    const [globalUserState] = useRecoilState(userState);
    const [globalJWTState] = useRecoilState(JWTState);
    const [isSelectingAdventurers, setIsSelectingAdventurers] = useState(false);
    const [adventurers, setAdventurers] = useState([]);
    const [selectedAdventurer, setSelectedAdventurer] = useState(null);

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
                        <Button variant="light" className="gameButton" onClick={() => GetAdventurers('https://localhost:5101/api/Adventurer/get')}><p className="gameButtonText">C:\ Start</p></Button>
                    }
                </div>
            </div>
        )

    function GetAdventurers(route) {
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
                    throw Error(response.statusText);
                }
                return response.json();
            })
            .then(data => {
                setAdventurers(data);
                setIsSelectingAdventurers(true);
            })
            .catch(error => {
                console.error('Error:', error);
            });
    }

    return (
        <div className="gameBackground container-fluid">
            <div className="gameHeader">
                <h1 className="gameHeaderText">SELECT YOUR CHARACTER</h1>
            </div>
            <div className="container d-flex justify-content-center">
                <div className="GameCharacterSelection">
                    {adventurers.map((adventurer, index) =>
                        <div className="d-flex align-items-center justify-content-center">
                            <div className="adventurerOption d-flex align-items-center d-inline align-middle noselect" key={adventurer.id} onClick={(selectedOption) => SelectAdventurer(selectedOption.target, index)}>
                                <Icon icon="mdi:chevron-double-up" color="white" />{Math.floor(adventurer.experience / 100)}
                                &nbsp;<Icon icon="mdi:cards-heart" color="white" />{adventurer.health}
                                &nbsp;<Icon icon="mdi:sword" rotate={1} />14
                            </div>
                            <Button variant="danger" className="adventurerDeleteButton"><Icon icon="bx:bxs-trash" color="white" width="24" /></Button>
                        </div>
                    )}
                    <>
                        <Button variant="secondary"><p className="gameButtonText">new adventure</p></Button>
                        <Button variant="primary"><p className="gameButtonText">Resume</p></Button>
                    </>
                </div>
            </div>
        </div>
    )

    function SelectAdventurer(selectedOption, index) {

        for (let option of document.getElementsByClassName("adventurerOption")) {
            option.style.borderColor = "#ffffff";
        }

        if (selectedAdventurer == adventurers[index])
        {
            setSelectedAdventurer(null);
            return;
        }
        setSelectedAdventurer(adventurers[index]);

        while (selectedOption.tagName !== "DIV") {
            selectedOption = selectedOption.parentElement;
        }
        selectedOption.style.borderColor = "#11B6DA";
    }
}