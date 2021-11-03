import { useRecoilValue, useSetRecoilState } from 'recoil';
import { JWTState } from '../state'
import { useJWTActions, useUserActions } from '../actions'
import jwt_decode from "jwt-decode";
import { CreateRequest } from './APIConnectionHelper';

export default function useAuthHook() {
    const globalJWTState = useRecoilValue(JWTState);
    const setGlobalJWTState = useSetRecoilState(JWTState);

    const JWTActions = useJWTActions();
    
    const UserAtions = useUserActions();
    
    RefreshJWT();
    RenewExpiredJWT();
    
    async function RefreshJWT() {
        if (globalJWTState == "empty" || new Date() >= new Date(jwt_decode(globalJWTState).exp * 1000)) {
            await CreateRequest('POST', 'User/renew-token')
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
}
