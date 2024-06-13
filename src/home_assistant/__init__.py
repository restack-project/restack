from __future__ import annotations

import asyncio
import logging

from homeassistant.const import (
    CONF_HOST,
)
from homeassistant.core import HomeAssistant
from homeassistant.config_entries import ConfigEntry
from homeassistant.helpers.typing import ConfigType
from homeassistant.helpers import device_registry as dr
from homeassistant.helpers.aiohttp_client import async_get_clientsession
from homeassistant.helpers.update_coordinator import DataUpdateCoordinator, UpdateFailed

from .const import COORDINATOR_UPDATE_INTERVAL, DOMAIN, LOGGER, PLATFORMS
from pyrestack import restack_client

# The domain of your component. Should be equal to the name of your component.
_LOGGER: logging.Logger = logging.getLogger(__package__)


async def async_setup_entry(hass: HomeAssistant, entry: ConfigEntry) -> bool:
    """Set up ReStack from a config entry."""
    hass.data.setdefault(DOMAIN, {})
    host: str = entry.data[CONF_HOST]
    restack_api = restack_client.ReStackClient(host, backend="httpx")
    dev_reg = dr.async_get(hass)
    hass.data[DOMAIN][entry.entry_id] = coordinator = ReStackDataUpdateCoordinator(
        hass,
        config_entry_id=entry.entry_id,
        dev_reg=dev_reg,
        api=restack_api,
    )

    await coordinator.async_config_entry_first_refresh()

    await hass.config_entries.async_forward_entry_setups(entry, PLATFORMS)

    # Return boolean to indicate that initialization was successfully.
    return True


async def async_unload_entry(hass: HomeAssistant, entry: ConfigEntry) -> bool:
    """Unload a config entry."""
    if unload_ok := await hass.config_entries.async_unload_platforms(entry, PLATFORMS):
        hass.data[DOMAIN].pop(entry.entry_id)

    return unload_ok


class ReStackDataUpdateCoordinator(DataUpdateCoordinator):
    """Data update coordinator for ReStack"""

    data: list
    config_entry: ConfigEntry

    def __init__(
        self,
        hass: HomeAssistant,
        config_entry_id: str,
        dev_reg: dr.DeviceRegistry,
        api: restack_client.ReStackClient,
    ) -> None:
        """Initialize coordinator."""
        super().__init__(
            hass,
            LOGGER,
            name=DOMAIN,
            update_interval=COORDINATOR_UPDATE_INTERVAL,
        )
        self._config_entry_id = config_entry_id
        self._device_registry = dev_reg
        self.api = api

    async def _async_update_data(self) -> dict | None:
        """Update data."""
        stacks = await self.api.get_all()

        current_stacks = {
            list(device.identifiers)[0][1]
            for device in dr.async_entries_for_config_entry(
                self._device_registry, self._config_entry_id
            )
        }

        new_stacks = {str(stack["name"]) for stack in stacks}
        if stale_stacks := current_stacks - new_stacks:
            for stack_id in stale_stacks:
                if device := self._device_registry.async_get_device(
                    {(DOMAIN, stack_id)}
                ):
                    self._device_registry.async_remove_device(device.id)

        # If there are new monitors, we should reload the config entry so we can
        # create new devices and entities.
        if self.data and new_stacks - {str(stack["name"]) for stack in self.data}:
            self.hass.async_create_task(
                self.hass.config_entries.async_reload(self._config_entry_id)
            )
            return None

        return stacks
