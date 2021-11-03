import './GameHomeComponent.css'
import { React, useState } from 'react';
import { Button } from 'react-bootstrap'
import { JWTState, userState } from '../../state';
import { useRecoilState } from 'recoil';
import { Icon } from '@iconify/react';
import { CreateAuthRequest } from '../../actions/APIConnectionHelper';

export function GameHomeComponent() {
    const [globalUserState] = useRecoilState(userState);
    const [globalJWTState] = useRecoilState(JWTState);
    const [isSelectingAdventurers, setIsSelectingAdventurers] = useState(false);
    const [adventurers, setAdventurers] = useState([]);
    const [selectedAdventurer, setSelectedAdventurer] = useState(null);
    const [settingAdventurerName, setSettingAdventurerName] = useState(false);

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
                        <Button variant="light" className="gameButton" onClick={() => GetAdventurers()}><p className="gameButtonText">C:\ Start</p></Button>
                    }
                </div>
            </div>
        )

    return (
        <div className="gameBackground container-fluid">
            <div className="gameHeader">
                <h1 className="gameHeaderText">SELECT YOUR CHARACTER</h1>
            </div>
            <div className="container d-flex justify-content-center">
                <div className="GameCharacterSelection">
                    {adventurers.map((adventurer, index) =>
                        <>
                            <div className="d-flex align-items-center justify-content-center">
                                <div className="adventurerOption align-items-center noselect" onClick={(selectedOption) => SelectAdventurer(selectedOption.target, index)}>
                                    <p className="adventurerOptionName noclick">{adventurer.name}</p>
                                    <div className="d-flex d-inline align-items-center justify-content-center noclick">
                                        <Icon icon="mdi:chevron-double-up" color="white" />{Math.floor(adventurer.experience / 100)}
                                        &nbsp;<Icon icon="mdi:cards-heart" color="white" />{adventurer.health}
                                        &nbsp;<Icon icon="mdi:sword" rotate={1} />14
                                    </div>
                                </div>
                                <Button variant="danger" className="adventurerDeleteButton" onClick={() => DeleteAdventurer(adventurer.id)}><Icon icon="bx:bxs-trash" color="white" width="24" /></Button>
                            </div>
                        </>
                    )}
                    <>
                        {settingAdventurerName &&
                            <div className="d-flex">
                                <Button variant="secondary" className="CreateAdventurerButton">
                                    <div className="d-flex align-items-center ">
                                        <Icon icon="icomoon-free:cross" className="closeCreateAdventure" color="white" width="18" onClick={() => setSettingAdventurerName(!settingAdventurerName)}/>
                                        <input type="text" className="form-control form-control-sm SelectionButtonText" placeholder="Adventurer" maxLength="20" />
                                    </div>
                                </Button>
                                <Button variant="primary" onClick={(selectedButton) => CreateAdventurer(selectedButton.target)}><p className="SelectionButtonText noclick">Create</p></Button>
                            </div>
                        }
                        {selectedAdventurer == null && !settingAdventurerName &&
                            <div className="d-flex">
                                <Button variant="secondary" className="CreateAdventurerButton" onClick={() => setSettingAdventurerName(!settingAdventurerName)}><p className="SelectionButtonText noclick">New adventure</p></Button>
                                <Button variant="primary" disabled><p className="SelectionButtonText">Resume</p></Button>
                            </div>
                        }
                        {selectedAdventurer != null && !settingAdventurerName &&
                            <div className="d-flex">
                                <Button variant="secondary" className="CreateAdventurerButton" onClick={() => setSettingAdventurerName(!settingAdventurerName)}><p className="SelectionButtonText noclick">New adventure</p></Button>
                                <Button variant="primary"><p className="SelectionButtonText">Resume</p></Button>
                            </div>
                        }
                    </>
                </div>
            </div>
        </div>
    )

    function SelectAdventurer(selectedOption, index) {

        for (let option of document.getElementsByClassName("adventurerOption")) {
            option.style.borderColor = "#ffffff";
        }

        if (selectedAdventurer == adventurers[index]) {
            setSelectedAdventurer(null);
            return;
        }
        setSelectedAdventurer(adventurers[index]);
        selectedOption.style.borderColor = "#11B6DA";
    }

    function GetAdventurers() {
        CreateAuthRequest('POST', 'Adventurer/get', globalJWTState)
            .then(data => {
                setAdventurers(data);
                setIsSelectingAdventurers(true);
            })
            .catch(error => {
                console.error('Error:', error);
            });
    }

    function DeleteAdventurer(adventurerId) {
        CreateAuthRequest('POST', 'Adventurer/delete', globalJWTState, { "adventurerId": adventurerId })
            .then(function () {
                GetAdventurers();
            })
            .catch(error => {
                console.error('Error:', error);
            });
    }

    function CreateAdventurer(selectedButton) {
        CreateAuthRequest('POST', 'Adventurer/create', globalJWTState, {"name": selectedButton.parentElement.querySelector('input').value})
            .then(function () {
                GetAdventurers();
            })
            .catch(error => {
                console.error('Error:', error);
            });

        setSettingAdventurerName(false);
    }
}