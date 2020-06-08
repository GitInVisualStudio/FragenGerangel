<?php
class MissingParameterException extends Exception
{
    function __construct() {
        parent::__construct("There were one or more parameters missing from your call. Please refer to the API documentation.", 1);
    }
}

