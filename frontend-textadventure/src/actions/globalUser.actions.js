import { userState } from '../state';
import { useSetRecoilState } from 'recoil';


export { useUserActions }

function useUserActions() {
  const setUser = useSetRecoilState(userState);

  return {
    setGlobalUserState,
  }

  function setGlobalUserState(newGlobalUser) {
    setUser(newGlobalUser);
  }
}
