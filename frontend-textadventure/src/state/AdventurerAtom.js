import { atom } from 'recoil';

const AdventurerAtom = atom({
    key: "adventurerState",
    default: { id: null, experience: 0, health: 0, name: "Adventurer", damage: 0, roomsCleared: 0 },
});

export { AdventurerAtom }