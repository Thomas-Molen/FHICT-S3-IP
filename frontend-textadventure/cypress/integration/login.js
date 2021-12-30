import { React } from 'react';
import { useSetRecoilState } from 'recoil';
import { userAtom } from '../../src/state';

describe("renders the home page", () => {
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

