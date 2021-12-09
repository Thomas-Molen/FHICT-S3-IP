import { atom } from 'recoil';

const PreLoaderAtom = atom({
    key: "preloaderState",
    default: true,
});

export { PreLoaderAtom }