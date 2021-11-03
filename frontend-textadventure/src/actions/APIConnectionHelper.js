import { useRecoilValue } from 'recoil';
import { JWTState } from '../state';

const baseRoute = "https://localhost:5101/api/" ;

export async function CreateRequest(_method, APICall, body = null) {
    return await CreateFetch(_method, APICall, body);
}

export async function CreateAuthRequest(_method, APICall, JWTToken, body = null) {
    return await CreateFetch(_method, APICall, body, true, JWTToken);
}


// export async function CreateAuthPOSTRequest(APICall, body = null) {
//     // return await CreateAuthFetch('POST', APICall, body)
//     console.log('sup');
// }

// export async function CreateAuthGetRequest(APICall) {
//     return await CreateAuthFetch('GET', APICall, null)
// }

// export async function CreateAuthRequest(_method, APICall, body) {
//     return await CreateAuthFetch(_method, APICall, body)
// }

async function CreateFetch(_method, APICall, body, isAuth = false, JWTToken = "") {
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