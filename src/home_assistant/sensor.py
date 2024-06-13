"""Platform for sensor integration."""

from __future__ import annotations

import typing

from homeassistant.components.sensor import (
    SensorDeviceClass,
    SensorEntity,
    SensorStateClass,
)
from homeassistant.config_entries import ConfigEntry
from homeassistant.components.sensor import SensorEntity, SensorEntityDescription
from homeassistant.const import UnitOfTemperature
from homeassistant.core import HomeAssistant
from homeassistant.helpers.entity import EntityCategory, EntityDescription
from homeassistant.helpers.entity_platform import AddEntitiesCallback
from homeassistant.helpers.typing import ConfigType, DiscoveryInfoType

from .const import DOMAIN
from . import ReStackDataUpdateCoordinator
from .entity import ReStackEntity
from .utils import format_entity_name, sensor_name_from_url

JsonDictType = typing.Dict[str, typing.Any]


async def async_setup_entry(
    hass: HomeAssistant,
    entry: ConfigEntry,
    async_add_entities: AddEntitiesCallback,
) -> None:
    """Set up the ReStack sensors."""
    coordinator: ReStackDataUpdateCoordinator = hass.data[DOMAIN][entry.entry_id]
    async_add_entities(
        ReStackSensor(
            coordinator,
            SensorEntityDescription(
                key=str(stack["name"]),
                name=stack["name"],
                entity_category=EntityCategory.DIAGNOSTIC,
                device_class="restack__stacks",
            ),
            stack=stack,
        )
        for stack in coordinator.data
    )


class ReStackSensor(ReStackEntity, SensorEntity):
    """Representation of a ReStack sensor."""

    def __init__(
        self,
        coordinator: ReStackDataUpdateCoordinator,
        description: EntityDescription,
        stack: JsonDictType,
    ) -> None:
        """Set entity ID."""
        super().__init__(coordinator, description, stack)
        self.entity_id = f"sensor.restack_{format_entity_name(self.stack['name'])}"

    @property
    def native_value(self) -> str:
        """Return the status of the stack."""
        if self.stack["lastJob"]:
            return self.stack["lastJob"]["state"]

        return "Not run"

    @property
    def icon(self) -> str:
        """Return the icon of the stack."""
        if self.stack["lastJob"]:
            if self.stack["lastJob"]["state"] == "Success":
                return "mdi:check-circle"
            return "mdi:alpha-x-circle"

        return "mdi:help-circle"
