# pyuptimekuma
Simple Python wrapper for ReStack

## Installation

```shell
python3 -m pip install pyrestack
```

## Example

```python
import asyncio

import aiohttp

from pyrestack import ReStack

URL = ""
USERNAME = ""
PASSWORD = ""
VERIFY_SSL = True


async def main():

    async with aiohttp.ClientSession() as session:
        restack_api = ReStack(session, URL, USERNAME, PASSWORD, VERIFY_SSL)
        response = await restack_api.async_get_stacks()
        print(response.data)


loop = asyncio.get_event_loop()
loop.run_until_complete(main())

```

## Credit

I would like to give a special thanks to these repositories since a lot of code has been inspired by them.

- [ludeeus/pyuptimerobot](https://github.com/ludeeus/pyuptimerobot)
- [meichthys/utptime_kuma_monitor](https://github.com/meichthys/utptime_kuma_monitor)
- [jayakornk/pyuptimekuma](https://github.com/meichthys/utptime_kuma_monitor)