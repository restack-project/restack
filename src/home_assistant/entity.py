from __future__ import annotations

from homeassistant.helpers.device_registry import DeviceEntryType
from homeassistant.helpers.entity import DeviceInfo, EntityDescription
from homeassistant.helpers.update_coordinator import CoordinatorEntity

from . import ReStackDataUpdateCoordinator
from .const import ATTRIBUTION, DOMAIN

import typing

JsonDictType = typing.Dict[str, typing.Any]


class ReStackEntity(CoordinatorEntity[ReStackDataUpdateCoordinator]):
    """Base UptimeKuma entity."""

    _attr_attribution = ATTRIBUTION

    def __init__(
        self,
        coordinator: ReStackDataUpdateCoordinator,
        description: EntityDescription,
        stack: JsonDictType,
    ) -> None:
        """Initialize ReStack entities."""
        super().__init__(coordinator)
        self.entity_description = description
        self._stack = stack
        self._attr_device_info = DeviceInfo(
            identifiers={(DOMAIN, str(self.stack["name"]))},
            name=self.stack["name"],
            manufacturer="ReStack Integration",
            entry_type=DeviceEntryType.SERVICE,
        )
        self._attr_extra_state_attributes = {
            "type": self.stack["type"],
            "average_runtime": self.stack["averageRuntime"],
            "success_percentage": self.stack["succesPercentage"],
        }
        self._attr_unique_id = f"restack_{self.stack['name']}"
        self.api = coordinator.api

    @property
    def _stacks(self):
        """Return all stacks."""
        return self.coordinator.data or []

    @property
    def stack(self):
        """Return the stack for this entity."""
        return next(
            (
                stack
                for stack in self._stacks
                if str(stack["name"]) == self.entity_description.key
            ),
            self._stack,
        )
