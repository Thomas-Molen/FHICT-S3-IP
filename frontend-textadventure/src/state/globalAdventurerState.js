import { atom } from 'recoil';

const adventurerState = atom({
    key: "adventurerState",
    default: null,
});

export { adventurerState }