import { JWTState } from '../state';
import { useSetRecoilState } from 'recoil';


export { useJWTActions }

function useJWTActions() {
  const setJWT = useSetRecoilState(JWTState);

  return {
    setGlobalJWTState,
  }

  function setGlobalJWTState(newGlobalJWT) {
    setJWT(newGlobalJWT)
  }
}
