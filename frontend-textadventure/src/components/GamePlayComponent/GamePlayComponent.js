import './GamePlayComponent.css'
import { React, useState } from 'react';
import { CreateAuthEntityManagerRequest } from '../../actions/APIConnectionHelper';
import { useRecoilValue, useRecoilState } from 'recoil';
import { JWTState, adventurerState, userState } from '../../state';
import { useAdventurerState } from '../../actions';
import { Icon } from '@iconify/react';


export function GamePlayComponent() {
    const globalJWTState = useRecoilValue(JWTState);
    const globalAdventurerState = useRecoilValue(adventurerState);
    const globalAdventurerActions = useAdventurerState();
    GetAdventurer();

    return (
        <>
            <div className="gameBackground">
                <div className="gameHeader offset-1 col">
                    Welcome {globalAdventurerState.name}
                </div>
                <div className="row-fluid">
                    <div className="col-9">
                        <div className="offset-1 col-9">
                            <textarea className="gameConsole" readOnly />
                        </div>
                    </div>
                </div>
                <div className="row-fluid">
                    <div className="col-9">
                        <div className="offset-1 col-9">
                            <textarea className="gameInput" rows="1" onChange={(e) => GameInputOnChange(e)} onKeyPress={(e) => CheckForEnterKey(e)}>

                            </textarea>
                            <Icon icon="akar-icons:send" width="28" className="sendCommandIcon float-end EmptyConsoleInputIcon" onClick={(e) => ClickSendButton(e)} />
                        </div>
                    </div>
                </div>
            </div>
        </>
    )

    function GetAdventurer() {
        if (globalAdventurerState == undefined || globalAdventurerState.id == null) {
            CreateAuthEntityManagerRequest('GET', 'Adventurer/get/' + 1, globalJWTState)
                .then(data => {
                    console.log(data);
                    globalAdventurerActions.setGlobalAdventurerState(data);
                })
                .catch(error => {
                    console.error('Error:', error);
                });
        }
    }

    function GameInputOnChange(input) {
        const element = input.target.parentElement.childNodes[1];
        if (input.target.value == "") {
            RemoveClasses(element);
            element.classList.add("EmptyConsoleInputIcon");
        }
        else if (element.classList.contains("EmptyConsoleInputIcon")) {
            RemoveClasses(element);
            element.classList.add("ConsoleInputIcon");
        }
    }

    function RemoveClasses(object) {
        object.classList.remove("ConsoleInputIcon");
        object.classList.remove("EmptyConsoleInputIcon");
    }

    function CheckForEnterKey(input) {
        if (input.key == "Enter") {
            if (input.preventDefault) input.preventDefault();
            const element = input.target.parentElement.childNodes[1];

            RemoveClasses(element);
            element.classList.add("EmptyConsoleInputIcon");

            SendCommand(input.target.value);
            input.target.value = "";
        }
    }

    function ClickSendButton(button) {
        let svg = button.target;
        while (svg.tagName !== "svg") {
            svg = svg.parentElement;
        }
        SendCommand(svg.parentElement.childNodes[0].value);
        svg.parentElement.childNodes[0].value = "";
        RemoveClasses(svg);
        svg.classList.add("EmptyConsoleInputIcon");
    }

    function SendCommand(command) {
        AddToConsole(command);
    }

    function AddToConsole(message) {
        const _console = document.getElementsByClassName("gameConsole")
        if (message.length < 1) {
            return;
        }
        if (_console[0].value == "") {
            _console[0].value = message;
        }
        else {

            _console[0].value = _console[0].value + "\n" + message;
        }
    }
}