using System;

namespace MyRestaurantProject.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message)
        {
            
        }
    }
}