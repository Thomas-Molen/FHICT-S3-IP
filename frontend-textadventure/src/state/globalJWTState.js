import React from 'react';
import { atom } from 'recoil';

const JWTState = atom({
    key: "JWTToken",
    default: "empty",
});

export { JWTState }