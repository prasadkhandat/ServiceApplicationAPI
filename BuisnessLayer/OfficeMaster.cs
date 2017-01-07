using DataLayer;
using LogLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;


namespace BuisnessLayer
{
    public class OfficeMaster
    {
        DataSQL objSQL;
        public bool isError = false;
        public string errorMessage = string.Empty;
        public OfficeMaster(DataSQL objSQL)
        {
            this.objSQL = objSQL;
        }

        public List<OfficeEntity> getAllOffices()
        {
            List<OfficeEntity> data = new List<OfficeEntity>();
            try
            {
                DataTable dt =  objSQL.getData("select * from office_master order by creation_time");
                foreach (DataRow dr in dt.Rows)
                {
                    OfficeEntity oe = new OfficeEntity();
                    oe.office_id = dr["Ofc_Id"].ToString();
                    oe.office_name = dr["Ofc_Name"].ToString();
                    oe.office_address = dr["Ofc_Address"].ToString();
                    oe.office_city = dr["Ofc_City"].ToString();
                    oe.office_state = dr["Ofc_State"].ToString();
                    oe.office_zip = dr["Ofc_Zip"].ToString();
                    data.Add(oe);
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                isError = true;
                FileLogger.AppendLog("Error", "BuisnessLayer >> OfficeMaster >> getAllOffices", ex.Message);
            }
            if (errorMessage.Length == 0)
            {
                errorMessage = objSQL.errorMessage;
                isError = objSQL.isError;
            }
            return data;
        }

        public OfficeEntity getSingleLocation(int id)
        {
            OfficeEntity data = new OfficeEntity();
            try
            {
                DataTable dt = objSQL.getData("select * from office_master where ofc_ID=" + id);
                foreach (DataRow dr in dt.Rows)
                {

                    data.office_id = dr["Ofc_Id"].ToString();
                    data.office_name = dr["Ofc_Name"].ToString();
                    data.office_address = dr["Ofc_Address"].ToString();
                    data.office_city = dr["Ofc_City"].ToString();
                    data.office_state = dr["Ofc_State"].ToString();
                    data.office_zip = dr["Ofc_Zip"].ToString();

                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                isError = true;
                FileLogger.AppendLog("Error", "BuisnessLayer >> OfficeMaster >> getSingleLocation", ex.Message);
            }
            if (errorMessage.Length == 0)
            {
                errorMessage = objSQL.errorMessage;
                isError = objSQL.isError;
            }
            return data;
        }

        public bool inserNewOffice(OfficeEntity entity)
        {
            bool flag = true;
            try
            {
                string query = "INSERT INTO `office_master` ( `Ofc_Name`, `Ofc_Address`, `Ofc_City`, `Ofc_State`, `Ofc_Zip`) VALUE ('"+entity.office_name+ "','"+entity.office_address+ "','"+entity.office_city+ "', '"+entity.office_state+ "', '"+entity.office_zip+"')";
                objSQL.getData(query);
            }
            catch (Exception ex)
            {
                flag = false;
                isError = true;
                FileLogger.AppendLog("Error", "BuisnessLayer >> OfficeMaster >> getAllOffices", ex.Message);
            }
            if (errorMessage.Length == 0)
            {
                errorMessage = objSQL.errorMessage;
                isError = objSQL.isError;
                flag = !isError;
            }
            return flag;
        }

        public bool addbranchToOffice(int office_id,int branch_id)
        {
            bool flag = true;
            try
            {
                string query = "INSERT INTO `office_branch_master` ( `Ofc_Id`, `Branch_Id`) VALUE (" + office_id + "," + branch_id + ")";
                objSQL.getData(query);
            }
            catch (Exception ex)
            {
                flag = false;
                isError = true;
                FileLogger.AppendLog("Error", "BuisnessLayer >> OfficeMaster >> addbranchToOffice", ex.Message);
            }
            if (errorMessage.Length == 0)
            {
                errorMessage = objSQL.errorMessage;
                isError = objSQL.isError;
                flag = !isError;
            }
            return flag;
        }

        public bool removeBranchfromOffice(int office_id,int branch_id)
        {
            bool flag = true;
            try
            {
                string query = "delete from  `office_branch_master` where  `Ofc_Id`=" + office_id + " and  `Branch_Id`=" + branch_id;
                objSQL.getData(query);
            }
            catch (Exception ex)
            {
                flag = false;
                isError = true;
                FileLogger.AppendLog("Error", "BuisnessLayer >> OfficeMaster >> removeBranchfromOffice", ex.Message);
            }
            if (errorMessage.Length == 0)
            {
                errorMessage = objSQL.errorMessage;
                isError = objSQL.isError;
                flag = !isError;
            }
            return flag;
        }

        public bool updateOfficeEntry(OfficeEntity entity)
        {
            bool flag = true;
            try
            {
                string query = "update `office_master` set  `Ofc_Name`='" + entity.office_name + "', `Ofc_Address`='" + entity.office_address + "', `Ofc_City`='" + entity.office_city + "', `Ofc_State`='" + entity.office_state + "', `Ofc_Zip`='" + entity.office_zip + "' where ofc_ID=" + entity.office_id;
                objSQL.getData(query);
            }
            catch (Exception ex)
            {
                flag = false;
                isError = true;
                FileLogger.AppendLog("Error", "BuisnessLayer >> OfficeMaster >> updateOfficeEntry", ex.Message);
            }
            if (errorMessage.Length == 0)
            {
                errorMessage = objSQL.errorMessage;
                isError = objSQL.isError;
                flag = !isError;
            }
            return flag;
        }

        public bool deleteOffice(int id)
        {
            bool flag = true;            
            try
            {
               objSQL.getData("delete from office_master where ofc_ID=" + id);                
            }
            catch (Exception ex)
            {
                flag = false;
                errorMessage = ex.Message;
                isError = true;
                FileLogger.AppendLog("Error", "BuisnessLayer >> OfficeMaster >> getSingleLocation", ex.Message);
            }
            if (errorMessage.Length == 0)
            {
                errorMessage = objSQL.errorMessage;
                isError = objSQL.isError;
                flag = !isError;
            }
            return flag;
        }
    }

    public class OfficeEntity
    {
        public string office_name { get; set; }
        public string office_address { get; set; }
        public string office_city { get; set; }
        public string office_state { get; set; }
        public string office_zip { get; set; }
        public string office_id { get; set; }
    }

    public class OfficeBranchEntity
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "office id required")]
        public string office_id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "branch name required")]
        public string branch_id{ get; set; }
    }
}
