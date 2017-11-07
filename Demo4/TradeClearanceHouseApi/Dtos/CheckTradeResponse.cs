namespace TradeClearanceHouseApi.Dtos
{
    class CheckTradeResponse
    {
        public CheckTradeResponse(bool ok)
        {
            Ok = ok;
        }

        public bool Ok { get; }
    }
}