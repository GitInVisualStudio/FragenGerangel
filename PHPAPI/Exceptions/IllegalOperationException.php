<?php


class IllegalOperationException extends Exception
{
    function __construct()
    {
        parent::__construct("The requested operation was semantically illegal.", 8);
    }
}