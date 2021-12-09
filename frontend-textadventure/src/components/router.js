import React from 'react';
import { Sugar } from 'react-preloaders2';
import { Route, Switch } from 'react-router-dom';
import { useRecoilValue } from 'recoil';
import { FooterComponent, GameHomeComponent, GamePlayComponent, InformationComponent, IntroductionComponent, LeaderboardComponent, NavBarComponent, SocialMediaComponent } from '../components';
import { PreLoaderAtom, userAtom } from '../state';

const Routes = () => {
    const user = useRecoilValue(userAtom);
    
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