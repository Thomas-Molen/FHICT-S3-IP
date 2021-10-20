import './LeaderboardComponent.css'
import React from 'react';

export function LeaderboardComponent() {

    return (
        <div className="leaderboardBackground container-fluid d-flex justify-content-center">
            <div className="leaderboardHeader">
                <h1 className="leaderboardHeaderText">Rankings</h1>
                <h2 className="leaderboardFooterText">Top 25 players</h2>
            </div>
        </div>
    )
}