# basic calculator

this is a tiny, and simple CLI calculator that runs in your terminal.

## features

### two calculator modes
- **no memory mode** - resets after each operation (classic calculator behavior)
- **memory mode** - continuous operations with persistent memory storage

### operations
- basic arithmetic: add (+), subtract (-), multiply (* / x / ×), divide (/ / ÷)
- advanced math: power (^ / pow), square root (sqrt / √), reciprocal (1/x / inv)
- percent calculations:
  - **A op B%** - standard calculator percent (e.g., 200 + 10% = 220)
  - **A% of B** - find percentage (e.g., 10% of 200 = 20)

### memory mode features
- **set** - set memory to a specific value
- **clear** - reset memory to 0
- all operations automatically update memory
- view current memory value at all times

## how to build & run
- build: `dotnet build`
- run (dev): `dotnet run --project BasicCalculatorPart1`
- publish a single-file build (example for osx-x64):
  `dotnet publish -c Release -r osx-x64 --self-contained true /p:PublishSingleFile=true`

## where builds live
- look in `BasicCalculatorPart1/bin/Release/net10.0/`
- you should see platform folders like `osx-x64`, `osx-arm64`, `win-x64`, each has a `publish/` with the single-file binary

## usage
- when you start the calculator, choose between **no memory mode** (1) or **memory mode** (2)
- enter numbers using `.` as the decimal separator
- you can type operation numbers (1-9) or symbols/keywords (e.g., `+`, `sqrt`, `%`)
- type `0`, `q`, `quit`, or `exit` to return to mode menu or quit

## notes
- numbers use `.` as the decimal separator
- sqrt and fractional powers use `double` internally, so you might see tiny precision differences for some extreme values
- the program doesn't auto-clear the screen, so previous results stay visible for easy review
- input validation prevents common errors (division by zero, negative square roots, etc.)
