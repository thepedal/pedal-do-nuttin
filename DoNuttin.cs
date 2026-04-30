// Do Nuttin' – ReBuzz managed effect machine
// Accepts mono or stereo audio in, passes it out completely unmodified.
// When the input is silent, returns false immediately so the audio thread
// does no work — consistent with the pattern used in Pedal Hallverb and
// other Pedal machines.
//
// Build:  dotnet build DoNuttin.csproj -c Release -r win-x64
// Deploy: copy "Do Nuttin' NET.dll"  →  <ReBuzz>\gear\Effects\

using Buzz.MachineInterface;

namespace WDE.DoNuttin
{
    [MachineDecl(Name = "Pedal Do Nuttin'", ShortName = "DoNut", Author = "WDE", MaxTracks = 0, InputCount = 1, OutputCount = 1)]
    public class DoNuttinMachine : IBuzzMachine
    {
        IBuzzMachineHost host;

        // Amplitude below which a block is considered silent.
        // 1e-6f ≈ −120 dBFS — well below any audible signal.
        const float SILENCE_THRESHOLD = 1e-6f;

        // How many consecutive silent blocks before we stop working.
        // 4 blocks is enough to confirm real silence while being
        // effectively instantaneous at any typical block size.
        const int SILENCE_BLOCKS_REQUIRED = 4;

        int _silentBlocks;

        public DoNuttinMachine(IBuzzMachineHost host)
        {
            this.host = host;
        }

        [ParameterDecl(ValueDescriptions = new[] { "no", "yes" }, DefValue = 0, Description = "Pass audio through")]
        public bool Bypass { get; set; }

        public bool Work(Sample[] output, Sample[] input, int n, WorkModes mode)
        {
            if (mode == WorkModes.WM_NOIO || Bypass)
            {
                _silentBlocks = 0;
                return false;
            }

            // Scan the input block for any signal above the silence threshold.
            float peak = 0f;
            for (int i = 0; i < n; i++)
            {
                float l = input[i].L < 0f ? -input[i].L : input[i].L;
                float r = input[i].R < 0f ? -input[i].R : input[i].R;
                if (l > peak) peak = l;
                if (r > peak) peak = r;
            }

            if (peak < SILENCE_THRESHOLD)
            {
                // Input is silent this block.
                if (++_silentBlocks >= SILENCE_BLOCKS_REQUIRED)
                    return false;   // no work, no output
            }
            else
            {
                // Signal present — reset the counter and fall through to copy.
                _silentBlocks = 0;
            }

            // Pass the input through to the output unchanged.
            for (int i = 0; i < n; i++)
                output[i] = input[i];

            return true;
        }
    }
}
