<?php

class AuthTokenExpiredException extends Exception
{
    function __construct() {
        parent::__construct("The used authentification token has expired. Please request a new one.", 4);
    }
}

