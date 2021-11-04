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
    const [creatingAdventurer, setCreatingAdventurer] = useState(false);

    if (!isSelectingAdventurers)
        return (
            <div className="gameBackground container-fluid">
                <div className="gameHeader">
                    <h1 className="gameHeaderText">START YOUR ADVENTURE</h1>
                </div>
                <div className="d-flex justify-content-center">
                    {globalUserState.user_id == null ?
                        <Button variant="light" className="gameButton" disabled><p className="gameButtonText">C:\ Start</p></Button>
                        :
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
                <div className="GameCharacterSelection d-flex flex-wrap justify-content-center">
                    {adventurers.map((adventurer, index) =>
                        <div className="d-flex align-items-center justify-content-center">
                            <div className="adventurerOption align-items-center noselect" onClick={(selectedOption) => SelectAdventurer(selectedOption.target, index)}>
                                <div className="d-flex justify-content-between">
                                    <p className="adventurerOptionName noclick text-start">{adventurer.name}</p>
                                    <Button variant="danger" className="adventurerDeleteButton" onClick={(selectedButton) => DeleteAdventurer(selectedButton.target, adventurer.id)}><Icon icon="bx:bxs-trash" className="noclick" color="white" width="24" /></Button>
                                </div>
                                <div className="d-flex d-inline align-items-center justify-content-center noclick">
                                    <Icon icon="mdi:chevron-double-up" color="white" />{adventurer.level}
                                    &nbsp;<Icon icon="mdi:cards-heart" color="white" />{adventurer.health}
                                    &nbsp;<Icon icon="mdi:sword" rotate={1} />{adventurer.damage}
                                </div>
                            </div>
                        </div>
                    )}
                    
                    <div className="d-flex col-12 justify-content-center SelectAdventurersButtonOptions">
                    {!settingAdventurerName ?
                        (selectedAdventurer == null) ?
                            <>
                                <Button variant="secondary" className="CreateAdventurerButton" size="lg" onClick={() => setSettingAdventurerName(!settingAdventurerName)}><p className="SelectionButtonText noclick">New adventure</p></Button>
                                <Button variant="primary" disabled size="lg"><p className="SelectionButtonText">Resume</p></Button>
                            </>
                            :
                            <>
                                <Button variant="secondary" className="CreateAdventurerButton" size="lg" onClick={() => setSettingAdventurerName(!settingAdventurerName)}><p className="SelectionButtonText noclick">New adventure</p></Button>
                                <Button variant="primary" size="lg" onClick={() => TestingFunction()}><p className="SelectionButtonText noclick">Resume</p></Button>
                            </>
                        :
                        <>
                            <Button variant="secondary" className="CreateAdventurerButton" size="lg">
                                <div className="d-flex align-items-center">
                                    <Icon icon="icomoon-free:cross" className="closeCreateAdventure" color="white" width="18" onClick={() => setSettingAdventurerName(!settingAdventurerName)} />
                                    <input type="text" className="form-control form-control-sm SelectionButtonText" placeholder="Adventurer" maxLength="20" />
                                </div>
                            </Button>
                            {!creatingAdventurer ?
                                <Button variant="primary" size="lg" onClick={(selectedButton) => CreateAdventurer(selectedButton.target)}><p className="SelectionButtonText noclick">Create</p></Button>
                                :
                                <Button variant="primary" className="d-flex align-items-center" disabled size="lg">
                                    <span className="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>
                                    Loading...
                                </Button>
                            }
                        </>
                    }
                    </div>
                </div>
            </div>
        </div>
    )
    function SelectAdventurer(selectedOption, index) {
        console.log('got in the select', index);
        console.log(selectedOption);

        for (let option of document.getElementsByClassName("adventurerOption")) {
            option.style.borderColor = "#ffffff";
        }

        if (selectedAdventurer == adventurers[index]) {
            setSelectedAdventurer(null);
            return;
        }
        setSelectedAdventurer(adventurers[index]);

        while (!selectedOption.classList.contains('adventurerOption'))
        {
            selectedOption = selectedOption.parentElement;
        }
        selectedOption.style.borderColor = "#11B6DA";
    }

    function GetAdventurers() {
        CreateAuthRequest('GET', 'Adventurer/get', globalJWTState)
            .then(data => {
                setAdventurers(data);
                setIsSelectingAdventurers(true);
            })
            .catch(error => {
                console.error('Error:', error);
            });
    }

    function ClearSelection(adventurerId) {
        console.log('got in the clear');

        for (let option of document.getElementsByClassName("adventurerOption")) {
            option.style.borderColor = "#ffffff";
        }

        if (selectedAdventurer != null && selectedAdventurer.id == adventurerId) {
            setSelectedAdventurer(null);
            return;
        }
    }

    function DeleteAdventurer(selectedButton, adventurerId) {
        console.log('got in delete');    
        while (!selectedButton.classList.contains('adventurerOption'))
        {
            selectedButton = selectedButton.parentElement;
        }
        selectedButton.disabled = true;
        console.log(selectedButton.parentElement);
        selectedButton.remove();
        ClearSelection(adventurerId);

        CreateAuthRequest('POST', 'Adventurer/delete', globalJWTState, { "adventurerId": adventurerId })
            .then(function () {
                GetAdventurers();
            })
            .catch(error => {
                console.error('Error:', error);
            });
    }

    function CreateAdventurer(selectedButton) {
        setCreatingAdventurer(true);
        CreateAuthRequest('POST', 'Adventurer/create', globalJWTState, { "name": selectedButton.parentElement.querySelector('input').value })
            .then(function () {
                GetAdventurers();
                setSettingAdventurerName(false);
                setCreatingAdventurer(false);
            })
            .catch(error => {
                setSettingAdventurerName(false);
                setCreatingAdventurer(false);
                console.error('Error:', error);
            });


    }

    function TestingFunction()
    {
        console.log(selectedAdventurer);
    }
}