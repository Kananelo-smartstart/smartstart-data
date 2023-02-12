using SL.DataFixes.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SL.DataFixes.Factories.Interfaces
{
    public interface IAuthenticationFactory<T> where T : class
    {
        IAuthentication<T> BuildIAuthentication();
    }
}
