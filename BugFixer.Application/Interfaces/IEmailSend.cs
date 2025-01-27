namespace BugFixer.Application.Interfaces
{
   
    public interface IEmailSend
    {

        void SendEmail(string to, string subject, string body);
    }

}
