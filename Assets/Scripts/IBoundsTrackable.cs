using System;

namespace Scripts
{
    public interface IBoundsTrackable
    {
        public float BoundsOffset { get; }
        public void OnBoundsReached();
    }
}
