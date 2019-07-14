using Mic.Entity;
using Mic.Repository.Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mic.Repository.Repositories
{
    public class CityRepository
    {
        DapperHelper<SqlConnection> helper;
        public CityRepository()
        {
            helper = new DapperHelper<SqlConnection>(WebConfig.SqlConnection);
        }

        public List<CityEntity> GetProvinceList()
        {
            return helper.Query<CityEntity>($@"select * from City where PId=1").ToList();
        }
        public List<CityEntity> GetCityList(int provinceId)
        {
            return helper.Query<CityEntity>($@"select * from City where PId={provinceId}").ToList();
        }
        public List<CityEntity> GetCountyList(int cityId)
        {
            return helper.Query<CityEntity>($@"select * from City where PId={cityId}").ToList();
        }

        public Tuple<string, string, string> GetDeatilCity(int province, int city, int county)
        {
            var provinceStr = helper.QueryScalar($@"select CityName from City where Id={province}");
            var cityStr = helper.QueryScalar($@"select CityName from City where Id={city}");
            var countyStr = helper.QueryScalar($@"select CityName from City where Id={county}");
            return Tuple.Create(provinceStr.ToString(), cityStr.ToString(), countyStr.ToString());
        }
    }
}
