namespace ProductsModels.Exceptions
{
    public class ValidationError
    {
        public string ? Error { get; set; }
        public string ? PropertyName { get; set; }
        public string ? Code { get; set; } 
    }
    
    public class ValidationException : Exception
    {
       public ValidationError[] Errors { get; set; }
       
       public ValidationException():base(){}
       
       public ValidationException(string message):base(message){}
       
       public ValidationException(string message , Exception innerException):base(message , innerException){}

       public ValidationException(string message, ValidationError[] errors) : base(message) => Errors = errors;
    }
};

