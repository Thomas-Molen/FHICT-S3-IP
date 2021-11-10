import { adventurerState } from '../state';
import { useSetRecoilState } from 'recoil';


export { useAdventurerState }

function useAdventurerState() {
  const setAdventurer = useSetRecoilState(adventurerState);

  return {
    setGlobalAdventurerState,
  }

  function setGlobalAdventurerState(newGlobalAdventurer) {
    setAdventurer(newGlobalAdventurer);
  }
}
