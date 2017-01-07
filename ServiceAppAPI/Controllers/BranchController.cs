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
    public class BranchController : ApiController
    {
        // GET: api/Branch
        public async Task<IHttpActionResult> Get()
        {
            DBServerIdentification identificationInfo = await AuditLogs.getIdentificationInfo(Request.Headers.Authorization.Parameter);

            if (identificationInfo.Role.Equals(Roles.Admin.ToString()) || identificationInfo.Role.Equals(Roles.SuperAdmin.ToString()))
            {
                DataSQL objSQL = new DataSQL();
                BranchMaster officeMaster = new BranchMaster(objSQL);
                List<BranchEntity> data = officeMaster.getAllbranches();
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

        // GET: api/Branch/5
        public async Task<IHttpActionResult> Get(int id)
        {
            DBServerIdentification identificationInfo = await AuditLogs.getIdentificationInfo(Request.Headers.Authorization.Parameter);

            if (identificationInfo.Role.Equals(Roles.Admin.ToString()) || identificationInfo.Role.Equals(Roles.SuperAdmin.ToString()))
            {
                DataSQL objSQL = new DataSQL();
                BranchMaster officeMaster = new BranchMaster(objSQL);
                BranchEntity data = officeMaster.getSingleBranch(id);
                if (officeMaster.isError)
                    //throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, officeMaster.errorMessage));
                else if (data.branch_id == null || data.branch_id.Length == 0)
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NoContent));
                else
                    return Ok(data);
            }
            else
                return Unauthorized();
        }

        // POST: api/Branch
        public async Task<IHttpActionResult> Post([FromBody]BranchEntity value)
        {
            DBServerIdentification identificationInfo = await AuditLogs.getIdentificationInfo(Request.Headers.Authorization.Parameter);

            if (identificationInfo.Role.Equals(Roles.Admin.ToString()) || identificationInfo.Role.Equals(Roles.SuperAdmin.ToString()))
            {
                DataSQL objSQL = new DataSQL();
                BranchMaster officeMaster = new BranchMaster(objSQL);
                if (officeMaster.insertNewBranch(value))
                {
                    return Created("/branch/{ID}", new { status = "new resource created" });
                }
                else
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, officeMaster.errorMessage));
                }
            }
            else
                return Unauthorized();
        }

        // PUT: api/Branch/5
        public async Task<IHttpActionResult> Put(int id, [FromBody]BranchEntity value)
        {
            DBServerIdentification identificationInfo = await AuditLogs.getIdentificationInfo(Request.Headers.Authorization.Parameter);

            if (identificationInfo.Role.Equals(Roles.Admin.ToString()) || identificationInfo.Role.Equals(Roles.SuperAdmin.ToString()))
            {
                DataSQL objSQL = new DataSQL();
                BranchMaster officeMaster = new BranchMaster(objSQL);
                value.branch_id = id.ToString();
                if (officeMaster.updateBranchEntry(value))
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

        // DELETE: api/Branch/5
        public async Task<IHttpActionResult> Delete(int id)
        {
            DBServerIdentification identificationInfo = await AuditLogs.getIdentificationInfo(Request.Headers.Authorization.Parameter);

            if (identificationInfo.Role.Equals(Roles.Admin.ToString()) || identificationInfo.Role.Equals(Roles.SuperAdmin.ToString()))
            {
                DataSQL objSQL = new DataSQL();
                BranchMaster officeMaster = new BranchMaster(objSQL);
                if (officeMaster.deleteBranch(id))
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
    }
}
