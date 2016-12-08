using System.ServiceModel;

namespace CreditAssessment
{
    [ServiceContract]
    public interface ICreditAssessment
    {
        [OperationContract]
        bool IsOk(string counterpart);
    }
}
