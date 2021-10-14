import React from 'react';

import { userState } from '../state';
import { useRecoilState, useSetRecoilState } from 'recoil';


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
