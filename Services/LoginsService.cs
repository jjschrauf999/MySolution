using Budget.Data;

namespace Budget.Services
{
    public class LoginsService
    {
        public static bool Login(int userId, bool loginFailed)
        {
            return LoginsData.Login(userId, loginFailed);
        }
    }
}
