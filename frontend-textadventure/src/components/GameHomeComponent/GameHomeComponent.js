import './GameHomeComponent.css'
import { React, useState } from 'react';
import { useHistory } from 'react-router-dom';
import { Button } from 'react-bootstrap'
import { JWTAtom, userAtom } from '../../state';
import { useRecoilValue } from 'recoil';
import { Icon } from '@iconify/react';
import { UseFetchWrapper } from '../../helpers';

export function GameHomeComponent() {
    const user = useRecoilValue(userAtom);
    const JWTToken = useRecoilValue(JWTAtom);
    const fetchWrapper = UseFetchWrapper();

    const [isSelectingAdventurers, setIsSelectingAdventurers] = useState(false);
    const [adventurers, setAdventurers] = useState([]);
    const [selectedAdventurer, setSelectedAdventurer] = useState(null);
    const [settingAdventurerName, setSettingAdventurerName] = useState(false);
    const [creatingAdventurer, setCreatingAdventurer] = useState(false);
    let history = useHistory();

    if (!isSelectingAdventurers)
        return (
            <div className="gameBackground container-fluid">
                <div className="gameHomeHeader">
                    <h1 className="gameHomeHeaderText">START YOUR ADVENTURE</h1>
                </div>
                <div className="d-flex justify-content-center">
                    {user.id == null ?
                        <Button variant="light" className="gameHomeButton" disabled><p className="mb-0">C:\ Start</p></Button>
                        :
                        <Button variant="light" className="gameHomeButton" onClick={() => GetAdventurers()}><p className="mb-0">C:\ Start</p></Button>
                    }
                </div>
            </div>
        )

    return (
        <div className="gameBackground container-fluid">
            <div className="gameHomeHeader">
                <h1 className="gameHomeHeaderText">SELECT YOUR CHARACTER</h1>
            </div>
            <div className="container d-flex justify-content-center">
                <div className="GameCharacterSelection d-flex flex-wrap justify-content-center">
                    {adventurers.map((adventurer) =>
                        <div className="d-flex align-items-center justify-content-center" key={adventurer.id} onClick={(selectedOption) => SelectAdventurer(selectedOption.target, adventurer.id)}>
                            <div className="adventurerOption align-items-center noselect">
                                <div className="d-flex justify-content-between">
                                    {adventurer.health < 1 ?
                                        <p className="adventurerOptionName text-start" ><del>{adventurer.name}</del></p>
                                    :
                                    <p className="adventurerOptionName text-start" >{adventurer.name}</p>
                                    }
                                    <Button variant="danger" className="adventurerDeleteButton" onClick={(selectedButton) => DeleteAdventurer(selectedButton.target, adventurer.id)}><Icon icon="bx:bxs-trash" className="noclick" color="white" width="24" /></Button>
                                </div>
                                <div className="d-flex d-inline align-items-center justify-content-center noclick">
                                    <Icon icon="mdi:chevron-double-up" color="white" />{adventurer.level}
                                    &nbsp;<Icon icon="akar-icons:heart" width="27" color="white" />{adventurer.health}
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
                                    <Button variant="primary" size="lg" onClick={() => ResumeGame()}><p className="SelectionButtonText noclick">Resume</p></Button>
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
    function SelectAdventurer(selectedOption, adventurerId) {

        if (selectedOption.nodeName === "BUTTON") {
            return;
        }

        for (let option of document.getElementsByClassName("adventurerOption")) {
            option.style.borderColor = "#ffffff";
        }

        if (selectedAdventurer === adventurers.find(a => a.id === adventurerId)) {
            setSelectedAdventurer(null);
            return;
        }
        setSelectedAdventurer(adventurers.find(a => a.id === adventurerId));

        while (!selectedOption.classList.contains('adventurerOption')) {
            selectedOption = selectedOption.parentElement;
        }
        selectedOption.style.borderColor = "#11B6DA";
    }

    function GetAdventurers() {
        fetchWrapper.get('Adventurer/get')
            .then((data) => {
                setAdventurers(data);
                setIsSelectingAdventurers(true);
            })
            .catch(error => {
                console.error('Error:', error);
            });
    }

    function DeleteAdventurer(selectedButton, adventurerId) {
        if (!window.confirm('Are you sure you want to delete ' + adventurers.find(a => a.id == adventurerId).name + '\nthis action is permanent')){
            return;
        }

        if (selectedAdventurer != null && selectedAdventurer.id == adventurerId) {
            setSelectedAdventurer(null);
        }

        while (!selectedButton.classList.contains('adventurerOption')) {
            selectedButton = selectedButton.parentElement;
        }
        selectedButton.remove();
        fetchWrapper.delete('Adventurer/delete', { "adventurerId": adventurerId })
            .then(() => {
                GetAdventurers();
            })
            .catch(error => {
                console.error('Error:', error);
            });
    }

    function CreateAdventurer(selectedButton) {
        setCreatingAdventurer(true);
        fetchWrapper.post('Adventurer/create', { name: selectedButton.parentElement.querySelector('input').value })
            .then(() => {
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

    function ResumeGame() {
        history.push("/game?user=" + selectedAdventurer.id)
    }
}