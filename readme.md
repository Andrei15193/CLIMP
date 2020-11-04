CLIMP (Command Line Interface Media Player)
-------------------------------------------

A [VLC](https://www.videolan.org/) wrapper for providing a media player through
the command line rather than a GUI. The project is configured to run as a
[dotnet tool](https://docs.microsoft.com/en-us/dotnet/core/tools/#tool-management-commands).

To install CLIMP you first need to download the source code (yeah, I know,
a GitHub Actions Workflow is on the to-do list to also publish packages to the
GitHub Feed of the latest build, continuous delivery, so that you don't need to
do this any more).

Simply clone the repository, switch to the project directory, pack it in a NuGet
and install it as a dotnet tool:

```
git clone https://github.com/Andrei15193/CLIMP.git
cd ./CLIMP
dotnet pack --configuration Release --output ./packages
dotnet tool install climp --global --add-source ./packages
```

In order to make climp work, you need to configure it. Currently you need to do
this manually (yeah I know, a config command is on the to-do list so this can
be done through the application). Create a file in your user profile directory
(in powershell you can use `$env:userProfile` to get the full path to it,
`echo %userProfile%` in cmd to do the same) named `.climp`. This is a JSON file
with the following structure:

```js
{
    "vlcExecutablePath": "C:\\Program Files (x86)\\VideoLAN\\VLC\\vlc.exe", // The path where VLC is installed, absolute path so it works from everywhere
    "mediaDirectories": [
        "C:\\Music" // media directories where to look for music and other media, absolute paths so they work from everywhere
    ]
}
```

Currently only `.m4a` files are being looked for (yeah, I know, extending the
media files that can be played is on the to-do list, it's
[VLC](https://www.videolan.org/) who's playing them anyway).

Once you have done this, you can enjoy your music from the command line by
starting climp and playing a song.

```
climp
```
```
> play my favourite song
```