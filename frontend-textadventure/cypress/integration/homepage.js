describe("home page", () => {
    beforeEach(() => {
        cy.intercept('Get', '**/get-leaderboard', {
            statusCode: 200,
            body: [],
        }).as("default get-leaderboard");

        cy.intercept('Post', '**/renew-token', {
            statusCode: 400,
            body: {
                message: "renew token was denied"
            },
        }).as("default renew-token");

        cy.visit("/");
    })

    it("Logo brings user to homepage", () => {
        cy.visit("/someurlthatdoesntexist");
        cy.get(".TAlogo").click();
        cy.location('pathname').should('eq', '/');
    });

    it("when not logged in play button on introduction should be disabled", () => {
        cy.get(".introductionPlayButton").should('have.class', 'disabled');
    });

    it("when logged in play button on introduction should bring player to play area", () => {
        cy.intercept('Post', '**/renew-token', {
            statusCode: 200,
            body: {
                id: 1,
                username: "testingUsername",
                email: "testingEmail",
                admin: false,
                token: "testingJWTToken"
            },
        }).as("override renew-token");
        cy.visit("/");

        cy.get(".introductionPlayButton").click();
        cy.window().its('scrollY').should('eq', 950)
    });

    it("when not logged in start button on game area should be disabled", () => {
        cy.get(".gameHomeButton").should('have.class', 'disabled');
    });

    it("when logged in start button on game area should not be disabled", () => {
        cy.intercept('Post', '**/renew-token', {
            statusCode: 200,
            body: {
                id: 1,
                username: "testingUsername",
                email: "testingEmail",
                admin: false,
                token: "testingJWTToken"
            },
        }).as("override renew-token");
        cy.visit("/");
        
        cy.get(".gameHomeButton").should('not.have.class', 'disabled');
    });

    it("leaderboard will show given adventurers properly", () => {
        cy.intercept('Get', '**/get-leaderboard', {
            statusCode: 200,
            body: [
                {
                    "position": 1,
                    "user": "TestingUser",
                    "adventurer": "TestingAdventurer",
                    "level": 0,
                    "rooms": 0,
                    "damage": 0,
                    "health": 0
                }
            ],
        }).as("default get-leaderboard");
        cy.visit("/");
        
        cy.get(".leaderboardEntry").should("exist");
    });
});

