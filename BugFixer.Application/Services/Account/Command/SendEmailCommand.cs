using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Io;
using BugFixer.Application.Contract.DTOS.Account;
using BugFixer.Application.Interfaces;
using BugFixer.Application.Services.Result;
using MediatR;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BugFixer.Application.Services.Account.Command
{
    public record SendEmailCommand(  string to, string subject, string body) : IRequest<OperationResult>;
    public class SendEmailCommandHandler : IRequestHandler<SendEmailCommand, OperationResult>
    {
       
        private readonly IEmailSend _emailSender;

        public SendEmailCommandHandler(IEmailSend emailSender)
        {
            _emailSender = emailSender;
        }

        public async Task<OperationResult> Handle(SendEmailCommand request, CancellationToken cancellationToken)
        {
            try
            {
                 _emailSender.SendEmail(request.to, request.subject, request.body );

                return OperationResult.Success("عملیات با موفقیت انجام شد"); ;
            }
            catch (Exception exception)
            {
                return OperationResult.Error(exception.Message);
            }
        }
    }
 
}

