import { useRecoilState, useSetRecoilState } from 'recoil';
import { JWTState, userState } from '../state'
import jwt_decode from "jwt-decode";
import { CreateEntityManagerRequest } from './APIConnectionHelper';

export function useAuthHook() {
    const [globalJWTState, setGlobalJWTState] = useRecoilState(JWTState);
    const setGlobalUserState = useSetRecoilState(userState)
    
    RefreshJWT();
    RenewExpiredJWT();
    
    async function RefreshJWT() {
        if (globalJWTState == "empty" || new Date() >= new Date(jwt_decode(globalJWTState).exp * 1000)) {
            await CreateEntityManagerRequest('POST', 'User/renew-token')
                .then(data => {
                    setGlobalJWTState(data.token);
                    setGlobalUserState({ user_id: data.id, username: data.username, email: data.email, admin: data.admin });
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
}
