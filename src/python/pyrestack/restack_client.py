# -*- coding: utf-8 -*-
#
# Copyright 2018-2023 Bartosz Kryza <bkryza@gmail.com>
#
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
#
#    http://www.apache.org/licenses/LICENSE-2.0
#
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.
"""
Example decorest based client to Swagger Petstore sample service.

    http://petstore.swagger.io/

"""

import json
import asyncio
import typing
import xml.etree.ElementTree as ET

from decorest import DELETE, GET, HEAD, POST, PUT
from decorest import HttpStatus, RestClient
from decorest import __version__
from decorest import accept, body, content, endpoint, header, on, query, backend

JsonDictType = typing.Dict[str, typing.Any]

class StackAPI(RestClient):
    
    @GET('stack/')
    @on(200, lambda r: r.json())
    @on(HttpStatus.ANY, lambda r: r.raise_for_status())
    async def get_all(self) -> JsonDictType:
        """Get all stacks status."""

    @GET('stack/{stack_id}/execute')
    @on(200, lambda r: r.json())
    @on(HttpStatus.ANY, lambda r: r.raise_for_status())
    async def execute(self, stack_id) -> JsonDictType:
        """Execute stack by id."""
    
    # """Everything about your Pets."""
    # @POST('pet')
    # @content('application/json')
    # @accept('application/json')
    # @body('pet', lambda p: json.dumps(p))
    # def add_pet(self, pet):
    #     """Add a new pet to the store."""

    # @PUT('pet')
    # @body('pet')
    # def update_pet(self, pet):
    #     """Update an existing pet."""

    # @GET('pet/findByStatus')
    # @on(200, lambda r: r.json())
    # @on(HttpStatus.ANY, lambda r: r.raise_for_status())
    # def find_pet_by_status(self):
    #     """Find Pets by status."""

    # @GET('pet/findByStatus')
    # @accept('application/xml')
    # @on(200, lambda r: ET.fromstring(r.text))
    # def find_pet_by_status_xml(self):
    #     """Find Pets by status."""

    # @GET('pet/{pet_id}')
    # def find_pet_by_id(self, pet_id):
    #     """Find Pet by ID."""

    # @HEAD('pet/{pet_id}')
    # def head_find_pet(self, pet_id):
    #     """Head find Pet by ID."""

    # @POST('pet/{pet_id}')
    # def update_pet_by_id(self, pet_id):
    #     """Update a pet in the store with form data."""

    # @DELETE('pet/{pet_id}')
    # def delete_pet(self, pet_id):
    #     """Delete a pet."""

    # @POST('pet/{pet_id}/uploadImage')
    # def upload_pet_image(self, pet_id, image):
    #     """Upload an image."""


@header('user-agent', 'decorest/{v}'.format(v=__version__))
@content('application/json')
@accept('application/json')
@backend('httpx')
class ReStackClient(StackAPI):
    """Swagger ReStack client."""

# async def main():
#     client = ReStackClient('http://192.168.5.12:5000/api')

#     print(await client.execute(7))

# asyncio.run(main())