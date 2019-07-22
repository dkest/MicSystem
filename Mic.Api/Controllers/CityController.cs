using Mic.Api.Models;
using Mic.Entity;
using Mic.Repository;
using Mic.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Mic.Api.Controllers
{
    /// <summary>
    /// 省市县三级地区列表获取
    /// </summary>
    [RoutePrefix("city")]
    public class CityController : ApiController
    {
        private CityRepository cityRepository;
        public CityController()
        {
            cityRepository = ClassInstance<CityRepository>.Instance;
        }
        /// <summary>
        /// 获取省份
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("getProvinceList")]
        public ResponseResultDto<List<CityEntity>> GetProvinceList()
        {
            var result = cityRepository.GetProvinceList();
            return new ResponseResultDto<List<CityEntity>>
            {
                IsSuccess = true,
                ErrorMessage = string.Empty,
                Result = result
            };
        }
        /// <summary>
        /// 根据省份编码获取市列表
        /// </summary>
        /// <param name="provinceId">省份编码</param>
        /// <returns></returns>
        [HttpGet, Route("getCityList")]
        public ResponseResultDto<List<CityEntity>> GetCityList(int provinceId)
        {
            var result = cityRepository.GetCityList(provinceId);
            return new ResponseResultDto<List<CityEntity>>
            {
                IsSuccess = true,
                ErrorMessage = string.Empty,
                Result = result
            };
        }
        /// <summary>
        /// 根据市编码获取县区列表
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        [HttpGet, Route("getCountyList")]
        public ResponseResultDto<List<CityEntity>> GetCountyList(int cityId)
        {
            var result = cityRepository.GetCountyList(cityId);
            return new ResponseResultDto<List<CityEntity>>
            {
                IsSuccess = true,
                ErrorMessage = string.Empty,
                Result = result                
            };
        }
        
    }
}
