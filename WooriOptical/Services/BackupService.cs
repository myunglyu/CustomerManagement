using Microsoft.EntityFrameworkCore;
using WooriOptical.Models;

namespace WooriOptical.Services
{
    public interface IBackupService
    {
        Task<bool> CreateBackupAsync(string backupPath);
        Task<bool> RestoreBackupAsync(string backupPath);
        Task<bool> CreateScheduledBackupAsync();
    }

    public class BackupService : IBackupService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<BackupService> _logger;
        private readonly IConfiguration _configuration;

        public BackupService(AppDbContext context, ILogger<BackupService> logger, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<bool> CreateBackupAsync(string backupPath)
        {
            try
            {
                var sourceDb = _context.Database.GetConnectionString();
                if (string.IsNullOrEmpty(sourceDb))
                {
                    _logger.LogError("Database connection string is null or empty");
                    return false;
                }

                // Extract the database file path from connection string
                var dbPath = sourceDb.Replace("Data Source=", "").Trim();
                
                if (!File.Exists(dbPath))
                {
                    _logger.LogError("Source database file not found: {DbPath}", dbPath);
                    return false;
                }

                // Ensure backup directory exists
                var backupDir = Path.GetDirectoryName(backupPath);
                if (!string.IsNullOrEmpty(backupDir) && !Directory.Exists(backupDir))
                {
                    Directory.CreateDirectory(backupDir);
                }

                // Copy database file
                File.Copy(dbPath, backupPath, overwrite: true);
                
                _logger.LogInformation("Database backup created successfully: {BackupPath}", backupPath);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create database backup: {BackupPath}", backupPath);
                return false;
            }
        }

        public async Task<bool> RestoreBackupAsync(string backupPath)
        {
            try
            {
                if (!File.Exists(backupPath))
                {
                    _logger.LogError("Backup file not found: {BackupPath}", backupPath);
                    return false;
                }

                var sourceDb = _context.Database.GetConnectionString();
                if (string.IsNullOrEmpty(sourceDb))
                {
                    _logger.LogError("Database connection string is null or empty");
                    return false;
                }

                var dbPath = sourceDb.Replace("Data Source=", "").Trim();

                // Close all connections to the database
                await _context.Database.CloseConnectionAsync();

                // Copy backup file to database location
                File.Copy(backupPath, dbPath, overwrite: true);
                
                _logger.LogInformation("Database restored successfully from: {BackupPath}", backupPath);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to restore database from backup: {BackupPath}", backupPath);
                return false;
            }
        }

        public async Task<bool> CreateScheduledBackupAsync()
        {
            try
            {
                var backupDir = Path.Combine(AppContext.BaseDirectory, "Backups");
                var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                var backupPath = Path.Combine(backupDir, $"WooriOptical_Backup_{timestamp}.db");

                return await CreateBackupAsync(backupPath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create scheduled backup");
                return false;
            }
        }
    }
}
