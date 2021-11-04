import './LeaderboardComponent.css'
import React, { useEffect, useState } from 'react';
import { CreateRequest } from '../../actions/APIConnectionHelper';

export function LeaderboardComponent() {
    const [leaderboard, setLeaderboard] = useState(null);
    GetLeaderboard();
    
    return (
        <div className="leaderboardBackground container-fluid d-flex justify-content-center">
            <div className="leaderboardHeader">
                <h1 className="leaderboardHeaderText">Rankings</h1>
                <h2 className="leaderboardFooterText">Top 25 players</h2>
                {(leaderboard !== null) &&
                leaderboard.map((adventurer) =>
                <div key={adventurer.id}>
                    <p>{adventurer.user}</p>
                </div>
                )
            }
            </div>
            
        </div>
    )

    function GetLeaderboard() {
        console.log(leaderboard);
        if (leaderboard == null)
        {
            CreateRequest('GET', 'Adventurer/get-leaderboard')
            .then(data => {
                setLeaderboard(data);
                return data;
            })
            .catch(error => {
                setLeaderboard([]);
                console.error('Error:', error);
            });
        }
    }
}