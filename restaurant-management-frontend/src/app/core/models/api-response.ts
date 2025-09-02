export class ApiResponse<T> {
  success: boolean;
  message: string;
  statusCode: number;
  data?: T | null;

  private constructor(
    success: boolean,
    message: string,
    statusCode: number,
    data: T | null
  ) {
    this.success = success;
    this.message = message;
    this.statusCode = statusCode;
    this.data = data ?? null;
  }

  static successResponse<T>(data?: T | null, message: string='', statusCode: number = 200): ApiResponse<T> {
    return new ApiResponse<T>(true, message, statusCode, data ?? null);
  }

  static failureResponse<T>(message: string, statusCode: number = 400): ApiResponse<T> {
    return new ApiResponse<T>(false, message, statusCode, null);
  }
}
