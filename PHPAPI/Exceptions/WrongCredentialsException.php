<?php

class WrongCredentialsException extends Exception
{
    function __construct() {
        parent::__construct("The entered credentials were wrong.", 2);
    }
}

