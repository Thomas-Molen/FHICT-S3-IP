import { React } from 'react';
import { useSetRecoilState } from 'recoil';
import { userAtom } from '../../src/state';

describe("router rendering", () => {
    beforeEach(() => {
        cy.intercept('Get', '/api/Adventurer/get-leaderboard', {
            statusCode: 200,
            body: [],
        }).as("default get-leaderboard");

        cy.intercept('Post', '/api/User/renew-token', {
            statusCode: 400,
            body: {
                message: "renew token was denied"
            },
        }).as("default renew-token");
    })

    it("router renders container", () => {
        cy.visit("/");
        cy.get("#container").should("exist");
    });

    it("Home page loads correct components", () => {
        cy.visit("/");
        cy.get("#navBarComponent").should("exist");
        cy.get("#introductionComponent").should("exist");
        cy.get("#socialmediaComponent").should("exist");
        cy.get("#gameHomeComponent").should("exist");
        cy.get("#informationComponent").should("exist");
        cy.get("#leaderboardComponent").should("exist");
        cy.get("#footerComponent").should("exist");
    });

    it("cannot go to game page without being logged in", () => {
        cy.visit("/game");
        cy.get("#gameComponent").should("not.exist");
    });

    it("Gameplay page loads correct components when logged in", () => {
        cy.intercept('Post', '/api/User/renew-token', {
            statusCode: 200,
            body: {
                id: 1,
                username: "testingUsername",
                email: "testingEmail",
                admin: false,
                token: "testingJWTToken"
            },
        }).as("override renew-token");

        cy.visit("/game");
        cy.wait(2000);
        cy.get("#navBarComponent").should("exist");
        cy.get("#gameComponent").should("exist");
        cy.get("#footerComponent").should("exist");
        cy.wait(5000);
    });

    it("router renders container", () => {
        cy.visit("/");
        cy.get("#container").should("exist");
    });

    it("router renders container", () => {
        cy.visit("/");
        cy.get("#container").should("exist");
    });

    it("router renders container", () => {
        cy.visit("/");
        cy.get("#container").should("exist");
    });

    // it("logging in will allow access to game page", () => {
    //     // cy.intercept('Post', '/api/User/login', {
    //     //     statusCode: 200,
    //     //     body: {
    //     //         id: 1,
    //     //         username: "testingUsername",
    //     //         email: "testingEmail",
    //     //         admin: false,
    //     //         token: "testingJWTToken"
    //     //     },
    //     // }).as("override login");

    //     // cy.intercept('Post', '/api/User/renew-token', {
    //     //     statusCode: 200,
    //     //     body: {
    //     //         id: 1,
    //     //         username: "testingUsername",
    //     //         email: "testingEmail",
    //     //         admin: false,
    //     //         token: "testingJWTToken"
    //     //     },
    //     // }).as("override renew-token");

    //     // cy.intercept('GET', '/api/Adventurer/get', {
    //     //     statusCode: 200,
    //     //     body: [{
    //     //         name: "Thomas", 
    //     //         level: 5, 
    //     //         health: 84, 
    //     //         id: 1, 
    //     //         damage: 42
    //     //     }],
    //     // }).as("override adventurer/token");

    //     // cy.visit("/")
    //     // // cy.intercept('Post', '/api/User/login', {
    //     // //     statusCode: 200,
    //     // //     body: {
    //     // //         id: 1,
    //     // //         username: "testingUsername",
    //     // //         email: "testingEmail",
    //     // //         admin: false,
    //     // //         token: "testingJWTToken"
    //     // //     },
    //     // // });

    //     // // cy.visit("/");
    //     // // cy.get("#signInButton").click()
    //     // // cy.get('.signUpSubmitButton').click();



    //     // // cy.get("#introductionComponent")

    //     // // cy.intercept('Post', '/api/User/renew-token', {
    //     // //     statusCode: 200,
    //     // //     body: {
    //     // //         id: 1,
    //     // //         username: "testingUsername",
    //     // //         email: "testingEmail",
    //     // //         admin: false,
    //     // //         token: "testingJWTToken"
    //     // //     },
    //     // // });

    //     // // cy.visit("/game");
    //     // // cy.get("#gameComponent").should("exist");

    //     // // // cy.get("#consoleInput").type("help");
    //     // // // cy.get("consoleInput").type("{enter}");
    //     // // /* ==== Generated with Cypress Studio ==== */
    //     // // cy.get('#consoleInput').click();
    //     // // /* ==== End Cypress Studio ==== */
    // });
});

