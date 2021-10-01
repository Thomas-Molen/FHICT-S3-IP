import React from 'react';
import { Route, Switch } from 'react-router-dom';
import * as Components from './index';
import { ExampleComponent } from './index';
import { useUserActions } from '../actions'
import { useRecoilState } from 'recoil';
import { textState } from '../state'

function FirstExampleComponent() {
    const [globalTextState] = useRecoilState(textState)
    const textActions = useUserActions()


    return (
        <div>
            <p>Component 1</p>
            <p>{globalTextState}</p>
            <input onChange={(e) => textActions.setGlobalTextState(e.target.value)} />
        </div>
    )
}

function SecondExampleComponent() {
    const [globalTextState] = useRecoilState(textState)


    return (
        <div>
            <p>Component 2</p>
            <p>{globalTextState}</p>
        </div>
    )
}

const Routes = () => {
    return (
        <main>
            <Switch>
                <Route exact path='/example' component={() => Components.ExampleComponent()} />
                <Route path='/' component={() =>
                    <div>
                        <FirstExampleComponent />
                        <SecondExampleComponent />
                    </div>} />
            </Switch>
        </main>
    )
}

export default Routes