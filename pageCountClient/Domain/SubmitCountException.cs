namespace pageCountClient.Domain;

public class SubmitCountException : Exception
{

    public SubmitCountException(string msg) : base(msg)
    {
        
    }
    
    public SubmitCountException(string msg, Exception innerException):  base(msg, innerException)
    {
        
    }
}