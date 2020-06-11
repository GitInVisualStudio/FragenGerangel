<?php


class ObjectDoesntExistException extends Exception
{
    function __construct()
    {
        parent::__construct("The specified object couldn't be found.", 7);
    }
}