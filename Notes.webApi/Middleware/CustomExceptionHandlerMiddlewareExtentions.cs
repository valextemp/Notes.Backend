namespace Notes.webApi.Middleware
{
    // Класс созданный для того чтобф могли включать наш Middleware  в конвеер
    public static class CustomExceptionHandlerMiddlewareExtentions
    {
        public static IApplicationBuilder UseCustomExceptionHundler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomExceptionHandlerMiddleware>();   
        }
    }
}
