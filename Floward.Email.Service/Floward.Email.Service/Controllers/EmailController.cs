using Floward.Email.Service.Domain.Entities;
using Floward.Email.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Floward.Email.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpGet("Send")]
        public async Task SendEmail(string product)
        {
            await _emailService.SendEmailAsync(product);
        }
    }
}
