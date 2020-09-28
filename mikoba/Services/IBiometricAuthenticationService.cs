using System;
using System.Threading.Tasks;

namespace mikoba.Services
{
    public interface IBiometricAuthenticationService
    {
        string GetAuthenticationType();
        Task<bool> AuthenticateUserIDWithTouchID();
        bool fingerprintEnabled();
    }
}