"""Constants for the ReStack integration."""
from __future__ import annotations

from datetime import timedelta
from logging import Logger, getLogger
from typing import Final

from homeassistant.const import Platform

LOGGER: Logger = getLogger(__package__)

# The free plan is limited to 10 requests/minute
COORDINATOR_UPDATE_INTERVAL: timedelta = timedelta(seconds=10)

DOMAIN = "restack"
PLATFORMS: Final = [Platform.SENSOR]

ATTRIBUTION: Final = "Data provided by ReStack"

API_ATTR_OK: Final = "ok"