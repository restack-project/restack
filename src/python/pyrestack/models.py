"""ReStack models"""
from __future__ import annotations

from dataclasses import dataclass
from enum import Enum
from typing import Any
import json

from prometheus_client.parser import text_string_to_metric_families as parser

class ReStackBaseModel:
    """ReStackBaseModel."""

class StackType(str, Enum):
    """Stack type."""
    BAT = "bat"
    Shell = "shell"

class Stack(ReStackBaseModel):
    def __init__(self, id, name, type, jobs):
        self.id = id
        self.name = name
        self.type = type
        self.jobs = jobs

class Job(ReStackBaseModel):
    def __init__(self, id, started, ended, success, stackId, logs):
        self.id = id
        self.started = started
        self.ended = ended
        self.success = success
        self.stackId = stackId
        self.logs = logs

class JobLog(ReStackBaseModel):
    def __init__(self, id, timestamp, componentName, message, error, jobId):
        self.id = id
        self.timestamp = timestamp
        self.componentName = componentName
        self.message = message
        self.error = error
        self.jobId = jobId

@dataclass
class StacksApiResponse(ReStackBaseModel):
    """API response model for ReStack."""

    _method: str | None = None
    _api_path: str | None = None
    data: list[Stack] | None = None

    @staticmethod
    def from_prometheus(data: dict[str, Any]) -> StacksApiResponse:
        """Generate object from json."""
        obj: dict[str, Any] = {}
        stacks = []

        for key, value in data.items():
            if hasattr(StacksApiResponse, key):
                obj[key] = value

        json_data = json.loads(data['content'])
        stacks = []

        for stack_item in json_data:
            stack_id = stack_item["id"]
            stack_name = stack_item["name"]
            stack_type = stack_item["type"]

            jobs = stack_item["jobs"]
            job_objects = []

            for job_data in jobs:
                if (job_data is not None):
                    logs = job_data["logs"]
                    job_logs = []

                    for log_data in logs:
                        job_log = JobLog(
                            log_data["id"],
                            log_data["timestamp"],
                            log_data["componentName"],
                            log_data["message"],
                            log_data["error"],
                            log_data["jobId"]
                        )
                        job_logs.append(job_log)

                    job = Job(
                        job_data["id"],
                        job_data["started"],
                        job_data["ended"],
                        job_data["success"],
                        job_data["stackId"],
                        job_logs
                    )
                    job_objects.append(job)

            stack = Stack(stack_id, stack_name, stack_type, job_objects)
            stacks.append(stack)

        obj['data'] = stacks

        return StacksApiResponse(**obj)

@dataclass
class JobApiResponse(ReStackBaseModel):
    """API response model for ReStack."""

    _method: str | None = None
    _api_path: str | None = None
    data: list[Stack] | None = None

    @staticmethod
    def from_prometheus(data: dict[str, Any]) -> JobApiResponse:
        """Generate object from json."""
        obj: dict[str, Any] = {}
        stacks = []

        for key, value in data.items():
            if hasattr(JobApiResponse, key):
                obj[key] = value

        json_data = json.loads(data['content'])
        logs = json_data["logs"]
        job_logs = []

        for log_data in logs:
            job_log = JobLog(
                log_data["id"],
                log_data["timestamp"],
                log_data["componentName"],
                log_data["message"],
                log_data["error"],
                log_data["jobId"]
            )
            job_logs.append(job_log)

        job = Job(
            json_data["id"],
            json_data["started"],
            json_data["ended"],
            json_data["success"],
            json_data["stackId"],
            job_logs
        )

        obj['data'] = job

        return JobApiResponse(**obj)
