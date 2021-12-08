import { useRecoilValue } from "recoil";
import { JWTAtom, userAtom } from "../state";

export function UseFetchWrapper() {
    const JWTToken = useRecoilValue(JWTAtom);
    const user = useRecoilValue(userAtom);
    return {
        get: request('GET'),
        post: request('POST'),
        put: request('PUT'),
        delete: request('DELETE')
    }
    
    function request(method) {
        return (url, body) => {
            const requestOptions = {
                method: method,
                headers: GetHeader(),
                credentials: 'include'
            };
            if (body)
            {
                requestOptions.headers['Content-Type'] = 'application/json';
                requestOptions.body = JSON.stringify(body);
            }
            return fetch(process.env.REACT_APP_ENTITY_MANAGER + url, requestOptions).then(HandleResponse);
        }
    }
    
    // helper functions
    function GetHeader() {
        
        if (user.id != null) {
            return { Authorization: `Bearer ${JWTToken}` };
        }
        else {
            return {};
        }
    }
    
    function HandleResponse(response) {
        return response.text().then(text => {
            const data = text && JSON.parse(text);
            
            if (!response.ok) {
                if ([401, 403].includes(response.status)) {
                    const error = "Access Denied";
                    return Promise.reject(error);
                }

                const error = (data && data.message) || response.statusText;
                return Promise.reject(error);
            }
            
            return data;
        });
    }
};