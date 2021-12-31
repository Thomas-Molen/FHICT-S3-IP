describe("game page", () => {
    beforeEach(() => {
        cy.intercept('Get', '**/get-leaderboard', {
            statusCode: 200,
            body: [],
        }).as("default get-leaderboard");

        cy.intercept('Post', '**/renew-token', {
            statusCode: 200,
            body: {
                id: 1,
                username: "testingUsername",
                email: "testingEmail",
                admin: false,
                token: "testingJWTToken"
            },
        }).as("default renew-token");

        cy.intercept('GET', '**/get/1', {
            statusCode: 200,
            body: {
                drawing: ""
            },
        }).as("default drawing get");

        cy.intercept('POST', '**/negotiate?negotiateVersion=1', {
            statusCode: 400
        }).as("default upgrade websockets");

        cy.visit("/game");
    });

    it("Console will display text", () => {
        cy.wait(2000);
        cy.get("#console").invoke('val')
            .then(consoleText => expect(consoleText).to.contain("Failed"));
    });

    it("Can input text in input field", () => {
        cy.get("#consoleInput").type("help");
        cy.get("#consoleInput").should("have.value", "help");
        cy.get("#consoleInput").type("{enter}");
        cy.get("#consoleInput").should("have.value", "");
    });

    it("Can get previously sent message from input field", () => {
        cy.get("#consoleInput").type("help");
        cy.get("#consoleInput").type("{enter}");
        cy.get("#consoleInput").type("{uparrow}");
        cy.get("#consoleInput").should("have.value", "help");
    });

    it("can save drawing", () => {
        cy.intercept('Post', '**/save/**', {
            statusCode: 200
        }).as("override save drawing");

        cy.get(".NotePadCanvas > :nth-child(4)").click(10, 10);
        cy.get("#saveDrawingOption").click();

        //wait for request and then check it
        cy.wait("@override save drawing");
        cy.get("@override save drawing")
            .its("request.body")
            .should(
                "deep.equal",
                {
                    drawing: "N4IgNglgdgpgziAXAbVABwPbQC4JaADyQE4A6YiyqygFgGYAaEATyQEYAGUgJgFYOBgoR14B2AL4NCJctTnF6TVok49+w4WMnTEZeXMUt2XPhs0SAukwBGAJwCucABYBhDGAy2kIAMQAODgCAkBsHZwAlAEMAEwhHJG5xKxAAdwho7CdjASYnGAgAcydsJDoANg5xIA="
                }
            );
    });
});

