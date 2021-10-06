import React from 'react';
import { Route, Switch } from 'react-router-dom';
import * as Components from './index';
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
            <br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br />
            <br /><br /><br /><br /><br /><br /><br /><br />
            <p>hello</p>
        </div>
    )
}

const Routes = () => {
    return (
        <main>
            <Switch>
                <Route exact path='/game' component={() => Components.ExampleComponent()} />
                <Route path='/' component={() =>
                    <div>
                        <FirstExampleComponent />
                    </div>} />
            </Switch>
        </main>
    )
}

export default Routes