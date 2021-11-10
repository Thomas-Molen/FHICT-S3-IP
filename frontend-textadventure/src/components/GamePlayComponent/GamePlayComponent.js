import './GamePlayComponent.css'
import { React, useState } from 'react';
import { CreateAuthEntityManagerRequest } from '../../actions/APIConnectionHelper';
import { useRecoilValue, useRecoilState } from 'recoil';
import { JWTState, adventurerState, userState } from '../../state';
import { useAdventurerActions } from '../../actions'


export function GamePlayComponent() {
    const [globalJWTState] = useRecoilState(JWTState);
    const [globalAdventurerState] = useRecoilState(adventurerState);
    console.log(globalAdventurerState);
    const [Adventurer, setAdventurer] = useState({ id: null, experience: 0, health: 0, name: "Adventurer", damage: 0, positionX: 0, positionY: 0 });
    GetAdventurer(globalAdventurerState);

    if (Adventurer == undefined || Adventurer == null)
        return (
            <div className="gameBackground row-fluid">
                <div className="gameWarningHeader offset-1 col-5">
                    <div class="alert alert-danger" role="alert">
                        <h4 class="alert-heading">Watch Out!</h4>
                        <p>It seems no player data has been found!</p>
                        <hr />
                        <p class="mb-0">Try waiting a bit or refresh the page.</p>
                    </div>
                </div>
            </div>
        )

    return (
        <>
            <div className="gameBackground row-fluid">
                <div className="gameHeader offset-1 col-5">
                    Welcome {Adventurer.name}
                </div>
            </div>
        </>
    )

    function GetAdventurer(adventurerId) {
        if (Adventurer == undefined || Adventurer.id == null) {
            console.log(Adventurer)
            console.log(globalAdventurerState);
            
            
            // CreateAuthEntityManagerRequest('GET', 'Adventurer/get/' + _adventurerId)
            //     .then(data => {
            //         console.log(data);
            //         // setAdventurer(data);
            //     })
            //     .catch(error => {
            //         console.error('Error:', error);
            //     });
        }
    }
}