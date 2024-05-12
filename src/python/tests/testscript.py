import asyncio

import aiohttp

from pyrestack import ReStack

URL = "http://192.168.5.12:5000"
USERNAME = ""
PASSWORD = ""
VERIFY_SSL = True


async def main():

    async with aiohttp.ClientSession() as session:
        restack_api = ReStack(session, URL, USERNAME, PASSWORD, VERIFY_SSL)
        response = await restack_api.async_execute_stack(1)
        print(response.data)


loop = asyncio.get_event_loop()
loop.run_until_complete(main())