import { Attendee } from "./attendee";

export interface Meeting {
    meetingId: string;
    eventId: number;
    attendee1: Attendee;
    attendee2: Attendee;
    tableName: string;
    startTime: Date;
    finishTime: Date;
}