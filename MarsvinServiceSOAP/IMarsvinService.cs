using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace MarsvinServiceSOAP
{
    [ServiceContract]
    public interface IMarsvinService
    {
        [OperationContract]
        IList<User> GetAllUsers();


        [OperationContract]
        IList<User> GetUserByPhoneNo(int phoneno);

        [OperationContract]
        IList<User> GetUserByMail(string email);

        [OperationContract]
        int AddUser(User user);

        [OperationContract]
        void DeleteUser(User user);


    }

}
