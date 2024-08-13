import { jwtDecode } from "jwt-decode";

export function DecodeToken(token: string | null): any | null {
  if (token == null) {
    return null;
  }
  try {
    let tokenPayload = jwtDecode(token);
    if (tokenPayload) {
      return tokenPayload;
    }
  } catch (error) {
    return null;
  }
}