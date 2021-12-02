import { atom } from 'recoil';

const userAtom = atom({
    key: "userState",
    default: { id: null, username: null, email: null, admin: null },
});

export { userAtom }