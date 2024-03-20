namespace pageCountClient.Domain;

public class SubmitCountException(string msg, Exception innerException) : Exception(msg, innerException)
{
    
}