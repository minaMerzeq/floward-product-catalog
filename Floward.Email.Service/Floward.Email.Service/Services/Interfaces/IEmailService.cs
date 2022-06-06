using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Floward.Email.Service.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string product);
    }
}
