using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthService.DomainModel
{
    [Table("AuditLogs")]
    public class AuditLog:IEntity
    {
        [Key]
        public int Id { get; set; }
        public int userId { get; set; }
        public string userName { get; set; }
        public string action { get; set; }
        public string entityType { get; set; }
        public string entityId { get; set; }
        public string entityName { get; set; }
        public string entityDescription { get; set; }
        public DateTime timestamp { get; set; }
        public string ipAddress { get; set; }
        public string userAgent { get; set; }
        public string sessionId { get; set; }
        public string correlationId { get; set; }
        public string additionalData { get; set; }
        public string status { get; set; }
        public string errorMessage { get; set; }
        public string errorStackTrace { get; set; }
        public string requestUrl { get; set; }
        public string requestMethod { get; set; }
        public string responseStatusCode { get; set; }
        public string responseHeaders { get; set; }
        public string responseBody { get; set; }
        public string requestHeaders { get; set; }
    
    }
    [Table("AuditLogDetails")]
    public class AuditLogDetail
    {
        public int Id { get; set; }
        public int AuditLogId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public AuditLog AuditLog { get; set; }
    }

}
