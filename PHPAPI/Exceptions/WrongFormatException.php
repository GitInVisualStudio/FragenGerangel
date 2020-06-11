<?php

class WrongFormatException extends Exception
{
    function __construct() {
        parent::__construct("A parameter you passed was in the wrong format. Please refer to the API documentation.", 3);
    }
}

