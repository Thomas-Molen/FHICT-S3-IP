import { atom } from 'recoil';

const EnemyAtom = atom({
    key: "enemyState",
    default: { difficulty: 1, name: "Enemy", weapon: "Weapon", health: 0 },
});

export { EnemyAtom }