﻿namespace restaurant_management_backend.Utility
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public int StatusCode { get; set; }
        public T? Data { get; set; }

        public static ApiResponse<T> SuccessResponse(T? data, string? message = null, int statusCode = 200)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                StatusCode = statusCode,
                Data = data
            };
        }

        public static ApiResponse<T> FailureResponse(string message, int statusCode = 400)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                StatusCode = statusCode,
                //Data = default
            };
        }

    }

}
