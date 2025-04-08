namespace Fabric.Api.Dtos
{
    public class EvaluateContractRequest
    {
        public string Contract { get; }

        public EvaluateContractRequest(string contract)
        {
            Contract = contract;
        }
    }
}
