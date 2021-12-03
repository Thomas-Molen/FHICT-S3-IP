import { atom } from 'recoil';

const JWTAtom = atom({
    key: "JWTToken",
    default: "empty",
});

export { JWTAtom }