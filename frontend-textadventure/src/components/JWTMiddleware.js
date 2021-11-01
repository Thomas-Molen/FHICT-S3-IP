import React from 'react';
import { useRecoilState } from 'recoil';
import { JWTState } from '../state'
import { useJWTActions, useUserActions } from '../actions'
import jwt_decode from "jwt-decode";

export function JWTMiddleware() {
    const [globalJWTState] = useRecoilState(JWTState);
    const JWTActions = useJWTActions();

    const UserAtions = useUserActions();

    RefreshJWT();
    RenewExpiredJWT();

    async function RefreshJWT() {
        if (globalJWTState == "empty" || new Date() >= new Date(jwt_decode(globalJWTState).exp * 1000)) {
            await fetch('https://localhost:5101/api/User/renew-token', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
'Accept': 'application/json',
                },
                credentials: "include",
            })                    
                .then(function (response) {
                    if (!response.ok) {
                        throw Error(response.statusText);
                    }
                    return response.json();
                })
                .then(data => {
                    JWTActions.setGlobalJWTState(data.token);
                    UserAtions.setGlobalUserState({ user_id: data.id, username: data.username, email: data.email, admin: data.admin });
                })
                .catch(error => {
                    console.error('Error:', error);
                });
        }
    }

    async function RenewExpiredJWT() {
        if (globalJWTState !== "empty") {
            let currentTime = new Date();
            let expTime = new Date(jwt_decode(globalJWTState).exp * 1000);
            let timeDif = (expTime.getTime() - currentTime.getTime());

            await new Promise(r => setTimeout(r, timeDif));
            RefreshJWT();
        }
    }

    return (
        <>
        </>
    )
}