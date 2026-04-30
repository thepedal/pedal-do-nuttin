# Do Nuttin'

A managed effect machine for [ReBuzz](https://github.com/wasteddesign/ReBuzz) that passes stereo or mono audio through completely unmodified.

When the input is silent, the machine returns immediately without doing any work on the audio thread — consistent with the pattern used in Pedal Hallverb and other Pedal machines.

## Requirements

- [ReBuzz](https://github.com/wasteddesign/ReBuzz) (1812-preview or later)
- [.NET 10.0 Desktop Runtime (Windows x64)](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
- [.NET 10.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0) — to build from source

## Installation

1. Build from source (see below), or grab `Do Nuttin' NET.dll` from the [latest release](../../releases/latest).
2. Copy `Do Nuttin' NET.dll` into your ReBuzz Effects gear folder:
   ```
   C:\Program Files\ReBuzz\gear\Effects\
   ```
3. Restart ReBuzz. **Do Nuttin'** will appear in the Effects section of the machine list.

## Building from source

Adjust the `OutputPath` and `HintPath` in `DoNuttin/DoNuttin.csproj` if your ReBuzz installation is not at `C:\Program Files\ReBuzz\`, then run:

```powershell
dotnet build DoNuttin/DoNuttin.csproj -c Release -r win-x64
```

The output `Do Nuttin' NET.dll` is written directly to your ReBuzz Effects folder.

## Parameters

| Parameter | Values | Default | Description |
|-----------|--------|---------|-------------|
| Bypass    | no / yes | no | When set to **yes**, the machine outputs silence instead of passing audio through. |

## License

MIT
