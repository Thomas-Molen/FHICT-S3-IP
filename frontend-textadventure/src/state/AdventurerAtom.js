import { atom } from 'recoil';

const AdventurerAtom = atom({
    key: "adventurerState",
    default: { id: null, experience: "-", health: "-", name: "", damage: "-", roomsCleared: "-" },
});

export { AdventurerAtom }