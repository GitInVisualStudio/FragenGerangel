<?php


class InvalidHigherIDException extends Exception
{
    function __construct()
    {
        parent::__construct("The given higher online-ID of object was invalid. Please try uploading the semester/course first or validating the set icon.", 6);
    }
}