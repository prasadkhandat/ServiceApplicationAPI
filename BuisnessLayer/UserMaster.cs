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
    public class UserMaster
    {
        DataSQL objSQL;
        public bool isError = false;
        public string errorMessage = string.Empty;
        public UserMaster(DataSQL objSQL)
        {
            this.objSQL = objSQL;
        }

        public List<UserEntity> getAllUsers()
        {
            List<UserEntity> data = new List<UserEntity>();
            try
            {
                DataTable dt = objSQL.getData("select * from user_master order by DateCreated");
                foreach (DataRow dr in dt.Rows)
                {
                    UserEntity oe = new UserEntity();
                    oe.user_id = dr["User_ID"].ToString();
                    oe.user_name = dr["UName"].ToString();
                    //oe.password = dr["Password"].ToString();
                    oe.first_Name = dr["FirstName"].ToString();
                    oe.last_name = dr["LastName"].ToString();
                    oe.phone = dr["Phone"].ToString();
                    oe.email = dr["Email"].ToString();
                    oe.role = dr["Role"].ToString();
                    data.Add(oe);
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                isError = true;
                FileLogger.AppendLog("Error", "BuisnessLayer >> UserMaster >> getAllUsers", ex.Message);
            }
            if (errorMessage.Length == 0)
            {
                errorMessage = objSQL.errorMessage;
                isError = objSQL.isError;
            }
            return data;
        }

        public UserEntity getSingleUser(int id)
        {
            UserEntity data = new UserEntity();
            try
            {
                DataTable dt = objSQL.getData("select * from user_master where User_ID=" + id);
                foreach (DataRow dr in dt.Rows)
                {
                    data.user_id = dr["User_ID"].ToString();
                    data.user_name = dr["UName"].ToString();
                    //oe.password = dr["Password"].ToString();
                    data.first_Name = dr["FirstName"].ToString();
                    data.last_name = dr["LastName"].ToString();
                    data.phone = dr["Phone"].ToString();
                    data.email = dr["Email"].ToString();
                    data.role = dr["Role"].ToString();
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                isError = true;
                FileLogger.AppendLog("Error", "BuisnessLayer >> UserMaster >> getSingleUser", ex.Message);
            }
            if (errorMessage.Length == 0)
            {
                errorMessage = objSQL.errorMessage;
                isError = objSQL.isError;
            }
            return data;
        }

        public bool insertNewUser(UserEntity entity)
        {
            bool flag = true;
            try
            {
                string query = "INSERT INTO `user_master` ( `UName`, `Password`, `FirstName`, `LastName`, `Phone`, `Email`, `Role`) VALUE ('" + entity.user_name + "','" + entity.password + "','" + entity.first_Name + "', '" + entity.last_name + "', '" + entity.phone + "', '" + entity.email + "', '" + entity.role + "')";
                objSQL.getData(query);
            }
            catch (Exception ex)
            {
                flag = false;
                isError = true;
                FileLogger.AppendLog("Error", "BuisnessLayer >> UserMaster >> insertNewUser", ex.Message);
            }
            if (errorMessage.Length == 0)
            {
                errorMessage = objSQL.errorMessage;
                isError = objSQL.isError;
                flag = !isError;
            }
            return flag;
        }

        public bool updateUserEntry(UserEntity entity)
        {
            bool flag = true;
            try
            {
                string query = "update `user_master` set  `UName`='" + entity.user_name + "', `FirstName`='" + entity.first_Name + "', `LastName`='" + entity.last_name + "', `Phone`='" + entity.phone + "',`Email`='" + entity.email + "',`Role`='" + entity.role + "' where User_ID=" + entity.user_id;
                objSQL.getData(query);
            }
            catch (Exception ex)
            {
                flag = false;
                isError = true;
                FileLogger.AppendLog("Error", "BuisnessLayer >> UserMaster >> updateUserEntry", ex.Message);
            }
            if (errorMessage.Length == 0)
            {
                errorMessage = objSQL.errorMessage;
                isError = objSQL.isError;
                flag = !isError;
            }
            return flag;
        }

        public bool deleteUser(int id)
        {
            bool flag = true;
            try
            {
                objSQL.getData("delete from user_master where User_ID=" + id);
            }
            catch (Exception ex)
            {
                flag = false;
                errorMessage = ex.Message;
                isError = true;
                FileLogger.AppendLog("Error", "BuisnessLayer >> UserMaster >> deleteUser", ex.Message);
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

    public class UserEntity
    {
        public string user_id{ get; set; }
        public string user_name { get; set; }
        public string password { get; set; }
        public string first_Name { get; set; }
        public string last_name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string role { get; set; }        
    }
}
