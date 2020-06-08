<?php


class ServerException extends Exception
{
    function __construct()
    {
        parent::__construct("Something went wrong server-side. Please try again.", 0);
    }
}