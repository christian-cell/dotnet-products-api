namespace ProductsModels.Exceptions
{
    public class AlreadyExistsException : Exception
    {
       public AlreadyExistsException(string entity , string value): base($"{entity} with Id '{value}' already exists"){}
       public AlreadyExistsException(string entity , string property , string value): base($"{entity} with {property} '{value}' already exists"){}
       
       public AlreadyExistsException(string entity , Guid value): base($"{entity} with Id '{value}' already exists"){}
       public AlreadyExistsException(string entity , string property , Guid value): base($"{entity} with {property} '{value}' already exists"){}
    }
};

