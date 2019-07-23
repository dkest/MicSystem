using Mic.Entity;
using Mic.Repository.Dapper;
using Mic.Repository.Utils;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace Mic.Repository.Repositories
{
    public class StoreStaffApiRepository
    {
        DapperHelper<SqlConnection> helper;
        public StoreStaffApiRepository()
        {
            helper = new DapperHelper<SqlConnection>(WebConfig.SqlConnection);
        }

        public bool AddStoreStaff(string Id, StoreStaffParam staff)
        {
            // 先根据token，Id,再获取，获取 StoreCode，然后再添加Staff
            return true;
        }
    }
}
