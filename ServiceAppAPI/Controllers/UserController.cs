using BuisnessLayer;
using DataLayer;
using ServiceAppAPI.Enums;
using ServiceAppAPI.HelperClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ServiceAppAPI.Controllers
{
    [Authorize]
    public class UserController : ApiController
    {
        // GET: api/Branch
        public async Task<IHttpActionResult> Get()
        {
            DBServerIdentification identificationInfo = await AuditLogs.getIdentificationInfo(Request.Headers.Authorization.Parameter);

            if (identificationInfo.Role.Equals(Roles.Admin.ToString()) || identificationInfo.Role.Equals(Roles.SuperAdmin.ToString()))
            {
                DataSQL objSQL = new DataSQL();
                UserMaster userMaster = new UserMaster(objSQL);
                List<UserEntity> data = userMaster.getAllUsers();
                if (userMaster.isError)
                    //throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, userMaster.errorMessage));
                else if (data.Count == 0)
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NoContent));
                else
                    return Ok(data);
            }
            else
                return Unauthorized();
        }

        // GET: api/Branch/5
        public async Task<IHttpActionResult> Get(int id)
        {
            DBServerIdentification identificationInfo = await AuditLogs.getIdentificationInfo(Request.Headers.Authorization.Parameter);

            if (identificationInfo.Role.Equals(Roles.Admin.ToString()) || identificationInfo.Role.Equals(Roles.SuperAdmin.ToString()))
            {
                DataSQL objSQL = new DataSQL();
                UserMaster userMaster = new UserMaster(objSQL);
                UserEntity data = userMaster.getSingleUser(id);
                if (userMaster.isError)
                    //throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, userMaster.errorMessage));
                else if (data.user_id == null || data.user_id.Length == 0)
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NoContent));
                else
                    return Ok(data);
            }
            else
                return Unauthorized();
        }

        // POST: api/Branch
        public async Task<IHttpActionResult> Post([FromBody]UserEntity value)
        {
            DBServerIdentification identificationInfo = await AuditLogs.getIdentificationInfo(Request.Headers.Authorization.Parameter);

            if (identificationInfo.Role.Equals(Roles.Admin.ToString()) || identificationInfo.Role.Equals(Roles.SuperAdmin.ToString()))
            {
                DataSQL objSQL = new DataSQL();
                UserMaster userMaster = new UserMaster(objSQL);
                if (userMaster.insertNewUser(value))
                {
                    return Created("/user/{ID}", new { status = "new resource created" });
                }
                else
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, userMaster.errorMessage));
                }
            }
            else
                return Unauthorized();
        }

        // PUT: api/Branch/5
        public async Task<IHttpActionResult> Put(int id, [FromBody]UserEntity value)
        {
            DBServerIdentification identificationInfo = await AuditLogs.getIdentificationInfo(Request.Headers.Authorization.Parameter);

            if (identificationInfo.Role.Equals(Roles.Admin.ToString()) || identificationInfo.Role.Equals(Roles.SuperAdmin.ToString()))
            {
                DataSQL objSQL = new DataSQL();
                UserMaster userMaster = new UserMaster(objSQL);
                value.user_id = id.ToString();
                if (userMaster.updateUserEntry(value))
                {
                    return Ok("Resource Updated successfully.");
                }
                else
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, userMaster.errorMessage));
                }
            }
            else
                return Unauthorized();
        }

        // DELETE: api/Branch/5
        public async Task<IHttpActionResult> Delete(int id)
        {
            DBServerIdentification identificationInfo = await AuditLogs.getIdentificationInfo(Request.Headers.Authorization.Parameter);

            if (identificationInfo.Role.Equals(Roles.Admin.ToString()) || identificationInfo.Role.Equals(Roles.SuperAdmin.ToString()))
            {
                DataSQL objSQL = new DataSQL();
                UserMaster userMaster = new UserMaster(objSQL);
                if (userMaster.deleteUser(id))
                {
                    return Ok("Resource deleted.");
                }
                else
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, userMaster.errorMessage));
                }
            }
            else
                return Unauthorized();
        }
    }
}
