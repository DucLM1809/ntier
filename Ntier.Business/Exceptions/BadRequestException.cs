namespace Ntier.Business.Exceptions
{
    [Serializable]
    public class BadRequestException(string message) : Exception(message);
}