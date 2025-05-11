export interface Event {
    eventId: number;
    name: string;
    startDate: Date | string;
    finishDate: Date | string;
    location: string;
    bio: string;
    networkingStartDate: Date | string;
    networkingFinishDate: Date | string;
    meetingDuration: number;
    breakDuration: number;
    availableTables: number;
  }
  