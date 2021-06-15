using System;
using System.Threading.Tasks;

namespace AottgBotLib.Logic
{
    public sealed class Round
    {
        private bool _gameEnd;
        private bool _roundWon;
        private bool _roundLost;
        private DateTime _lastTime;
        private System.Threading.CancellationToken _cancellationToken;

        /// <summary>
        /// Current round time. Resets after restart
        /// </summary>
        public float RoundTime { get; internal set; }

        /// <summary>
        /// If round ended with humanity victory
        /// </summary>
        public bool GameWon
        {
            get => _roundWon;
            set
            {
                if (value)
                {
                    _gameEnd = true;
                }
                _roundWon = value;
            }
        }

        /// <summary>
        /// If round ended with humanity loss
        /// </summary>
        public bool GameLose
        {
            get => _roundLost;
            set
            {
                if (value)
                {
                    _gameEnd = true;
                }
                _roundLost = value;
            }
        }

        /// <summary>
        /// If current round ended
        /// </summary>
        public bool GameEnd
        {
            get => _gameEnd;
            set
            {
                if(value == false && _gameEnd)
                {
                    GameLose = false;
                    GameWon = false;
                }
                _gameEnd = value;
            }
        }

        internal Round(System.Threading.CancellationToken cancelToken)
        {
            _lastTime = DateTime.Now;
            this._cancellationToken = cancelToken;
            Task.Run(UpdateTask);
        }

        internal void OnRestart()
        {
            RoundTime = 0f;
            GameEnd = false;
        }

        private async Task UpdateTask()
        {
            while (true)
            {
                await Task.Delay(100);
                DateTime now = DateTime.Now;
                float diff = ((now - _lastTime).Milliseconds / 1000f);
                RoundTime += diff;
                _lastTime = now;
                _cancellationToken.ThrowIfCancellationRequested();
            }
        }
    }
}
