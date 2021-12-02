import React from 'react';
import { Route, Switch } from 'react-router-dom';
import { useRecoilState } from 'recoil';
import { IntroductionComponent, SocialMediaComponent, GameHomeComponent, InformationComponent, LeaderboardComponent, FooterComponent, NavBarComponent, GamePlayComponent } from '../components';
import { userAtom } from '../state';

const Routes = () => {
    const [user] = useRecoilState(userAtom);
    return (
        <main>
            <Switch>
                {user.id != null &&
                <Route path='/game'>
                    <NavBarComponent />
                    <GamePlayComponent />
                    <FooterComponent />
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