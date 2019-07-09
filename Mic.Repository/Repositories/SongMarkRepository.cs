using Dapper;
using Mic.Entity;
using Mic.Repository.Dapper;
using Mic.Repository.IRepositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mic.Repository.Repositories
{
    public class SongMarkRepository
    {
        private DapperHelper<SqlConnection> helper;
        public SongMarkRepository()
        {
            helper = new DapperHelper<SqlConnection>(WebConfig.SqlConnection);
        }
        public Tuple<bool, SongMarkEntity> AddOrUpdateSongMark(SongMarkEntity songMarkEntity)
        {
            SongMarkEntity updateEntity = songMarkEntity;
            int result = 0;
            if (songMarkEntity.Id > 0)
            {
                result = helper.Execute($@"update SongMark set MarkName='{songMarkEntity.MarkName}' where Id={songMarkEntity.Id}");
                updateEntity = songMarkEntity;
            }
            else
            {
                var p = new DynamicParameters();
                p.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                result = helper.Execute($@"insert into SongMark (MarkName) values ('{songMarkEntity.MarkName}');SELECT @Id=SCOPE_IDENTITY()",p);
                var id = p.Get<int>("@Id");
                songMarkEntity.Id = id;
            }
            return Tuple.Create(result > 0 ? true : false,updateEntity);
        }

        public bool DeleteSongMark(int id)
        {
            int result = helper.Execute($@"delete from SongMark where Id={id}");
            return result > 0 ? true : false;
        }

        public List<SongMarkEntity> GetSongMakList()
        {
            return helper.Query<SongMarkEntity>($@"select * from SongMark;").ToList();
        }

    }
}
