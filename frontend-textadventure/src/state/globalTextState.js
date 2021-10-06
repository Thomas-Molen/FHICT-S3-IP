import React from 'react';
import {
  atom,
} from 'recoil';

const textState = atom({
    key: 'myTextState',
    default: "My default state"
})

export {textState}