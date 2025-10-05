using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Weather.Data.Enums;
using Weather.Data.Interface;
using Weather.Data.Repository;
using Weather.Models;

namespace Weather.Data.UnitOfWork
{
    public class GenericUoW : IGenericUoW
    {
        private readonly DataBaseContext _dbContext;
        private readonly Dictionary<Type, object> _repositories = new();

        public GenericUoW(DataBaseContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public IRepository<T> Repository<T>() where T : class
        {
            var type = typeof(T);
            if (!_repositories.ContainsKey(type))
            {
                var repoInstance = new GenericRepository<T>(_dbContext);
                _repositories[type] = repoInstance;
            }
            return (IRepository<T>)_repositories[type];
        }

        /// <summary>
        /// ذخیره تغییرات DbContext و ثبت اکشن کاربر به صورت async
        /// </summary>
        public async Task<bool> SaveAsync(long userId, EnuAction actionType, string entityName)
        {
            if (userId <= 0 || string.IsNullOrWhiteSpace(entityName))
                return false;

            try
            {
                // ثبت اکشن کاربر
                var existingAction = await _dbContext.UserAction
                    .FirstOrDefaultAsync(u => u.UserId == userId &&
                                              u.EntityName == entityName &&
                                              u.ActionType == actionType);

                if (existingAction is null)
                {
                    await _dbContext.UserAction.AddAsync(new UserAction
                    {
                        UserId = userId,
                        ActionType = actionType,
                        EntityName = entityName,
                        InsertDate = DateTime.UtcNow
                    });
                }
                else
                {
                    existingAction.InsertDate = DateTime.UtcNow;
                    _dbContext.Entry(existingAction).State = EntityState.Modified;
                }

                // ذخیره همه تغییرات موجودیت‌ها و UserAction
                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"DbUpdateException in SaveAsync: {ex.InnerException?.Message ?? ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in SaveAsync: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// ذخیره تغییرات DbContext و ثبت اکشن کاربر به صورت sync
        /// </summary>
        public bool Save(long userId, EnuAction actionType, string entityName)
        {
            if (userId <= 0 || string.IsNullOrWhiteSpace(entityName))
                return false;

            try
            {
                var existingAction = _dbContext.UserAction
                    .FirstOrDefault(u => u.UserId == userId &&
                                         u.EntityName == entityName &&
                                         u.ActionType == actionType);

                if (existingAction is null)
                {
                    _dbContext.UserAction.Add(new UserAction
                    {
                        UserId = userId,
                        ActionType = actionType,
                        EntityName = entityName,
                        InsertDate = DateTime.UtcNow
                    });
                }
                else
                {
                    existingAction.InsertDate = DateTime.UtcNow;
                    _dbContext.Entry(existingAction).State = EntityState.Modified;
                }

                // ذخیره همه تغییرات موجودیت‌ها و UserAction
                _dbContext.SaveChanges();

                return true;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"DbUpdateException in Save: {ex.InnerException?.Message ?? ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in Save: {ex.Message}");
                return false;
            }
        }


        /// <summary>
        /// جدا کردن همه‌ی Entityهای ردیابی‌شده از Context
        /// </summary>
        public void DetachAllEntities()
        {
            foreach (var entry in _dbContext.ChangeTracker.Entries())
                entry.State = EntityState.Detached;
        }

        /// <summary>
        /// اجرای Stored Procedure و دریافت نتیجه در قالب DataTable
        /// </summary>
        public DataTable GetFromSp(string[,] parameters, string spName)
        {
            _dbContext.OpenConnection();

            using (DbCommand cmd = _dbContext.Command())
            {
                cmd.CommandText = spName;
                cmd.CommandTimeout = 120;
                cmd.CommandType = CommandType.StoredProcedure;

                int count = parameters.GetLength(0);
                for (int i = 0; i < count; i++)
                {
                    string name = parameters[i, 0];
                    string value = parameters[i, 1];

                    object typedValue = int.TryParse(value, out int intVal) ? intVal :
                                        DateTime.TryParse(value, out DateTime dateVal) ? dateVal :
                                        string.IsNullOrEmpty(value) ? DBNull.Value : value;

                    cmd.Parameters.Add(new SqlParameter(name, typedValue));
                }

                DataTable dt = new();
                using var reader = cmd.ExecuteReader();
                if (reader != null && reader.HasRows)
                    dt.Load(reader);

                return dt;
            }
        }
    }
}
