using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFSoundVisualizationLib;

namespace WatchCheck
{
    class WaveFormI : IWaveformPlayer
    {
        public double ChannelPosition { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public double ChannelLength => throw new NotImplementedException();

        public float[] WaveformData => throw new NotImplementedException();

        public TimeSpan SelectionBegin { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public TimeSpan SelectionEnd { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool IsPlaying => throw new NotImplementedException();

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
