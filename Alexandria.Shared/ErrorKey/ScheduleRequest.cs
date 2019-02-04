using System;
using System.Collections.Generic;
using System.Text;
using Svalbard;

namespace Alexandria.Shared.ErrorKey
{
  public static class ScheduleRequest
  {
    public static ServiceError NotFound = new ServiceError("SCHEDULE_REQUEST.NOT_FOUND", 404);
    public static ServiceError ScheduleRequestWithTimeslotAlreadyExists = new ServiceError("SCHEDULE_REQUEST.TIMESLOT_ALREADY_EXISTS", 409);
    public static ServiceError UnmatchingMatchTypes = new ServiceError("SCHEDULE_REQUEST.MATCHTYPE_NOT_MATCHING", 409);
  }
}
