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

In order to make climp work, you need to configure it, to do this simply run
`climp config --vlc-path <path to VLC executable> --media-directories <media directory 1> <media directory 2>`

Currently only `.m4a` files are being looked for (yeah, I know, extending the
media files that can be played is on the to-do list, it's
[VLC](https://www.videolan.org/) who's playing them anyway).

Once you have done this, you can enjoy your music from the command line by
starting telling climp to play a song.

```
climp play my favourite song
```

For more information about what commands are available run `climp help`