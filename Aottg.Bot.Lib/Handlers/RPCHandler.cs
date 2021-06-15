using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using ExitGames.Client.Photon;
using Photon.Realtime;

namespace AottgBotLib.Handlers
{
    /// <summary>
    /// Handles event 200 that stands for RPC (Remote method call)
    /// </summary>
    public sealed class RPCHandler
    {
        private static readonly Dictionary<byte, string> _defaultShortcuts = new Dictionary<byte, string>()
        {
            { 13, SupportedRPC.NetDie },
            { 37, SupportedRPC.NetGameLose},
            { 50, SupportedRPC.NetDie2 },
            { 53, SupportedRPC.Net3DmgSmoke },
            { 62, SupportedRPC.Chat },
            { 83, SupportedRPC.LoadLevelRPC },
            { 85, SupportedRPC.NetGameWin },
            { 86, SupportedRPC.SomeOneIsDead },
            { 87, SupportedRPC.RequireStatus },
            { 88, SupportedRPC.RefreshStatus },
            { 90, SupportedRPC.OneTitanDown },
            { 94, SupportedRPC.GetRacingResult },
            { 95, SupportedRPC.NetRefreshRacingResult },
            { 98, SupportedRPC.RefreshPvpStatus },
            { 113, SupportedRPC.SetMyTeam },
            { 117, SupportedRPC.RefreshAhssPvpStatus },
        };

        private BotClient _client;
        private readonly Dictionary<string, Action<RPCArguments>> _callbacks = new Dictionary<string, Action<RPCArguments>>();
        private Dictionary<byte, string> _shortcuts = new Dictionary<byte, string>();

        /// <summary>
        /// All written shortcuts
        /// </summary>
        public IReadOnlyDictionary<byte, string> Shortcuts => _shortcuts;

        /// <summary>
        /// Contains all shortcuts that initialized by default, so you should not add them manually. They always writes to <seealso cref="Shortcuts"/>
        /// </summary>
        public static IReadOnlyDictionary<byte, string> DefaultShortcuts => _defaultShortcuts;

        internal RPCHandler(BotClient client)
        {
            _client = client;
            _shortcuts = new Dictionary<byte, string>(_defaultShortcuts);
        }

        internal void CheckRpcReceived(EventData data)
        {
            if (data.Code == 200)
            {
                Hashtable hash = data[245] as Hashtable;

                if (hash == null)
                {
                    return;
                }

                OnRPCReceived(hash, _client.CurrentRoom.GetPlayer(data.Sender));
            }
        }

        internal void OnRPCReceived(Hashtable hash, Player sender)
        {
            int viewId;
            string rpcName;
            object[] parameters;

            try
            {
                rpcName = "";
                if (hash.ContainsKey((byte)5))
                {
                    byte shortByte = (byte)hash[(byte)5];
                    if (!_shortcuts.ContainsKey(shortByte))
                    {
                        return;
                    }
                    rpcName = _shortcuts[shortByte];
                }
                else if (hash.ContainsKey((byte)3))
                {
                    rpcName = (string)hash[(byte)3];
                }
                else
                {
                    throw new InvalidOperationException("Not found any RPC Name keys");
                }
                viewId = (int)hash[(byte)0];
                parameters = (object[])hash[(byte)4];
            }
            catch (Exception ex)
            {
                Log.LogError($"Error while parsing RPC occured by ID {sender}\nException: {ex.Message}\n{ex.StackTrace}");
                return;
            }

            if (!_callbacks.TryGetValue(rpcName, out Action<RPCArguments> callback))
            {
                return;
            }

            var callArgs = new RPCArguments(_client, parameters, new RPCCallInfo { Sender = sender, ViewID = viewId });

            callback(callArgs);
        }

        /// <summary>
        /// Executes <paramref name="callback"/> when RPC with name <paramref name="rpcName"/> received
        /// </summary>
        /// <param name="rpcName">Method name</param>
        /// <param name="callback">Method that will be executed</param>
        public void AddCallback(string rpcName, Action<RPCArguments> callback)
        {
            if (rpcName == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            if (_callbacks.ContainsKey(rpcName))
            {
                _callbacks[rpcName] = callback;
            }
            else
            {
                _callbacks.Add(rpcName, callback);
            }
        }

        /// <summary>
        /// Adds byte to string shortcut
        /// </summary>
        /// <param name="b">Byte key</param>
        /// <param name="fullName">Full RPC name</param>
        public void AddShortcut(byte b, string fullName)
        {
            if (fullName == null)
            {
                throw new ArgumentNullException(nameof(fullName));
            }

            if (!_shortcuts.ContainsKey(b))
            {
                _shortcuts.Add(b, fullName);
            }
            else
            {
                _shortcuts[b] = fullName;
            }
        }

        /// <summary>
        /// Initializes <seealso cref="Shortcuts"/> collection from given collection
        /// </summary>
        /// <param name="shortcuts">Collecton of byte to string shortcuts</param>
        public void InitializeShortcuts(IEnumerable<KeyValuePair<byte, string>> shortcuts)
        {
            if (shortcuts == null)
            {
                throw new ArgumentNullException(nameof(shortcuts));
            }

            _shortcuts = new Dictionary<byte, string>(_shortcuts.Union(shortcuts));
        }
    }
}