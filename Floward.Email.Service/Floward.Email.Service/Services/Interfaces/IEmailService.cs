using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Email.Service.Services.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(string product);
    }
}
