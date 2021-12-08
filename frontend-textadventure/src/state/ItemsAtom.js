import { atom } from 'recoil';

const ItemsAtom = atom({
    key: "itemsState",
    default: [],
});

export { ItemsAtom }