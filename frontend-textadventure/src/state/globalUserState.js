import React from 'react';
import { atom } from 'recoil';

const userState = atom({
    key: "userState",
    default: {user_id: null, username: null, email: null, admin: null},
});

export { userState }