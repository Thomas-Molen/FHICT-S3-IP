import './LeaderboardComponent.css'
import React, { useEffect, useState } from 'react';
import { UseFetchWrapper } from '../../helpers';

export function LeaderboardComponent() {
    const fetchWrapper = UseFetchWrapper();
    const [leaderboard, setLeaderboard] = useState(null);

    useEffect(() => {
        if (leaderboard == null) {
            fetchWrapper.get('Adventurer/get-leaderboard')
                .then(data => {
                    setLeaderboard(data);
                    return data;
                })
                .catch(error => {
                    setLeaderboard([]);
                    console.error('Error:', error);
                });
        }
    }, [])

    return (
        <div className="leaderboardBackground">
            <div className="leaderboardHeader container-fluid justify-content-center">
                <h1 className="leaderboardHeaderText">Rankings</h1>
                <h2 className="leaderboardFooterText">Top 25 players</h2>
            </div>
            <div className="container justify-content-center d-flex align-items-center">
                <div className="table-responsive LeaderboardTable">
                    <table className="table table-striped table-dark">
                        <thead>
                            <tr>
                                <th>#</th>
                                <th>User</th>
                                <th>Adventurer</th>
                                <th>Level</th>
                                <th>Rooms</th>
                                <th>Health</th>
                                <th>Damage</th>
                            </tr>
                        </thead>
                        <tbody>
                            {(leaderboard !== null && leaderboard !== [] && leaderboard !== undefined) &&
                                leaderboard.map((adventurer) =>
                                    <tr key={adventurer.position}>
                                        <th>{adventurer.position}</th>
                                        <td>{adventurer.user}</td>
                                        <td>{adventurer.adventurer}</td>
                                        <td>{adventurer.level}</td>
                                        <td>{adventurer.rooms}</td>
                                        <td>{adventurer.health}</td>
                                        <td>{adventurer.damage}</td>
                                    </tr>
                                )
                            }
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    )
}