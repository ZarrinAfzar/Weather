using Weather.Data.Enums;
using Weather.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Data.Interface
{
    public interface IGenericUoW
    {
        IRepository<T> Repository<T>() where T : class; 
        bool Save(long UserId, EnuAction ActionType,  string EntityName); 
    }
}
