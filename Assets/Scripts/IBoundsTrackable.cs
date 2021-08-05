using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts
{
    public interface IBoundsTrackable
    {
        public float BoundsOffset { get; }
        //public event Action OnTeleport;
    }
}
