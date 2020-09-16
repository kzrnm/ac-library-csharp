#!/bin/bash

if ! command -v dotnet &> /dev/null
then
    if ! command -v dotnet.exe &> /dev/null
    then
        echo "dotnet command could not be found"
        exit
    else
        dotnet_command=$(command -v dotnet.exe)
    fi
else
    dotnet_command=$(command -v dotnet)
fi

# https://stackoverflow.com/questions/3504945/timeout-command-on-mac-os-x
function timeout() { perl -e 'alarm shift; exec @ARGV' "$@"; }

function filetime() {
if command -v GetFileInfo &> /dev/null
then
    GetFileInfo -m "$@" # Mac
else
    date +%s -r "$@"
fi
}


DIR="$(dirname $0)"

while true
do
    LAST_WRITE=$(filetime "$DIR/Program.cs")
    if [ "$PREV_LAST_WRITE" != "$LAST_WRITE" ]
    then
        echo "start  expand: $(date)"
        "$dotnet_command" build "$DIR" -c Release --verbosity q
        timeout 5 "$dotnet_command" run -c Release --no-build -p "$DIR"
        echo "finish expand: $(date)"
        PREV_LAST_WRITE="$LAST_WRITE"
    fi
    sleep 5
done

