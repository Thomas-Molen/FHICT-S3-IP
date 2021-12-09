import { Icon } from '@iconify/react';
import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { React, useEffect, useState } from 'react';
import { useLocation } from 'react-router-dom';
import ReactTooltip from 'react-tooltip';
import { useRecoilState, useRecoilValue } from 'recoil';
import { DrawingComponent } from '..';
import { UseCMDWrapper, UseConnectionHub } from '../../helpers';
import { EnemyAtom, ItemsAtom, JWTAtom, AdventurerAtom } from '../../state';
import './GamePlayComponent.css';

export function GamePlayComponent() {
    const JWTToken = useRecoilValue(JWTAtom);
    let URI = useLocation();
    const cmd = UseCMDWrapper();
    const hub = UseConnectionHub();

    //stats sidebar
    const [adventurer, setAdventurer] = useRecoilState(AdventurerAtom);
    const [enemy, setEnemy] = useRecoilState(EnemyAtom);
    const [items, setItems] = useRecoilState(ItemsAtom);
    const [selectedView, setSelectedView] = useState("stats");
    const [loadingInventory, setLoadingInventory] = useState(false);

    //input
    const [inputCommand, setInputCommand] = useState("");
    const [inputHistory, setInputHistory] = useState([]);
    const [inputHistoryIndex, setInputHistoryIndex] = useState(0);

    const [connection, setConnection] = useState(new HubConnectionBuilder()
        .withUrl(process.env.REACT_APP_GAME_MANAGER + "game", { accessTokenFactory: () => JWTToken })
        .configureLogging(LogLevel.Information)
        .withAutomaticReconnect()
        .build());

    useEffect(() => {
        hub.connect();
    }, [])

    return (
        <>
            <div className="gameBackground">
                <div className="col-12 col-lg-9">
                    <div className="gameHeader offset-1 col ">
                        Welcome {adventurer.name}
                    </div>
                </div>
                <div className="container-fluid">
                    <div className="row">
                        <div className="col-11 col-lg-8 ms-2 ms-lg-0">
                            <div className="offset-1 col">
                                <textarea className="Console" readOnly />
                            </div>
                            <div className="offset-1 col">
                                <textarea className="gameInput" rows="1" onChange={(e) => setInputCommand(e.target.value)} onKeyDown={(e) => CheckForSpecialKey(e)} autoFocus={true}></textarea>
                                <Icon icon="akar-icons:send" width="28" className={"sendCommandIcon float-end " + (inputCommand == "" ? "noclick" : "white pointer")} onClick={(e) => ClickSendButton(e)} />
                            </div>
                        </div>
                        <div className="col-12 col-lg-4 d-flex">
                            <div className="offset-lg-1 offset-1 col-10">
                                {/* stats UI */}
                                <div className="characterStats mb-5">
                                    {/* stats tabs */}
                                    <div className="row-fluid m-1">
                                        <a data-tip="Adventurer's stats">
                                            <Icon icon="ant-design:user-outlined" color="#585858" width="30" className={"statsoption stats " + (selectedView == "stats" ? "white noclick" : "")} onClick={(e) => setSelectedView("stats")} />
                                        </a>
                                        <a data-tip="Weapons">
                                            <Icon icon="mdi:treasure-chest" color="#585858" width="30" className={"statsoption inventory " + (selectedView == "inventory" ? "white noclick" : "")}onClick={(e) => setSelectedView("inventory")} />
                                        </a>
                                        <a data-tip="Enemy's stats">
                                            <Icon icon="mdi:sword-cross" color="#585858" width="30" className={"statsoption enemy " + (selectedView == "enemy" ? "white noclick" : "")} onClick={(e) => setSelectedView("enemy")} />
                                        </a>
                                        <ReactTooltip/>
                                    </div>
                                    <hr className="m-0" />
                                    <div className="statsInformationWindow">
                                        {/* Stats */}
                                        {selectedView == "stats" &&
                                            <div className="row">
                                                <div className="col-4">
                                                    <div className="d-flex align-items-center">
                                                        <div data-tip="Level">
                                                            <Icon icon="mdi:chevron-double-up" width="50" color="white" />{adventurer.experience / 10}
                                                        </div>
                                                    </div>
                                                    <div className="d-flex align-items-center">
                                                        <div data-tip="Health">
                                                            <Icon icon="akar-icons:heart" width="35" color="white" className="ms-2 me-2" />{adventurer.health}
                                                        </div>
                                                    </div>
                                                    <div className="d-flex align-items-center">
                                                        <div data-tip="Attack power from equipped weapon">
                                                            <Icon icon="mdi:sword" rotate={1} width="40" color="white" className="ms-1 me-1" />{adventurer.damage}
                                                        </div>
                                                    </div>
                                                </div>
                                                <div className="col">
                                                    <div className="d-flex align-items-center">
                                                        <div data-tip="Successfully cleared rooms">
                                                            <Icon icon="ic:baseline-meeting-room" width="50" color="white" className="me-1" />{adventurer.roomsCleared}
                                                        </div>
                                                        <ReactTooltip />
                                                    </div>
                                                </div>
                                            </div>
                                        }
                                        {/* Inventory */}
                                        {selectedView == "inventory" &&
                                            <div className={"row-fluid Inventory overflow-auto " + (loadingInventory ? "disabled" : "")}>
                                                {items.map((item) =>
                                                    <div key={item.id} className="d-flex align-items-center">
                                                        <div className={"ms-3 " + (item.equiped == true ? 'EquipedItem' : 'pointer')} onClick={() => EquipWeapon(item.id)}>
                                                            {item.name}
                                                            <a data-tip="Damage">
                                                                <Icon icon="mdi:sword" rotate={1} width="20" className="ms-3 me-0" />{item.attack}
                                                            </a>
                                                            <a data-tip="Durability">
                                                                <Icon icon="ps:broken-link" rotate={1} width="20" className="ms-3 me-0" />{item.durability}
                                                            </a>
                                                            <ReactTooltip />
                                                        </div>
                                                    </div>
                                                )}
                                            </div>
                                        }
                                        {/* Enemy */}
                                        {selectedView == "enemy" &&
                                            <>
                                                <div className="row">
                                                    <div className="col">
                                                        <div className="d-flex align-items-center">
                                                            <div data-tip="Difficulty">
                                                                <Icon icon={"" + (enemy.difficulty < 2 ? "fad:xlrplug" : (enemy.difficulty < 3 ? "ri:skull-line" : "ri:skull-2-line"))} width="50" color="white" className="me-1" />
                                                                <div className="d-inline">
                                                                    {enemy.name}
                                                                </div>
                                                            </div>
                                                            <div data-tip="Health">
                                                                <Icon icon="akar-icons:heart" width="35" color="white" className="ms-5 me-2" />{enemy.health}
                                                            </div>
                                                        </div>
                                                    </div>

                                                </div>
                                                <div className="row">
                                                    <div className="col">
                                                        <div className="d-flex align-items-center">
                                                            <div data-tip="Weapon">
                                                                <Icon icon="mdi:sword" rotate={1} width="40" color="white" className="ms-1 me-2" />{enemy.weapon}
                                                            </div>
                                                            <ReactTooltip />
                                                        </div>
                                                    </div>
                                                </div>
                                            </>
                                        }
                                    </div>
                                </div>
                                <DrawingComponent adventurerId={parseInt(URI.search.replace("?user=", ""))} />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </>
    )

    //input 
    function CheckForSpecialKey(input) {
        if (input.key == "ArrowUp") {
            input?.preventDefault();

            if (inputHistoryIndex > 0) {
                setInputHistoryIndex(inputHistoryIndex - 1);
                input.target.value = inputHistory[inputHistoryIndex - 1];
            }
        }
        else if (input.key == "ArrowDown") {
            input?.preventDefault();

            if (inputHistoryIndex < inputHistory.length - 1) {
                setInputHistoryIndex(inputHistoryIndex + 1);
                input.target.value = inputHistory[inputHistoryIndex + 1];
            }
            else {
                setInputHistoryIndex(inputHistory.length);
                input.target.value = "";
            }

        }
        else if (input.key == "Enter") {
            input?.preventDefault();
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
    }

    function SendCommand(command)
    {
        setInputCommand("");
        setInputHistoryIndex(inputHistory.length + 1);
        hub.sendCommand(command);
        setInputHistory(prevState => (
            [...prevState, command]
        ));
    }

    //stats window functionality
    async function EquipWeapon(weaponId) {
        setLoadingInventory(true);
        await hub.equipWeapon(weaponId)
        setLoadingInventory(false);
    }
}