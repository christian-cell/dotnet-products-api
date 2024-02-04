namespace ProductsModels.Exceptions
{
    public class ConfigurationException : Exception
    {
        public ConfigurationException(string entity):base($"{entity} is not configured"){}
    }
};

