import './LeaderboardComponent.css'
import React, { useEffect, useState } from 'react';
import { CreateRequest } from '../../actions/APIConnectionHelper';

export function LeaderboardComponent() {
    const [leaderboard, setLeaderboard] = useState([]);
    useEffect(() => {
        //GetLeaderboard();
    });
    

    return (
        <div className="leaderboardBackground container-fluid d-flex justify-content-center">
            <div className="leaderboardHeader">
                <h1 className="leaderboardHeaderText">Rankings</h1>
                <h2 className="leaderboardFooterText">Top 25 players</h2>
            </div>
            {leaderboard.map((adventurer, index) =>
            <>
            {console.log(adventurer)}
            </>
            )}
        </div>
    )

    function GetLeaderboard() {
        console.log(leaderboard)


            CreateRequest('GET', 'Adventurer/get-leaderboard')
                .then(data => {
                    setLeaderboard(data);
                    console.log(leaderboard);
                })
                .catch(error => {
                    setLeaderboard([]);
                    console.error('Error:', error);
                });
    }
}