describe("home page game area", () => {
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

        cy.intercept('GET', '**/get', {
            statusCode: 200,
            body: [
                {
                    "name": "Adventurer",
                    "level": 1,
                    "health": 1,
                    "id": 1,
                    "damage": 1
                }
            ],
        }).as("default Adventurer get");
        cy.visit("/");
    });

    it("Clicking on start will display adventurers", () => {
        cy.get(".gameHomeButton").click();
        cy.get(".adventurerOption").should("exist");
    });

    it("Clicking on adventurer will toggle highlight", () => {
        cy.get(".gameHomeButton").click();
        cy.get(".adventurerOption").click();
        cy.get(".adventurerOption").should("have.css", "border-color", "rgb(17, 182, 218)");
        cy.get(".adventurerOption").click();
        cy.get(".adventurerOption").should("not.have.css", "border-color", "rgb(17, 182, 218)");
    });

    it("having no adventurer selected will have disabled resume button", () => {
        cy.get(".gameHomeButton").click();
        cy.get(".resumeButton").should("have.class", "disabled");
    });

    it("having adventurer selected will not have disabled resume button", () => {
        cy.get(".gameHomeButton").click();
        cy.get(".adventurerOption").click();
        cy.get(".resumeButton").should("not.have.class", "disabled");
    });

    it("clicking resume button will change the url", () => {
        cy.get(".gameHomeButton").click();
        cy.get(".adventurerOption").click();
        cy.get(".resumeButton").click();
        cy.location('pathname').should("eq", "/game");
        cy.location('search').should("eq", "?user=1");
    });

    it("clicking on new adventurer button will give input", () => {
        cy.get(".gameHomeButton").click();
        cy.get(".CreateAdventurerButton").click();
        cy.get(".createAdventurerInputBox").should("exist");
        cy.get("#createAdventurerButton").should("exist");
    });

    it("creating new adventurer will create a new adventurer", () => {
        cy.intercept("POST", "**/create", {
            statusCode: 200,
        }).as("override create adventurer");

        cy.get(".gameHomeButton").click();
        cy.get(".CreateAdventurerButton").click();
        cy.get("#createAdventurerButton").click();

        //check request made
        cy.get("@override create adventurer")
            .its("request.body")
            .should(
                "deep.equal",
                {
                    name: ""
                }
            );
    });

    it("clicking on delete button will delete adventurer", () => {
        cy.intercept("DELETE", "**/delete", {
            statusCode: 200,
        }).as("override delete adventurer");

        cy.get(".gameHomeButton").click();
        cy.get(".adventurerDeleteButton").click();
        cy.on("window:confirm", () => true);

        //check request made
        cy.get("@override delete adventurer")
            .its("request.body")
            .should(
                "deep.equal",
                {
                    adventurerId: 1
                }
            );

        cy.get(".adventurerOption").should("not.exist");
    });
});

