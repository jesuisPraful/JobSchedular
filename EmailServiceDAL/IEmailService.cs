using EmailServiceDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailServiceDAL
{
    public interface IEmailService
    {
        #region Email
        Task<bool> AddEmailAsync(Email email);
        Task<List<Email>> GetEmailsAsync();
        Task<bool> UpdateEmailStatusAsync(Guid emailId, string status, int retryCount, DateTime? sentAt);
        Task<List<Email>> GetPendingEmailAsync();
        Task<Email?> GetEmailByIdAsync(Guid id);
        Task<bool> UpdateEmailAsync(Email email);
        Task<bool> DeleteEmail(Guid id);

        #endregion

        #region EmailLog
        Task<bool> AddEmailLogAsync(EmailLog emailLog);
        Task<List<EmailLog>> GetEmailLogsAsync();
        Task<EmailLog?> GetEmailLogByLogsIdAsync(Guid id);
        Task<List<EmailLog>> GetEmailLogsByEmailIdAsync(Guid emailId);
        Task<bool> UpdateEmailLogsAsync(EmailLog emailLog);
        Task<bool> DeleteEmailLogsAsync(Guid id);
        #endregion

        #region EmailTemplate
        Task<bool> AddEmailTemplateAsync(EmailTemplate emailTemplate);
        Task<List<EmailTemplate>> GetEmailTemplatesAsync();
        Task<EmailTemplate?> GetEmailTemplateByIdAsync(Guid id);
        Task<bool> UpdateEmailTemplateAsync(EmailTemplate emailTemplate);
        Task<bool> DeleteEmailTemplateAsync(Guid id);
        #endregion

        #region OutboxEmail
        Task<bool> AddOutboxEmailAsync(OutboxEmail outboxEmail);
        Task<List<OutboxEmail>> GetOutboxEmailsAsync();
        Task<OutboxEmail?> GetOutboxEmailByIdAsync(Guid id);
        Task<bool> UpdateOutboxEmailAsync(OutboxEmail outboxEmail);
        Task<bool> DeleteOutboxEmailAsync(Guid id);
        #endregion

    }
}
