namespace ProductsModels.Exceptions
{
    public class NotAllowedException : Exception
    {
       public NotAllowedException():base(){}
       
       public NotAllowedException(string message):base(message){}
       
       public NotAllowedException(string message , Exception innerException):base(message , innerException){}
    }
};

