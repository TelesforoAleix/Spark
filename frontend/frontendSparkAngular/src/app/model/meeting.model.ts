// Model: Defines the shape of a Meeting object used in the frontend
// Used across components and services to ensure type safety and consistency

export interface Meeting {
    id: number;
    attendeeId: number;
    tableName: string;
    time: string;
  }
  