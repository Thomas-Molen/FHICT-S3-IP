import './FooterComponent.css'
import React, { useState, useEffect } from 'react';
import { useRecoilState } from 'recoil';
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faFacebook, faFacebookF, faFacebookMessenger, faFacebookSquare, faGithub, faGoogle, faInstagram, faLinkedin, faTwitter } from "@fortawesome/free-brands-svg-icons";

export function FooterComponent() {

    return (
        <footer class="bg-dark text-center text-white">
            <div class="container p-4 pb-0">
                <section class="mb-4">
                    <a class="btn btn-outline-light btn-floating m-1 rounded-circle" href="#" role="button">
                        <FontAwesomeIcon icon={faFacebookF} />
                    </a>

                    <a class="btn btn-outline-light btn-floating m-1 rounded-circle" href="#" role="button">
                        <FontAwesomeIcon icon={faTwitter} />
                    </a>

                    <a class="btn btn-outline-light btn-floating m-1 rounded-circle" href="#" role="button">
                    <FontAwesomeIcon icon={faGoogle} />    
                    </a>

                    <a class="btn btn-outline-light btn-floating m-1 rounded-circle" href="#" role="button">
                        <FontAwesomeIcon icon={faLinkedin} />
                    </a>

                    <a class="btn btn-outline-light btn-floating m-1 rounded-circle" href="#" role="button">
                        <FontAwesomeIcon icon={faGithub} />
                    </a>
                </section>
            </div>

            <div class="copyrightfooter">
                <div class="text-center p-3">
                    <p>© 2021 Copyright: TextAdventure.com</p>
                </div>
            </div>
        </footer>
    )
}