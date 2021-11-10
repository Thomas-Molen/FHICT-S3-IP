import { adventurerState } from '../state';
import { useSetRecoilState } from 'recoil';


export { useAdventurerState }

function useAdventurerState() {
  const setAdventurer = useSetRecoilState(adventurerState);

  return {
    setGlobalAdventurerState,
    setGlobalAdventurerStateId,
  }

  function setGlobalAdventurerState(newGlobalAdventurer) {
    setAdventurer(newGlobalAdventurer);
  }

  function setGlobalAdventurerStateId(adventurerId) {
    setAdventurer({id: adventurerId});
  }
}
