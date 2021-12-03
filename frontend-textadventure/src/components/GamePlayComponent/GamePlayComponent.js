import './GamePlayComponent.css'
import { React, useEffect } from 'react';
import useState from 'react-usestateref';
import { useLocation, useHistory } from 'react-router-dom';
import { cmd } from '../../helpers';
import { useRecoilValue } from 'recoil';
import { JWTAtom } from '../../state';
import { Icon } from '@iconify/react';
import ReactTooltip from 'react-tooltip';
import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';

export function GamePlayComponent() {
    const JWTToken = useRecoilValue(JWTAtom);
    const [adventurer, setAdventurer, adventurerRef] = useState({ id: null, experience: 0, health: 0, name: "Adventurer", damage: 0, roomsCleared: 0 });
    const [enemy, setEnemy, enemyRef] = useState({ difficulty: 1, name: "Enemy", weapon: "Weapon", health: 0 });
    const [selectedView, setSelectedView] = useState("stats");
    const [items, setItems] = useState([]);
    const [loadingInventory, setLoadingInventory] = useState(false);


    let URI = useLocation();

    const [connection, setConnection] = useState(new HubConnectionBuilder()
        .withUrl("https://localhost:5101/game", { accessTokenFactory: () => JWTToken })
        .configureLogging(LogLevel.Information)
        .withAutomaticReconnect()
        .build());

    useEffect(() => {
        ConnectToHub();
    }, [])

    return (
        <>
            <div className="gameBackground">
                <div className="col-9">
                    <div className="gameHeader offset-1 col ">
                        Welcome {adventurer.name}
                    </div>
                </div>
                <div className="container-fluid">
                    <div className="row">
                        <div className="col-8">
                            <div className="offset-1 col">
                                <textarea className="gameConsole" readOnly />
                            </div>
                        </div>
                        <div className="col-4 d-flex">
                            <div className="offset-1 col-10">
                                {/* stats UI */}
                                <div className="characterStats mb-5">
                                    <div className="row-fluid statsOptionsBar">
                                        <Icon icon="ant-design:user-outlined" color="#585858" width="30" className="statsoption selectedStatOption stats" data-tip="Stats" onClick={(e) => SetStatsWindow("stats", e)} />
                                        <Icon icon="mdi:treasure-chest" color="#585858" width="30" className="statsoption inventory" data-tip="Inventory" onClick={(e) => SetStatsWindow("inventory", e)} />
                                        <Icon icon="mdi:sword-cross" color="#585858" width="30" className="statsoption enemy" data-tip="Enemy details" onClick={(e) => SetStatsWindow("enemy", e)} />
                                        <ReactTooltip />
                                    </div>
                                    <hr className="m-0" />
                                    <div className="statsInformationWindow">

                                        {selectedView == "stats" &&
                                            <div className="row">
                                                <div className="col-4">
                                                    <ReactTooltip />
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
                                                    </div>
                                                </div>
                                            </div>
                                        }
                                        {selectedView == "inventory" &&
                                            <div className={"row-fluid Inventory overflow-auto " + (loadingInventory ? "disabled" : "")}>
                                                <ReactTooltip />
                                                {items.map((item) =>
                                                    <div key={item.id} className="d-flex align-items-center">
                                                        <div className={"ms-3 " + (item.equiped == true ? 'EquipedItem' : 'pointer')} onClick={() => EquipWeapon(item.id)}>
                                                            {item.name}
                                                            <Icon icon="mdi:sword" rotate={1} width="20" className="ms-3 me-0" />{item.attack}
                                                            <Icon icon="ps:broken-link" rotate={1} width="20" className="ms-3 me-0" />{item.durability}
                                                        </div>
                                                    </div>
                                                )}
                                            </div>
                                        }
                                        {selectedView == "enemy" &&
                                            <>
                                                <div className="row">
                                                    <div className="col">
                                                        <ReactTooltip />
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
                                                        </div>
                                                    </div>
                                                </div>
                                            </>
                                        }
                                    </div>
                                </div>
                                {/* minimap */}
                                <div className="MiniMap characterStats d-flex" data-tip="Minimap">
                                </div>

                            </div>
                        </div>
                    </div>
                    <div className="row">
                        <div className="col-8">
                            <div className="offset-1 col">
                                <textarea className="gameInput" rows="1" onChange={(e) => GameInputOnChange(e)} onKeyPress={(e) => CheckForEnterKey(e)} autoFocus={true}>

                                </textarea>
                                <Icon icon="akar-icons:send" width="28" className="sendCommandIcon float-end EmptyConsoleInputIcon" onClick={(e) => ClickSendButton(e)} />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </>
    )

    //startup
    async function ConnectToHub() {
        if (connection.state == "Disconnected") {
            window.scrollTo(0, 0);
            cmd.DisplayMessage("Connecting to game servers...");
            try {
                await connection.start();
                cmd.Clear();

                //background setup
                connection.onreconnecting(() => {
                    cmd.DisplayMessage("Attempting to reconnect to the game server, \nthis might take a moment...");
                });

                connection.onreconnected(() => {
                    window.location.reload();
                });

                //invoke connection commands
                connection.on("ReceiveMessage", (message) => {
                    cmd.DisplayMessage(message);
                });

                connection.on("ClearConsole", () => {
                    cmd.Clear();
                });

                connection.on("UpdateWeapons", (items) => {
                    setItems(items);
                });

                connection.on("UpdateAdventurer", (adventurer) => {
                    setAdventurer(adventurer);
                });
                connection.on("UpdateAttack", (attack) => {
                    setAdventurer({ ...adventurerRef.current, damage: attack });
                });

                connection.on("UpdateEnemy", (enemy) => {
                    setEnemy(enemy);
                });
                connection.on("UpdateEnemyHealth", (health) => {
                    setAdventurer({ ...enemyRef.current, health: health });
                });
                await connection.invoke("Join", parseInt(URI.search.replace("?user=", "")));
            }
            catch (e) {
                cmd.Clear();
                cmd.DisplayMessage("Failed to connect to game server. \nPlease check your internet connection and the status of our servers.");
                console.log("Error: " + e);
            }
        }
    }

    //input styling
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

    //stats styling
    function SetStatsWindow(option, button) {
        let svg = button.target;
        while (svg.tagName !== "svg") {
            svg = svg.parentElement;
        }
        if (svg.classList.contains("selectedStatOption")) {
            return;
        }
        document.getElementsByClassName("selectedStatOption")[0].classList.remove("selectedStatOption");
        svg.classList.add("selectedStatOption");
        setSelectedView(option);
    }

    //game logic
    async function SendCommand(command) {
        await connection.invoke("SendCommand", command);
    }

    async function EquipWeapon(weaponId) {
        setLoadingInventory(true);
        await connection.invoke("EquipWeapon", weaponId)
        setLoadingInventory(false);
    }
}