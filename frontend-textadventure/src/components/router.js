import React from 'react';
import { Route, Switch } from 'react-router-dom';
import {IntroductionComponent, SocialMediaComponent, GameHomeComponent, InformationComponent, LeaderboardComponent, FooterComponent} from '../components';

const Routes = () => {
    return (
        <main>
            <Switch>
                <Route path='/' component={() =>
                    <div>
                        <IntroductionComponent />
                        <SocialMediaComponent />
                        <GameHomeComponent />
                        <InformationComponent />
                        <LeaderboardComponent />
                        <FooterComponent />
                    </div>} />
            </Switch>
        </main>
    )
}

export default Routes