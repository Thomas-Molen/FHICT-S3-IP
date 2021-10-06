import logo from '../../logo.svg';
import './ExampleComponent.css'
import React, { useState, useEffect } from 'react';
import { useRecoilState } from 'recoil';
import {textState} from '../../state'

import {useUserActions} from '../../actions'

export function ExampleComponent() {
  const [globalTextState] = useRecoilState(textState)

  const textActions = useUserActions()



  
  // On component load, OR component destroy
  useEffect(() => {

    // On component render
    // We make some ASYNC call here // We wait for response
    // Meanwhile component unloaded

  })

  return (
    <div className="App">
      <header className="App-header">
        <img src={logo} className="App-logo" alt="logo" />
        <p className="example-text">
          Example component, have fun.
        </p>
        <p>{globalTextState}</p>
        <button onClick={() => {
          textActions.setGlobalTextState("My NEW TEXT")
        }}>CLick me to change global state for all components :)</button>
        {/* <input onChange={(theInputField) => setText(theInputField.target.value)}></input> */}
      </header>
    </div>
  )
}