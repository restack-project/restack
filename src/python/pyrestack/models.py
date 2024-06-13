"""ReStack models"""
from __future__ import annotations

from dataclasses import dataclass
from enum import Enum
from typing import Any

@dataclass
class ReStack_Stack():
    """Stack model for ReStack."""

    id: int = 0
    name: str = ""
    averageRuntime: str = ""

    @staticmethod
    def from_dict(data: dict[str, Any]) -> ReStack_Stack:
        """Generate object from json."""
        obj: dict[str, Any] = {}
        for key, value in data.items():
            if hasattr(ReStack_Stack, key):
                obj[key] = value

        return ReStack_Stack(**obj)