import api from "./api";
import type {
  LoginRequest,
  LoginResponse,
  RegisterRequest,
  RegisterResponse,
} from "../types/auth";

export const register = async (
  request: RegisterRequest
): Promise<RegisterResponse> => {
  const response = await api.post<RegisterResponse>(
    "/api/Auth/register",
    request
  );

  return response.data;
};

export const login = async (
    request:LoginRequest
) : Promise<LoginResponse> => {
    const response = await api.post<LoginResponse>(
        "/api/Auth/login", 
        request
    );
    return response.data;
};
