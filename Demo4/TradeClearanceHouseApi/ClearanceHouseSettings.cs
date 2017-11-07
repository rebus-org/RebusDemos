using System;

namespace TradeClearanceHouseApi
{
    public class ClearanceHouseSettings
    {
        public ClearanceHouseSettings() => Mode = Mode.Normal;

        public void Toggle() => Mode = GetNextMode();

        public Mode Mode { get; private set; }

        Mode GetNextMode() => (Mode)(((int)Mode + 1) % Enum.GetValues(typeof(Mode)).Length);
    }

    public enum Mode
    {
        Normal,
        Unstable,
        InDistress,
        Slow,
    }
}