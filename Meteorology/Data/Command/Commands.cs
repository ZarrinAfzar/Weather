using Weather.Data.UnitOfWork;
using Weather.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Data.Command
{
    public class Commands
    {
        private readonly GenericUoW _genericUoW;
        public Commands(GenericUoW genericUoW)
        {
            _genericUoW = genericUoW;
        }


        //public List<int> GetStationIdWithProjectId(int ProjectId)
        //{
        //    return _genericUoW.Repository<Station>().GetAll(n => n.ProjectId == ProjectId).Select(n => n.Id).ToList();
        //}

        //public List<int> AlarmWithStationId(int StationId)
        //{
        //   return _genericUoW.Repository<Alarm>().GetAll(n =>  n.StationId == StationId).Select(n => n.Id).ToList();
        //}
        //public List<int> AlarmWithStationIds(List<int> StationIds)
        //{
        //    return _genericUoW.Repository<Alarm>().GetAll(n => n.StationId == StationId).Select(n => n.Id).ToList();
        //}
    }
}
