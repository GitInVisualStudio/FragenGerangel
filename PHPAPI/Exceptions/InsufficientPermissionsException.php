<?php


class InsufficientPermissionsException extends Exception
{
    function __construct()
    {
        parent::__construct("You do not have permission to perform this action.", 5);
    }
}