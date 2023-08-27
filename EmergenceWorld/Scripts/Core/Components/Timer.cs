namespace EmergenceWorld.Scripts.Core.Components
{
    public class Timer
    {
        // in seconds
        public float Time { get; private set; }
        public float WaitTime { get; private set; }

        private Action timeout;

        private bool isRunning = false;

        public Timer(float waitTime, Action timeout)
        {
            WaitTime = waitTime;
            this.timeout = timeout;
        }

        public void Start()
        {
            isRunning = true;
        }

        public void Stop()
        {
            isRunning = false;
        }

        public void Step(float delta)
        {
            if (isRunning)
            {
                Time += delta;

                if (Time >= WaitTime)
                {
                    Time = 0;
                    timeout();
                }
            }
        }
    }
}
