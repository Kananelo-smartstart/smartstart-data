namespace SL.DataFixes.Interfaces
{
    public interface IAuthentication<DsType>
    {
        DsType Connect(string connString, string username, string password, string domain);
    }
}
