using ServiceAppAPI.Enums;
using ServiceAppAPI.HelperClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BuisnessLayer;
using DataLayer;

namespace ServiceAppAPI.Controllers
{
    [Authorize]
    public class OfficeController : ApiController
    {
        // GET: api/Office
        public async Task<IHttpActionResult> Get()
        {            
            DBServerIdentification identificationInfo = await AuditLogs.getIdentificationInfo(Request.Headers.Authorization.Parameter);

            if (identificationInfo.Role.Equals(Roles.Admin.ToString()) || identificationInfo.Role.Equals(Roles.SuperAdmin.ToString()))
            {
                DataSQL objSQL = new DataSQL();
                OfficeMaster officeMaster = new OfficeMaster(objSQL);
                List<OfficeEntity> data = officeMaster.getAllOffices();
                if (officeMaster.isError)
                    //throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, officeMaster.errorMessage));
                else if (data.Count == 0)
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NoContent));
                else
                    return Ok(data);
            }
            else
                return Unauthorized();
        }

        // GET: api/Office/5
        public async Task<IHttpActionResult> Get(int id)
        {
            DBServerIdentification identificationInfo = await AuditLogs.getIdentificationInfo(Request.Headers.Authorization.Parameter);

            if (identificationInfo.Role.Equals(Roles.Admin.ToString()) || identificationInfo.Role.Equals(Roles.SuperAdmin.ToString()))
            {
                DataSQL objSQL = new DataSQL();
                OfficeMaster officeMaster = new OfficeMaster(objSQL);
                OfficeEntity data = officeMaster.getSingleLocation(id);
                if (officeMaster.isError)
                    //throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, officeMaster.errorMessage));
                else if (data.office_id == null || data.office_id.Length == 0)
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NoContent));
                else
                    return Ok(data);
            }
            else
                return Unauthorized();
        }

        // POST: api/Office
        public async Task<IHttpActionResult> Post([FromBody]OfficeEntity value)
        {
            DBServerIdentification identificationInfo = await AuditLogs.getIdentificationInfo(Request.Headers.Authorization.Parameter);

            if (identificationInfo.Role.Equals(Roles.Admin.ToString()) || identificationInfo.Role.Equals(Roles.SuperAdmin.ToString()))
            {
                DataSQL objSQL = new DataSQL();
                OfficeMaster officeMaster = new OfficeMaster(objSQL);
                if (officeMaster.inserNewOffice(value))
                {
                    return Created("/office/{ID}", new { status = "new resource created" });
                }
                else {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, officeMaster.errorMessage));
                }
            }
            else
                return Unauthorized();
        }

        // POST: api/Office/addbranch *Using action name*
        [HttpPost]
        [ActionName("addbranch")]
        public async Task<IHttpActionResult> Post([FromUri]int office_id,[FromUri]int branch_id )
        {
            DBServerIdentification identificationInfo = await AuditLogs.getIdentificationInfo(Request.Headers.Authorization.Parameter);

            if (identificationInfo.Role.Equals(Roles.Admin.ToString()) || identificationInfo.Role.Equals(Roles.SuperAdmin.ToString()))
            {
                DataSQL objSQL = new DataSQL();
                OfficeMaster officeMaster = new OfficeMaster(objSQL);
                if (officeMaster.addbranchToOffice(office_id,branch_id))
                {
                    return Created("/office/{ID}", new { status = "new resource created" });
                }
                else
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, officeMaster.errorMessage));
                }
            }
            else
                return Unauthorized();
        }

        // PUT: api/Office/5
        public async Task<IHttpActionResult> Put(int id, [FromBody]OfficeEntity value)
        {
            DBServerIdentification identificationInfo = await AuditLogs.getIdentificationInfo(Request.Headers.Authorization.Parameter);

            if (identificationInfo.Role.Equals(Roles.Admin.ToString()) || identificationInfo.Role.Equals(Roles.SuperAdmin.ToString()))
            {
                DataSQL objSQL = new DataSQL();
                OfficeMaster officeMaster = new OfficeMaster(objSQL);
                value.office_id = id.ToString();
                if (officeMaster.updateOfficeEntry(value))
                {
                    return Ok("Resource Updated successfully.");
                }
                else
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, officeMaster.errorMessage));
                }
            }
            else
                return Unauthorized();
        }

        // DELETE: api/Office/5
        public async Task<IHttpActionResult> Delete(int id)
        {
            DBServerIdentification identificationInfo = await AuditLogs.getIdentificationInfo(Request.Headers.Authorization.Parameter);

            if (identificationInfo.Role.Equals(Roles.Admin.ToString()) || identificationInfo.Role.Equals(Roles.SuperAdmin.ToString()))
            {
                DataSQL objSQL = new DataSQL();
                OfficeMaster officeMaster = new OfficeMaster(objSQL);               
                if (officeMaster.deleteOffice(id))
                {
                    return Ok("Resource deleted.");
                }
                else
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, officeMaster.errorMessage));
                }
            }
            else
                return Unauthorized();
        }

        // DELETE: api/Office/removebranch?office_id={office_id}&branch_id={branch_id}
        [ActionName("removebranch")]
        public async Task<IHttpActionResult> Delete([FromUri]int office_id,[FromUri]int branch_id)
        {
            DBServerIdentification identificationInfo = await AuditLogs.getIdentificationInfo(Request.Headers.Authorization.Parameter);

            if (identificationInfo.Role.Equals(Roles.Admin.ToString()) || identificationInfo.Role.Equals(Roles.SuperAdmin.ToString()))
            {
                DataSQL objSQL = new DataSQL();
                OfficeMaster officeMaster = new OfficeMaster(objSQL);
                if (officeMaster.removeBranchfromOffice(office_id,branch_id))
                {
                    return Created("/office/{ID}", new { status = "removed" });
                }
                else
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, officeMaster.errorMessage));
                }
            }
            else
                return Unauthorized();
        }
    }
}
