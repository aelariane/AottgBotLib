namespace AottgBotLib.Handlers
{
    /// <summary>
    /// Supported RPCs to add callbacks and it will work "from the box"
    /// </summary>
    /// <remarks>You do not have to add shortcuts for these, as they already built in</remarks>
    public class SupportedRPC
    {
        public const string Chat = "Chat";
        public const string GetRacingResult = "getRacingResult";
        public const string LoadLevelRPC = "RPCLoadLevel";
        public const string NetGameLose = "netGameLose";
        public const string NetGameWin = "netGameWin";
        public const string NetRefreshRacingResult = "netRefreshRacingResult";
        public const string OneTitanDown = "oneTitanDown";
        public const string RefreshStatus = "refreshStatus";
        public const string RequireStatus = "RequireStatus";
        public const string SomeOneIsDead = "someOneIsDead";
        public const string RefreshPvpStatus = "refreshPVPStatus";
        public const string RefreshAhssPvpStatus = "refreshPVPStatus_AHSS";
        public const string SetMyTeam = "setMyTeam";
        public const string Net3DmgSmoke = "new3DMGSMOKE";
        public const string ChatPM = "ChatPM";
        public const string NetDie = "netDie";
        public const string NetDie2 = "netDie2";
    }
}