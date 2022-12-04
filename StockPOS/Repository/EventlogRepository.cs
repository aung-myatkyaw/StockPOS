using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using StockPOS.Models;
using StockPOS.Util;

namespace StockPOS.Repository
{
    public class EventlogRepository: RepositoryBase<Eventlog>, IEventlogRepository
    {
        public EventlogRepository(StockPOSContext repositoryContext)
            :base(repositoryContext)
        {
        }

        public async Task AddEventLog(EventLogType LogTypeEnum, string LogMessage, string ErrMessage, string FormName = "")
        {
            EventLogType LogType = LogTypeEnum;
             
            string LoginUserID = "0";
            string Source = "";
            
            ClaimsIdentity objclaim = RepositoryContext._httpContextAccessor.HttpContext.User.Identities.Last();
            if(objclaim != null)
            {
                if(objclaim.FindFirst("UserID") != null) LoginUserID = objclaim.FindFirst("UserID").Value;
            } 

            Source = RepositoryContext._httpContextAccessor.HttpContext.Request.Path.ToString();
            if(RepositoryContext._actionContextAccessor.ActionContext != null && FormName == "")
                FormName = RepositoryContext._actionContextAccessor.ActionContext.ActionDescriptor.DisplayName.ToString().Replace("StockPOS.Controllers.", "").Replace("(StockPOS)", "");
        
            if (LogMessage != "" || ErrMessage != "")
            {
                try
                {
                    var newobj = new Eventlog
                    {
                        LogType = LogType,
                        LogDateTime = DateTime.UtcNow,
                        LogMessage = LogMessage,
                        ErrorMessage = ErrMessage,
                        Source = Source,
                        FormName = FormName.Trim()
                    };

                    if (LoginUserID != "0")
                    {
                        newobj.UserId = int.Parse(LoginUserID);
                    }

                    await CreateAsync(newobj);
                    await SaveAsync();

                }
                catch (Exception ex)
                {
                    Globalfunction.WriteSystemLog("SQL Exception :" + ex.Message);
                }
            }
        }
        
        public async Task Insert(dynamic obj)
        {
            string LogMessage = "";
            LogMessage += "Created :\r\n";
            LogMessage += this.SetOldObjectToString(obj);
            await AddEventLog(EventLogType.Insert, LogMessage, "");
        }

        public async Task Update(dynamic obj)
        {
            string LogMessage = "";
            LogMessage += "Updated :\r\n";
            LogMessage += this.SetOldObjectToString(obj);
            await AddEventLog(EventLogType.Update, LogMessage, "");
        }

        public async Task Delete(dynamic obj)
        {
            string LogMessage = "";
            LogMessage += "Deleted :\r\n";
            LogMessage += this.SetOldObjectToString(obj);
            await AddEventLog(EventLogType.Delete, LogMessage, "");
        }

        public async Task Error(string LogMessage, string ErrMessage)
        {
            await AddEventLog(EventLogType.Error, LogMessage, ErrMessage);
        }

        public async Task Info(string LogMessage)
        {
            await AddEventLog(EventLogType.Info, LogMessage, "");
        }

        public async Task Warning(string LogMessage)
        {
            await AddEventLog(EventLogType.Warning, LogMessage, "");
        }

    }
}