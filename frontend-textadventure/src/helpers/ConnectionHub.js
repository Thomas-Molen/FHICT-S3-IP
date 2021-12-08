import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { useLocation } from 'react-router-dom';
import useState from 'react-usestateref';
import { useRecoilState, useRecoilValue } from 'recoil';
import { UseCMDWrapper } from '.';
import { AdventurerAtom, EnemyAtom, ItemsAtom, JWTAtom } from '../state';

export function UseConnectionHub() {
    const JWTToken = useRecoilValue(JWTAtom);
    const [connection, setConnection] = useState(new HubConnectionBuilder()
        .withUrl(process.env.REACT_APP_GAME_MANAGER + "game", { accessTokenFactory: () => JWTToken })
        .configureLogging(LogLevel.Information)
        .withAutomaticReconnect()
        .build());

    return {
        connect: ConnectToHub(connection),
        sendCommand: SendCommand(connection),
        equipWeapon: EquipWeapon(connection)
    }
    
    //startup
    function ConnectToHub(connection) {
        let URI = useLocation();
        const cmd = UseCMDWrapper();
        
        const [items, setItems] = useRecoilState(ItemsAtom);
        const [adventurer, setAdventurer] = useRecoilState(AdventurerAtom);
        const [enemy, setEnemy] = useRecoilState(EnemyAtom);
        return async () => {
            if (connection.state == "Disconnected") {
                window.scrollTo(0, 0);
                cmd.display("Connecting to game servers...");
                try {
                    await connection.start();
                    cmd.clear();
                    
                    //background setup
                    connection.onreconnecting(() => {
                        cmd.display("Attempting to reconnect to the game server, \nthis might take a moment...");
                    });
                    
                    connection.onreconnected(() => {
                        window.location.reload();
                    });
                    
                    //invoke connection commands
                    connection.on("ReceiveMessage", (message) => {
                        cmd.display(message);
                    });
                    
                    connection.on("ClearConsole", () => {
                        cmd.clear();
                    });
                    
                    connection.on("UpdateWeapons", (items) => {
                        setItems(items);
                    });
                    
                    connection.on("UpdateAdventurer", (adventurer) => {
                        setAdventurer(adventurer);
                    });
                    connection.on("UpdateAttack", (attack) => {
                        // setAdventurer({ ...adventurerRef.current, damage: attack });
                    });
                    connection.on("UpdateHealth", (health) => {
                        // setAdventurer({ ...adventurerRef.current, health: health });
                    });
                    connection.on("UpdateRoomsExplored", (rooms) => {
                        // setAdventurer({ ...adventurerRef.current, roomsCleared: rooms });
                    });
                    connection.on("UpdateExperience", (exp) => {
                        // setAdventurer({ ...adventurerRef.current, experience: exp });
                    });
                    
                    connection.on("UpdateEnemy", (enemy) => {
                        setEnemy(enemy);
                    });
                    connection.on("UpdateEnemyHealth", (health) => {
                        // setEnemy({ ...enemyRef.current, health: health });
                    });
                    
                    await connection.invoke("Join", parseInt(URI.search.replace("?user=", "")));
                }
                catch (e) {
                    cmd.clear();
                    cmd.display("Failed to connect to game server. \nPlease check your internet connection and the status of our servers.");
                    console.log("Error: " + e);
                }
            }
        }
    }
    
    //game logic
    function SendCommand(connection) {
        return async (command) => {
            if (connection.state != "Disconnected") {
                await connection.invoke("SendCommand", command);
            }
        }
    }
    
    function EquipWeapon(connection) {
        return async (weaponId) => {
            if (connection.state != "Disconnected") {
                await connection.invoke("EquipWeapon", weaponId)
            }
        }
    }
};