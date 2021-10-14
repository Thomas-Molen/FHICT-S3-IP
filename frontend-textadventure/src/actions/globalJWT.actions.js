import React from 'react';

import { JWTState } from '../state';
import { useRecoilState, useSetRecoilState } from 'recoil';


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
