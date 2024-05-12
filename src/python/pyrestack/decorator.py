"""Decorator for ReStack"""
from __future__ import annotations

import asyncio
from typing import TYPE_CHECKING

import aiohttp

from .exceptions import ReStackConnectionException, ReStackException, ReStackAuthenticationException

from .const import LOGGER
from .models import StacksApiResponse, JobApiResponse

if TYPE_CHECKING:
    from .ReStack import ReStack


def api_request(api_path: str, method: str = "GET"):
    """Decorator for ReStack API request"""

    def decorator(func):
        """Decorator"""

        async def wrapper(*args, **kwargs):
            """Wrapper"""
            # TODO redo this
            resolved_api_path = api_path.replace('{id}', f'{args[1]}' if args and len(args) >= 2 else '')

            client: ReStack = args[0]
            url = f"{client._base_url}{resolved_api_path}"
            LOGGER.debug("Requesting %s", url)

            try:
                request = await client._session.request(
                    method=method,
                    url=url,
                    timeout=aiohttp.ClientTimeout(total=10),
                    auth=aiohttp.BasicAuth(client._username, client._password),
                    verify_ssl=client._verify_ssl,
                )

                if request.status != 200:
                    raise ReStackConnectionException(f"Request for '{url}' failed with status code '{request.status}'")

                result = await request.text()

            except aiohttp.ClientError as exception:
                raise ReStackConnectionException(f"Request exception for '{url}' with - {exception}") from exception

            except asyncio.TimeoutError:
                raise ReStackConnectionException(f"Request timeout for '{url}'") from None

            except ReStackConnectionException as exception:
                raise ReStackConnectionException(exception) from exception

            except ReStackException as exception:
                raise ReStackException(exception) from exception

            except (Exception, BaseException) as exception:
                raise ReStackException(f"Unexpected exception for '{url}' with - {exception}") from exception

            LOGGER.debug("Requesting %s returned %s", url, result)

            # print(result)
            # TODO redo this
            if (api_path == "/api/stack"):
                response = StacksApiResponse.from_prometheus(
                    {"content": result, "_api_path": api_path, "_method": method}
                )
            else:
                response = JobApiResponse.from_prometheus(
                    {"content": result, "_api_path": api_path, "_method": method}
                )

            return response

        return wrapper

    return decorator
