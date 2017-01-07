using DataLayer;
using LogLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLayer
{

    public class BranchMaster
    {
        DataSQL objSQL;
        public bool isError = false;
        public string errorMessage = string.Empty;
        public BranchMaster(DataSQL objSQL)
        {
            this.objSQL = objSQL;
        }

        public List<BranchEntity> getAllbranches()
        {
            List<BranchEntity> data = new List<BranchEntity>();
            try
            {
                DataTable dt = objSQL.getData("select * from branch_master order by creation_time");
                foreach (DataRow dr in dt.Rows)
                {
                    BranchEntity oe = new BranchEntity();
                    oe.branch_id = dr["Branch_Id"].ToString();
                    oe.branch_name = dr["Branch_Name"].ToString();                    
                    data.Add(oe);
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                isError = true;
                FileLogger.AppendLog("Error", "BuisnessLayer >> BranchMaster >> getAllbranches", ex.Message);
            }
            if (errorMessage.Length == 0)
            {
                errorMessage = objSQL.errorMessage;
                isError = objSQL.isError;
            }
            return data;
        }

        public BranchEntity getSingleBranch(int id)
        {
            BranchEntity data = new BranchEntity();
            try
            {
                DataTable dt = objSQL.getData("select * from branch_master where Branch_Id=" + id);
                foreach (DataRow dr in dt.Rows)
                {
                    data.branch_id = dr["Branch_Id"].ToString();
                    data.branch_name = dr["Branch_Name"].ToString();
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                isError = true;
                FileLogger.AppendLog("Error", "BuisnessLayer >> BranchMaster >> getSingleBranch", ex.Message);
            }
            if (errorMessage.Length == 0)
            {
                errorMessage = objSQL.errorMessage;
                isError = objSQL.isError;
            }
            return data;
        }

        public bool insertNewBranch(BranchEntity entity)
        {
            bool flag = true;
            try
            {
                string query = "INSERT INTO `branch_master` ( `Branch_Name`) VALUE ('" + entity.branch_name + "')";
                objSQL.getData(query);
            }
            catch (Exception ex)
            {
                flag = false;
                isError = true;
                FileLogger.AppendLog("Error", "BuisnessLayer >> BranchMaster >> insertNewBranch", ex.Message);
            }
            if (errorMessage.Length == 0)
            {
                errorMessage = objSQL.errorMessage;
                isError = objSQL.isError;
                flag = !isError;
            }
            return flag;
        }

        public bool updateBranchEntry(BranchEntity entity)
        {
            bool flag = true;
            try
            {
                string query = "update `branch_master` set  `Branch_Name`='" + entity.branch_name + "'where Branch_Id=" + entity.branch_id;
                objSQL.getData(query);
            }
            catch (Exception ex)
            {
                flag = false;
                isError = true;
                FileLogger.AppendLog("Error", "BuisnessLayer >> branchMaster >> updateBranchEntry", ex.Message);
            }
            if (errorMessage.Length == 0)
            {
                errorMessage = objSQL.errorMessage;
                isError = objSQL.isError;
                flag = !isError;
            }
            return flag;
        }

        public bool deleteBranch(int id)
        {
            bool flag = true;
            try
            {
                objSQL.getData("delete from branch_master where Branch_Id=" + id);
            }
            catch (Exception ex)
            {
                flag = false;
                errorMessage = ex.Message;
                isError = true;
                FileLogger.AppendLog("Error", "BuisnessLayer >> branchmaster >> deleteBranch", ex.Message);
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

    public class BranchEntity
    {
        public string branch_id { get; set; }
        public string branch_name { get; set; }
    }
}
