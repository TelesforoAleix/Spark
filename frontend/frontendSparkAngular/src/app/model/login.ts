export interface Login {
    username: String;
    password: String;
  }
  
  export interface LoginResponse {
    headerValue: string;
    userId: number;
    username: string;
  }