using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using Weather.Data.Enums;
using Weather.Data.Interface;
using Weather.Data.Repository;
using Weather.Models;
using Microsoft.EntityFrameworkCore;

namespace Weather.Data.UnitOfWork
{
    public class GenericUoW : IGenericUoW
    {
        private readonly DataBaseContext _dbContext = null;
        public Dictionary<Type, object> repositories = new Dictionary<Type, object>();



        public GenericUoW(DataBaseContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public DataTable GetFromSp(string[,] Parametr, string NameSp)
        {
            _dbContext.OpenConnection();

            using (DbCommand cmd = _dbContext.Command())
            {
                cmd.CommandText = NameSp;
                cmd.CommandTimeout = 120;
                cmd.CommandType = CommandType.StoredProcedure;

                int countParametr = Parametr.GetLength(0);

                for (int i = 0; i < countParametr; i++)
                {
                    string name = Parametr[i, 0];
                    string value = Parametr[i, 1];

                    object typedValue;

                    if (int.TryParse(value, out int intVal))
                    {
                        typedValue = intVal;
                    }
                    else if (DateTime.TryParse(value, out DateTime dateVal))
                    {
                        typedValue = dateVal;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(value))
                            typedValue = DBNull.Value;
                        else
                            typedValue = value;
                    }

                    cmd.Parameters.Add(new SqlParameter(name, typedValue));
                }

                DataTable dataTable = new DataTable();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader != null && reader.HasRows)
                    {
                        dataTable.Load(reader);
                    }
                }
                return dataTable;
            }
        }


        public IRepository<T> Repository<T>() where T : class
        {
            if (repositories.Keys.Contains(typeof(T)))
            {
                return repositories[typeof(T)] as IRepository<T>;
            }

            IRepository<T> repo = new GenericRepository<T>(_dbContext);
            repositories.Add(typeof(T), repo);
            return repo;
        }

        public bool Save(long UserId, EnuAction ActionType, string EntityName)
        {
            try
            {
                if (UserId != 0)
                    _dbContext.Add(new UserAction() { UserId = UserId, ActionType = ActionType, EntityName = EntityName });

                return Convert.ToBoolean(_dbContext.SaveChanges());
            }
            catch (Exception e)
            {
                //foreach (var eve in e.EntityValidationErrors)
                //{
                //    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                //        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                //    foreach (var ve in eve.ValidationErrors)
                //    {
                //        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                //            ve.PropertyName, ve.ErrorMessage);
                //    }
                //}
                return false;
            }
        }
    }
}
