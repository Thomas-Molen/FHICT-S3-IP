import React from 'react';
import { Route, Switch } from 'react-router-dom';
import { useRecoilState } from 'recoil';
import { IntroductionComponent, SocialMediaComponent, GameHomeComponent, InformationComponent, LeaderboardComponent, FooterComponent, NavBarComponent, GamePlayComponent } from '../components';
import { userState } from '../state';

const Routes = () => {
    const [globalUserState] = useRecoilState(userState);
    return (
        <main>
            <Switch>
                {globalUserState.user_id != null &&
                <Route path='/game'>
                    <NavBarComponent />
                    <GamePlayComponent />
                </Route>
                }
                <Route path='/'>
                <NavBarComponent />
                    <IntroductionComponent />
                    <SocialMediaComponent />
                    <GameHomeComponent />
                    <InformationComponent />
                    <LeaderboardComponent />
                    <FooterComponent />
                </Route>
            </Switch>
        </main >
    )
}

export default Routes