using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfService2
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {

        [OperationContract]
        string GetData(int value);

        /// <summary>
        /// Метод для получения номера игрока
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        int GetNumber();

        /// <summary>
        /// Метод для записи новых данных в БД
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [OperationContract]
        string WriteData(string value);

        /// <summary>
        /// Метод для отправки данных остальным игрокам
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [OperationContract]
        string SendData(int value);


        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        // TODO: Add your service operations here
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
