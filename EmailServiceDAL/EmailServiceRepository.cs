using EmailServiceDAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailServiceDAL
{
    public class EmailServiceRepository : IEmailService
    {
        private readonly EmailServiceDbContext _context;
        private readonly ILogger<EmailServiceRepository> _logger;

        public EmailServiceRepository(ILogger<EmailServiceRepository> logger)
        {
            _context = new EmailServiceDbContext();
            _logger = logger;
        }

        #region Email
        public async Task<bool> AddEmailAsync(Email email)
        {
            bool status = false;
            try
            {
                await _context.Emails.AddAsync(email);
                await _context.SaveChangesAsync();
                status = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AddEmailAsync");
                status = false;
            }
            return status;
        }

        public async Task<List<Email>> GetEmailsAsync()
        {
            List<Email> emails = new List<Email>();
            try
            {
                emails = await _context.Emails.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error in GetEmailsAsync");
                emails = new List<Email>();
            }
            return emails;
        }

        public async Task<List<Email>> GetPendingEmailAsync()
        {
            List<Email> emails = new List<Email>();
            try
            {
                emails = await _context.Emails.AsNoTracking().Where(e => (e.Status == "pending" || e.Status== "Retrying") && e.RetryCount< 3).OrderBy(e=>e.CreatedAt).Take(10).ToListAsync();
            }
            catch (Exception)
            {
                _logger.LogError("Error in GetPendingEmail");
                emails = new List<Email>();
            }
            return emails;
        }
        public async Task<Email?> GetEmailByIdAsync(Guid id)
        {
            Email email = new Email();
            try
            {   
                email =  await _context.Emails.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error in GetEmailByIdAsync");
                email = null;
            }
            return email;
        }

        public async Task<bool> UpdateEmailAsync(Email email)
        {
            bool status = false;
            try
            {
                var existingEmail = await _context.Emails.FirstOrDefaultAsync(e => e.Id == email.Id);
                if (existingEmail == null)
                {
                    _logger.LogWarning("Email with Id {EmailId} not found for update", email.Id);
                    status = false;
                }
                else
                {
                    _context.Emails.Update(email);
                    await _context.SaveChangesAsync();
                    status = true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateEmailAsync");
                status = false; 
            }
            return status;
        }

        public async Task<bool> UpdateEmailStatusAsync(Guid emailId, string status, int retryCount, DateTime? sentAt)
        {
            bool outputStatus = false;
            try
            {
                var existingEmail = await _context.Emails.FirstOrDefaultAsync(e => e.Id == emailId);
                if (existingEmail == null)
                {
                    _logger.LogWarning("Email with Id {EmailId} not found for status update", emailId);
                    outputStatus = false;
                }
                else
                {
                    existingEmail.Status = status;
                    existingEmail.RetryCount = retryCount;
                    existingEmail.SentAt = sentAt;

                    _context.Emails.Update(existingEmail);
                    await _context.SaveChangesAsync();
                    outputStatus = true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateEmailStatusAsync");
                outputStatus = false;
            }
            return outputStatus;
        }
        public async Task<bool> DeleteEmail(Guid id)
        {
            bool status = false;
            try
            {
                Email email = await _context.Emails.FindAsync(id);
                if (email == null)
                {
                    _logger.LogError("Email not found in DeleteEmail");
                    status = false;
                }
                else
                {
                    _context.Emails.Remove(email);
                    await _context.SaveChangesAsync();
                    status = true;
                }
            }
            catch (Exception)
            {
                _logger.LogError("Error in DeleteEmail");
                status = false;
            }
            return status;
        }
        #endregion

        #region EmailLog
        public async Task<bool> AddEmailLogAsync(EmailLog emailLog)
        {
            bool status = false;
            try
            {
                await _context.EmailLogs.AddAsync(emailLog);
                await _context.SaveChangesAsync();
                status = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AddEmailLogAsync");
                status = false;
            }
            return status;
        }
        public async Task<List<EmailLog>> GetEmailLogsAsync()
        {
            List<EmailLog> emailLogs = new List<EmailLog>();
            try
            {
                emailLogs = await _context.EmailLogs.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetEmailLogsByEmailIdAsync");
                emailLogs = new List<EmailLog>();
            }
            return emailLogs;
        }

        public async Task<EmailLog> GetEmailLogByLogsIdAsync(Guid id)
        {
            EmailLog emailLog = new EmailLog();
            try
            {
                emailLog = await _context.EmailLogs.FindAsync(id);
            }
            catch (Exception)
            {
                _logger.LogError("Error in GetEmailLogByLogsIdAsync");
                emailLog = new EmailLog();
            }
            return emailLog;
        }

        public async Task<List<EmailLog>> GetEmailLogsByEmailIdAsync(Guid emailId)
        {
            List<EmailLog> emailLogs = new List<EmailLog>();
            try
            {
                emailLogs = await _context.EmailLogs
                                          .Where(el => el.EmailId == emailId)
                                          .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetEmailLogsByEmailIdAsync");
                emailLogs = new List<EmailLog>();
            }
            return emailLogs;
        }

        public async Task<bool> UpdateEmailLogsAsync(EmailLog emailLog)
        {
            bool status = false;
            try
            {
                EmailLog existingEmailLog = await _context.EmailLogs.FindAsync(emailLog.Id);
                if (existingEmailLog == null)
                {
                    _logger.LogError("Email with Id {EmailId} not found for update", emailLog.Id);
                    status = false;
                }
                else
                {
                    _context.EmailLogs.Update(emailLog);
                    await _context.SaveChangesAsync();
                    status = true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateEmailLogsAsync");
                status = false;
            }
            return status;
        }

        public async Task<bool> DeleteEmailLogsAsync(Guid id)
        {
            bool status = false;
            try
            {
                EmailLog emailLog = await _context.EmailLogs.FindAsync(id);
                if (emailLog == null)
                {
                    _logger.LogError("EmailLog not found in DeleteEmailLogsAsync");
                    status = false;
                }
                else
                {
                    _context.EmailLogs.Remove(emailLog);
                    await _context.SaveChangesAsync();
                    status = true;
                }
            }
            catch (Exception)
            {
                _logger.LogError("Error in DeleteEmailLogsAsync");
                status = false;
            }
            return status;
        }
        #endregion

        #region EmailTemplate

        public async Task<bool> AddEmailTemplateAsync(EmailTemplate emailTemplate)
        {
            bool status = false;
            try
            {
                await _context.EmailTemplates.AddAsync(emailTemplate);
                await _context.SaveChangesAsync();
                status = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AddEmailTemplateAsync");
                status = false;
            }
            return status;
        }

        public async Task<List<EmailTemplate>> GetEmailTemplatesAsync()
        {
            List<EmailTemplate> emailTemplates = new List<EmailTemplate>();
            try
            {
                emailTemplates = await _context.EmailTemplates.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetEmailTemplatesAsync");
                emailTemplates = new List<EmailTemplate>();
            }
            return emailTemplates;
        }

        public async Task<EmailTemplate?> GetEmailTemplateByIdAsync(Guid id)
        {
            EmailTemplate emailTemplate = new EmailTemplate();
            try
            {
                emailTemplate = await _context.EmailTemplates.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetEmailTemplateByIdAsync");
                emailTemplate = null;
            }
            return emailTemplate;
        }

        public async Task<bool> UpdateEmailTemplateAsync(EmailTemplate emailTemplate)
        {
            bool status = false;
            try
            {
                var existingTemplate = await _context.EmailTemplates.FirstOrDefaultAsync(et => et.Id == emailTemplate.Id);
                if (existingTemplate == null)
                {
                    _logger.LogWarning("EmailTemplate with Id {TemplateId} not found for update", emailTemplate.Id);
                    status = false;
                }
                else
                {
                    _context.EmailTemplates.Update(emailTemplate);
                    await _context.SaveChangesAsync();
                    status = true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateEmailTemplateAsync");
                status = false;
            }
            return status;
        }

        public async Task<bool> DeleteEmailTemplateAsync(Guid id)
        {
            bool status = false;
            try
            {
                EmailTemplate emailTemplate = await _context.EmailTemplates.FindAsync(id);
                if (emailTemplate == null)
                {
                    _logger.LogError("EmailTemplate not found in DeleteEmailTemplateAsync");
                    status = false;
                }
                else
                {
                    _context.EmailTemplates.Remove(emailTemplate);
                    await _context.SaveChangesAsync();
                    status = true;
                }
            }
            catch (Exception)
            {
                _logger.LogError("Error in DeleteEmailTemplateAsync");
                status = false;
            }
            return status;
        }
        #endregion

        #region OutBoxEmails 
        public async Task<bool> AddOutboxEmailAsync(OutboxEmail outboxEmail)
        {
            bool status = false;
            try
            {
                await _context.OutboxEmails.AddAsync(outboxEmail);
                await _context.SaveChangesAsync();
                status = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AddOutboxEmailAsync");
                status = false;
            }
            return status;
        }

        public async Task<List<OutboxEmail>> GetOutboxEmailsAsync()
        {
            List<OutboxEmail> outboxEmails = new List<OutboxEmail>();
            try
            {
                outboxEmails = await _context.OutboxEmails.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetOutboxEmailsAsync");
                outboxEmails = new List<OutboxEmail>();
            }
            return outboxEmails;
        }

        public async Task<OutboxEmail?> GetOutboxEmailByIdAsync(Guid id)
        {
            OutboxEmail outboxEmail = new OutboxEmail();
            try
            {
                outboxEmail = await _context.OutboxEmails.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetOutboxEmailByIdAsync");
                outboxEmail = null;
            }
            return outboxEmail;
        }

        public async Task<bool> UpdateOutboxEmailAsync(OutboxEmail outboxEmail)
        {
            bool status = false;
            try
            {
                var existingOutboxEmail = await _context.OutboxEmails.FirstOrDefaultAsync(oe => oe.Id == outboxEmail.Id);
                if (existingOutboxEmail == null)
                {
                    _logger.LogWarning("OutboxEmail with Id {OutboxEmailId} not found for update", outboxEmail.Id);
                    status = false;
                }
                else
                {
                    _context.OutboxEmails.Update(outboxEmail);
                    await _context.SaveChangesAsync();
                    status = true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateOutboxEmailAsync");
                status = false;
            }
            return status;
        }

        public async Task<bool> DeleteOutboxEmailAsync(Guid id)
        {
            bool status = false;
            try
            {
                OutboxEmail outboxEmail = await _context.OutboxEmails.FindAsync(id);
                if (outboxEmail == null)
                {
                    _logger.LogError("OutboxEmail not found in DeleteOutboxEmailAsync");
                    status = false;
                }
                else
                {
                    _context.OutboxEmails.Remove(outboxEmail);
                    await _context.SaveChangesAsync();
                    status = true;
                }
            }
            catch (Exception)
            {
                _logger.LogError("Error in DeleteOutboxEmailAsync");
                status = false;
            }
            return status;
        }
        #endregion

    }
}
