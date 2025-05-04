export interface LoginRequest {
    email: string;
    password: string;
}

export interface RegisterRequest {
    email: string;
    password: string;
    firstName: string;
    lastName: string;
}

export interface JwtPayload {
    [key: string]: any;  // Allowing any property with any value
    exp: number;
}