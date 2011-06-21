using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaPortal.Player;

namespace MyPlexMedia.Plugin.Player {
    class MyPlexMediaPlayer : IPlayer{

        public override bool Play(string strFile) {
            return base.Play(strFile);
        }

        public override void Dispose() {
            throw new NotImplementedException();
        }
    }
}
