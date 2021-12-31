describe("login", () => {
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
    })

    it("when not logged in show sing in button", () => {
        cy.visit("/");
        cy.get("#signInButton").should("exist");
    });

    it("when logged in show sing out button", () => {
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
        cy.get("#signInButton").should("not.exist");
        cy.get("#signOutButton").should("exist");
    });
    
    it("clicking on login button will open form", () => {
        cy.visit("/");
        cy.get("#signInButton").click()
        cy.get("#loginForm").should("exist");
    });

    it("clicking on login while open will close it", () => {
        cy.visit("/");
        cy.get("#signInButton").click()
        cy.get("#loginForm").should("exist");
        cy.get("#signInButton").click()
        cy.get("#loginForm").should("not.exist");
    });

    it("clicking on register will switch form", () => {
        cy.visit("/");
        cy.get("#signInButton").click()
        cy.get('.signUpSwitchLink').click();
        cy.get("#registerForm").should("exist");
    });
});

