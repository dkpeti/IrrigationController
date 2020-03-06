using IrrigationController.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IrrigationController.Service
{
    public delegate void LoginSucceeded(User user);
    public delegate void LoginFailed(string reason);
    public delegate void LoginCanceled();

    public interface ILoginService
    {
        Task<bool> LoginCheck();
        void LoginWithGoogle(LoginSucceeded loginSucceeded, LoginFailed loginFailed, LoginCanceled loginCanceled);
        Task Logout();
    }
}
