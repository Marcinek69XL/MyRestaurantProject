using System;

namespace MyRestaurantProject.Exceptions
{
    public class ForbidException : Exception
    {
        public ForbidException() : base()
        {
            
        }
        public ForbidException(string message) : base(message)
        {
            
        }
    }
}