# basic calculator

this is a tiny, and simple CLI calculator that runs in your terminal.

what it does
- add, subtract, multiply, divide
- power (^ / pow), square root (sqrt / √)
- reciprocal (1/x) and two percent modes (a op b% and a% of b)

how to build & run
- build: `dotnet build`
- run (dev): `dotnet run --project BasicCalculatorPart1`
- publish a single-file build (example for osx-x64):
  `dotnet publish -c Release -r osx-x64 --self-contained true /p:PublishSingleFile=true`

where builds live
- look in `BasicCalculatorPart1/bin/Release/net10.0/`
- you should see platform folders like `osx-x64`, `osx-arm64`, `win-x64`, each has a `publish/` with the single-file binary

notes
- numbers use `.` as the decimal separator
- sqrt and fractional powers use `double` internally, so you might see tiny precision differences for some extreme values
- the program doesn't auto-clear the screen, so previous results stay visible for easy review
