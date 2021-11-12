import { useRecoilValue } from 'recoil';
import { JWTState } from '../state';

// Dev
// const baseGameRoute = "https://localhost:5101/api/" ;
// const baseEntityManagerRoute = "https://localhost:5201/api/" ;

// Production
const baseGameRoute = "https://backendtextadventure.azurewebsites.net/api/" ;
const baseEntityManagerRoute = "https://backendtextadventure-enitymanager.azurewebsites.net/api/" ;

export async function CreateGameRequest(_method, APICall, body = null) {
    return await CreateFetch(baseGameRoute, _method, APICall, body);
}

export async function CreateAuthGameRequest(_method, APICall, JWTToken, body = null) {
    return await CreateFetch(baseGameRoute, _method, APICall, body, true, JWTToken);
}

export async function CreateEntityManagerRequest(_method, APICall, body = null) {
    return await CreateFetch(baseEntityManagerRoute, _method, APICall, body);
}

export async function CreateAuthEntityManagerRequest(_method, APICall, JWTToken, body = null) {
    return await CreateFetch(baseEntityManagerRoute, _method, APICall, body, true, JWTToken);
}

async function CreateFetch(baseRoute , _method, APICall, body, isAuth = false, JWTToken = "") {
    const route = baseRoute + APICall;
    
    var _headers = {
        'Content-Type': 'application/json',
        'Accept': 'application/json',
    }
    if (isAuth)
    {
        _headers = {
            'Content-Type': 'application/json',
            'Accept': 'application/json',
            'Authorization': 'Bearer ' + JWTToken,
        }
    }
    
    if (body !== null)
    {
        return await fetch(route, {
            method: _method,
            headers: _headers,
            credentials: "include",
            body: JSON.stringify(body),
        })
        .then(function (response) {
            if (!response.ok) {
                throw Error(response.statusText);
            }
            return response.json();
        })
        .catch(error => {
            console.error('Error:', error);
        });
    }

    return fetch(route, {
        method: _method,
        headers: _headers,
        credentials: "include",
    })
    .then(function (response) {
        if (!response.ok) {
            throw Error(response.statusText);
        }
        return response.json();
    })
    .catch(error => {
        console.error('Error:', error);
    });
}
