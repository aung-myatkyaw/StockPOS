using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StockPOS.Models;

namespace StockPOS.Repository
{
    public interface IEventlogRepository: IRepositoryBase<Eventlog>
    {
        Task AddEventLog(EventLogType LogTypeEnum, string LogMessage, string ErrMessage, string FormName="");
        Task Insert(dynamic obj);
        Task Update(dynamic obj);
        Task Delete(dynamic obj);
        Task Info(string LogMessage);
        Task Error(string LogMessage, string ErrMessage);
        Task Warning(string LogMessage);
    }
}