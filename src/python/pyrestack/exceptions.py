"""ReStack exceptions."""


class ReStackException(Exception):
    """Base ReStack exception."""


class ReStackConnectionException(ReStackException):
    """ReStack connection exception."""


class ReStackAuthenticationException(ReStackException):
    """ReStack authentication exception."""
