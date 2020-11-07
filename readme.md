CLIMP (Command Line Interface Media Player)
-------------------------------------------

A [VLC](https://www.videolan.org/) wrapper for providing a media player through
the command line rather than a GUI. The project is configured to run as a
[dotnet tool](https://docs.microsoft.com/en-us/dotnet/core/tools/#tool-management-commands).

To install CLIMP simply run the following command in your favourite terminal.

```
dotnet tool install climp --global
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