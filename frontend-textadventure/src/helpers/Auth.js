import { useRecoilState, useSetRecoilState } from 'recoil';
import { JWTAtom, userAtom } from '../state'
import jwt_decode from "jwt-decode";
import { UseFetchWrapper } from '../helpers';
import { useEffect } from 'react';

export function useAuthHook() {
    const fetchWrapper = UseFetchWrapper();
    const [JWTToken, setJWTToken] = useRecoilState(JWTAtom);
    const setUser = useSetRecoilState(userAtom)
    
    useEffect(() => {
        RefreshJWT();
        RenewExpiredJWT();
    }, [])
    
    async function RefreshJWT() {
        if (JWTToken == "empty" || new Date() >= new Date(jwt_decode(JWTToken).exp * 1000)) {
            fetchWrapper.post('User/renew-token')
                .then(data => {
                    setJWTToken(data.token);
                    setUser({ id: data.id, username: data.username, email: data.email, admin: data.admin });
                })
                .catch(error => {
                    console.error('Error:', error);
                });
        }
    }
    
    async function RenewExpiredJWT() {
        if (JWTToken !== "empty") {
            let currentTime = new Date();
            let expTime = new Date(jwt_decode(JWTToken).exp * 1000);
            let timeDif = (expTime.getTime() - currentTime.getTime());
    
            await new Promise(r => setTimeout(r, timeDif));
            RefreshJWT();
        }
    }
}
