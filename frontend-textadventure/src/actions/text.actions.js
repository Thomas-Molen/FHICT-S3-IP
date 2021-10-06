import React from 'react';

import { textState } from '../state'
import { useRecoilState, useSetRecoilState } from 'recoil';


export { useUserActions }

function useUserActions() {
  const setText = useSetRecoilState(textState);

  return {
    setGlobalTextState,
  }


  function setGlobalTextState(newGlobalText) {
    setText(newGlobalText)
  }
}
