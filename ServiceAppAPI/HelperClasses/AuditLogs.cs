using DataLayer;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ServiceAppAPI.HelperClasses
{
    public class AuditLogs
    {
        //static string MongoConnectionString = "mongodb://54.227.245.138:27017";
        //static string MongoConnectionString = "mongodb://10.146.14.71:27017";
        static string MongoConnectionString = "mongodb://localhost:27017";
        static string DbName = "POSession";
        public static async Task insertRequestCall(string AuthToken, string URL, string RequestParameters, string Response, string ResponseStatus, string ContentSize, string resHeaders, string ReqSentTime, string reqCompletedTime, int resHttpCode)
        {
            try
            {
                var client = new MongoClient(MongoConnectionString);
                var database = client.GetDatabase(DbName);
                var collection = database.GetCollection<BsonDocument>("apicalls");
                //var document = new BsonDocument { { "Email", "abhay.dare@gmail.com" }, { "Name"," Abhay Dare " } };
                var document = new BsonDocument {
                    { "AuthToken",AuthToken},
                    { "APIUrl",URL},
                    { "RequestParameters",RequestParameters},
                    { "ResponseContent",Response},
                    { "ResponseStatus",ResponseStatus},
                    { "ResponseContentSize",ContentSize},
                    { "ResponseHeaders",resHeaders},
                    { "RequestSentTime",ReqSentTime},
                    { "RequestCompletedTime",reqCompletedTime},
                    { "ResHttpCode",resHttpCode }
                };
                await Task.Run(() => collection.InsertOneAsync(document));
            }
            catch (Exception ex)
            { }

        }

        public static async Task insertUtilityLogs(string UtilityName, string MasterCustID, string TO, string SentDate, string PracticeID, string MessageID, string Message, string isSendNow, string UserName)
        {
            try
            {
                var client = new MongoClient(MongoConnectionString);
                var database = client.GetDatabase(DbName);
                var collection = database.GetCollection<BsonDocument>("utilitylogs");
                var document = new BsonDocument {
                    { "MasterCustomerID",MasterCustID},
                    { "TO",TO},
                    { "SentDate",SentDate},
                    { "PracticeID",PracticeID},
                    { "MessageID",MessageID},
                    { "Message",Message},
                    { "IsSendNow",isSendNow},
                    { "UtilityName",UtilityName},
                    { "UserID",UserName }
                };
                await Task.Run(() => collection.InsertOneAsync(document));
            }
            catch (Exception ex)
            { }
        }

        public static async Task insertCustomerAuthCall(string UserID, string URL,string EmailID, string PhoneNumber, string AuthToken, string RemoteIP,string UID,string role)
        {
            try
            {
                var client = new MongoClient(MongoConnectionString);
                var database = client.GetDatabase(DbName);
                var collection = database.GetCollection<BsonDocument>("customeraccess");
               
                var document = new BsonDocument {
                    { "AuthToken",AuthToken},
                    { "UserName",UserID},
                    { "UID",UID},
                    { "role",role},
                    { "URL",URL},
                    { "EmailID",EmailID},
                    { "PhoneNumber",PhoneNumber},
                    { "RemoteIP",RemoteIP},
                    { "TokenGenTime",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},
                    { "TokenExpTime",DateTime.Now.AddHours(24).ToString("yyyy-MM-dd HH:mm:ss")}
                    
                };
                await Task.Run(() => collection.InsertOneAsync(document));

            }
            catch (Exception ex)
            { }
        }

        public static async Task<DBServerIdentification> getIdentificationInfo(string AuthToken)
        {
            
            DBServerIdentification obj = new DBServerIdentification() {  Status = "Error" };
            LicenseLibrary.LicenseClass lc = new LicenseLibrary.LicenseClass();
            try
            {
                var client = new MongoClient(MongoConnectionString);
                var database = client.GetDatabase(DbName);
                var collection = database.GetCollection<BsonDocument>("customeraccess");
                var Filter = new BsonDocument("AuthToken", AuthToken);
                var list = await collection.Find(Filter).ToListAsync();

                foreach (var tmpData in list)
                {
                    obj.ObjectID = tmpData["_id"].ToString();                    
                    obj.EmailID = tmpData["EmailID"].ToString();
                    obj.UserID = tmpData["UserName"].ToString();
                    obj.UID= tmpData["UID"].ToString();
                    obj.Role = tmpData["role"].ToString();
                    DateTime fromDate = Convert.ToDateTime(tmpData["TokenGenTime"].ToString());
                    DateTime toDate = Convert.ToDateTime(tmpData["TokenExpTime"].ToString());
                                       
                    if ((toDate - fromDate).TotalMinutes > 0)
                        obj.Status = "Success";
                    else
                        obj.Status = "TokenExpired";
                }
            }
            catch (Exception ex) { }
            
            return obj;
        }

        public static async Task<DBServerIdentification> getIdentificationInfobyID(string AuthToken)
        {
            DBServerIdentification obj = new DBServerIdentification() { Status = "Error" };


            try
            {
                var client = new MongoClient(MongoConnectionString);
                var database = client.GetDatabase(DbName);
                var collection = database.GetCollection<BsonDocument>("customeraccess");
                var Filter = new BsonDocument("AuthToken", AuthToken);
                var list = await collection.Find(Filter).ToListAsync();

                foreach (var tmpData in list)
                {
                    obj.ObjectID = tmpData["_id"].ToString();


                    obj.EmailID = tmpData["EmailID"].ToString();
                    obj.UserID = tmpData["UserID"].ToString();

                    DateTime fromDate = Convert.ToDateTime(tmpData["TokenGenTime"].ToString());
                    DateTime toDate = Convert.ToDateTime(tmpData["TokenExpTime"].ToString());

                    obj.Status = "Success";

                }
            }
            catch (Exception ex) { }

            return obj;
        }

    }

    public class DBServerIdentification
    {
        public string ObjectID { get; set; }
        public string UserID { get; set; }
        public string EmailID { get; set; }
        public string Status { get; set; }
        public string UID { get; set; }
        public string Role { get; set; }        
    }
}